using Fantasy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 状态管理器，负责状态的回滚和逻辑帧更新
/// </summary>
public class StateManager:MonoBehaviour
{
    private Queue<OneFrameShot> stateQueue = new();    //虚帧的快照缓存队列（用来回滚）
    private OneFrameShot lastFrameShot=new();               //最后一次的实帧状态（用来回滚）
    private Dictionary<long, State> playerStateDic = new();    //玩家状态管理字典(用来进行逻辑更新)
    private Dictionary<long, State> bulletStateDic = new();     //物体状态管理字典
    [SerializeField]
    private List<Transform> walls = new();                    //墙面管理列表
    private int maxNum = 100;                           //最大的虚帧状态缓存数量
    private int id = 0;

    public GamePanel gamePanel;

    public static StateManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 捕获当前的所有物体的状态
    /// </summary>
    private OneFrameShot GetShot()
    {
        OneFrameShot oneFrameShot = new();
        foreach(State state in playerStateDic.Values)
        {
            oneFrameShot.shotList.Add(state.GetShot());
        }

        foreach(State state in bulletStateDic.Values)
        {
            oneFrameShot.shotList.Add(state.GetShot());
        }

        return oneFrameShot;
    }

    public int VirtualStatesCount => stateQueue.Count;

    /// <summary>
    /// 负责帧处理 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isReal">是否为实帧更新</param>
    public void HandleCommand(OneFrameCommand oneFrameCommand, bool isReal=true)
    {
        if (isReal)
        {
            //如果有虚帧状态那么直接把第一个虚帧状态作为实帧状态
            if (stateQueue.Count > 0)
            {
                lastFrameShot = stateQueue.Dequeue();
                return;
            }

            LogicUpdate(oneFrameCommand);
            lastFrameShot = GetShot();
        }
        else
        {
            LogicUpdate(oneFrameCommand);
            stateQueue.Enqueue(GetShot());

            if (stateQueue.Count > maxNum)
            {
                //如果虚帧状态过多说明网络情况很差，开始断线重连 
                CommandManager.Instance.Reconnect();
            }
        }
    }

    /// <summary>
    /// 负责逻辑帧更新
    /// </summary>
    /// <param name="command"></param>
    private void LogicUpdate(OneFrameCommand oneFrameCommand)
    {
        //处理命令
        foreach(Command command in oneFrameCommand.commands)
        {
            (playerStateDic[command.clientId] as PlayerState).HandleCommand(command);
        }

        //更新逻辑
        foreach(State state in playerStateDic.Values)
        {
            state.LogicUpdate();
        }
        foreach (State state in bulletStateDic.Values)
        {
            state.LogicUpdate();
        }

        //更新碰撞
        ColliderTool.FrameCollide(playerStateDic.Values.Select(state=>state as PlayerState).ToList(), walls, bulletStateDic.Values.Select(state => state as BulletState).ToList());
    }

    /// <summary>
    /// 清空所有的虚帧状态 
    /// </summary>
    public void ClearVirtualstates()
    {
        stateQueue.Clear();
    }

    /// <summary>
    /// 回滚到最后一次实帧的状态 
    /// </summary>
    public void BackFramSate()
    {
        //销毁回滚状态没有的子弹
        foreach(State state in bulletStateDic.Values)
        {
            bool isHave = false;
            foreach(SnapShot snapShot in lastFrameShot.shotList)
            {
                if(snapShot is BulletSnapShot bulletSnapShot&&bulletSnapShot.id==state.id)
                {
                    isHave = true;
                    break;
                }
            }
            
            if (!isHave)
            {
                RemoveBullet(state as BulletState);
            }
        }

        foreach(SnapShot shot in lastFrameShot.shotList)
        {
            ApplyShot(shot);
        }
    }

    /// <summary>
    /// 应用一个快照 
    /// </summary>
    private void ApplyShot(SnapShot shot)
    {
        if(shot is PlayerSnapShot playerSnapShot)
        {
            playerStateDic[playerSnapShot.id].ApplyShot(shot);
        }

        if(shot is BulletSnapShot bulletSnapShot)
        {
            if (!bulletStateDic.ContainsKey(shot.id))
            {
                //如果子弹不存在就创造出来
                AddBullet(bulletSnapShot.x, bulletSnapShot.y, bulletSnapShot.dir, bulletSnapShot.ownerId);
            }
            else
            {
                bulletStateDic[bulletSnapShot.id].ApplyShot(shot);
            }
        }
    }

    /// <summary>
    /// 添加玩家
    /// </summary>
    public void AddPlayer(long id, Fantasy.Position position)
    {
        AddressableManager.Instance.LoadResAsync<GameObject>("Player", (obj) =>
        {
            PlayerState playerState = new();
            GameObject newObj = Instantiate(obj);
            playerState.id = id;
            playerState.player = newObj.GetComponent<Player>();
            newObj.GetComponent<MeshRenderer>().material = playerState.id==NetManager.Instance.Id? AddressableManager.Instance.LoadRes<Material>("Green"): AddressableManager.Instance.LoadRes<Material>("Red");
            playerStateDic.Add(playerState.id, playerState);
            playerState.x = position.x;
            playerState.y = position.y;
            newObj.transform.position = new Vector3(position.x / 1000f, 1, position.y / 1000f);
            if (playerState.id == NetManager.Instance.Id)
            {
                CameraManager.Instance.AddPlayerTrans(newObj.transform);
                playerState.gamePanel = gamePanel;
            }
        });
    }

    /// <summary>
    /// 添加子弹
    /// </summary>
    public void AddBullet(int x,int y,Vector2 dir,long id)
    {
        AddressableManager.Instance.LoadResAsync<GameObject>("Bullet", (obj) =>
        {
            BulletState bulletState = new(x,y,dir);
            GameObject newObj = Instantiate(obj, new Vector3(x / 1000f, 1, y / 1000f),Quaternion.identity);
            bulletState.id = GetBulletId();
            bulletState.bullet = newObj.GetComponent<Bullet>();
            bulletState.bullet.Init(new Vector2(x / 1000f, y / 1000f));
            bulletState.bornFrame = FSManager.Instance.frameId;
            bulletState.ownerId = id;
            bulletStateDic.Add(bulletState.id, bulletState);
        });
    }

    /// <summary>
    /// 获取一个物体id
    /// </summary>
    /// <returns></returns>
    public int GetBulletId()
    {
        id++;
        return id;
    }

    /// <summary>
    /// 移除子弹
    /// </summary>
    public void RemoveBullet(BulletState bulletState)
    {
        //销毁渲染子弹
        Destroy(bulletState.bullet.gameObject);
        //销毁逻辑子弹
        bulletStateDic.Remove(bulletState.id);
    }

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="playerState"></param>
    public void RemovePlayer(PlayerState playerState)
    {
        Destroy(playerState.player.gameObject);
        playerStateDic.Remove(playerState.id);
    }

    /// <summary>
    /// 重置状态管理器
    /// </summary>
    public void Clear()
    {
        List<State> playerStateList = playerStateDic.Values.ToList();
        List<State> bulletStateList = bulletStateDic.Values.ToList();

        for(int i= bulletStateList.Count-1;i>-1;i--)
        {
            BulletState bulletState = bulletStateList[i] as BulletState;
            RemoveBullet(bulletState);
        }
        for (int i = playerStateList.Count - 1; i > -1; i--)
        {
            PlayerState playerState = playerStateList[i] as PlayerState;
            RemovePlayer(playerState);
        }
        stateQueue.Clear();
        lastFrameShot.shotList.Clear() ;
    }
}
