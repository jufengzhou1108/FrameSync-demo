using Fantasy;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家的物理状态脚本
/// </summary>
public class PlayerState : State
{
    public int x;                //x和y都是mm
    public int y;
    public int speed=5;          //1ms走的mm数
    public Player player;
    public Vector2 dir=Vector2.zero;
    public int width = 1000;     //物体的宽度和高度用来碰撞检测
    public int height = 1000;
    public int hp = 100;
    public GamePanel gamePanel;     //游戏面板

    /// <summary>
    /// 玩家的逻辑帧更新
    /// </summary>
    /// <param name="command"></param>
    public override void LogicUpdate()
    {
        x += (int) (speed* dir.x* 33);
        y += (int) (speed *dir.y* 33);
        if (NetManager.Instance.Id == this.id)
        {
            gamePanel.UpdateHp(hp);
        }

        player.ViewUpdte(new Vector2(x / 1000f, y / 1000f));
    }

    /// <summary>
    /// 处理接受到的命令
    /// </summary>
    /// <param name="command"></param>
    public void HandleCommand(Command command)
    {
        //处理移动
        if (command.commandType == 2)
        {
            if (command.x != 0)
            {
                dir.x = command.x / 1000f;
            }
            else
            {
                dir.x = 0;
            }

            if (command.z != 0)
            {
                dir.y = command.z / 1000f;
            }
            else
            {
                dir.y = 0;
            }
        }

        //处理攻击
        if (command.commandType == 3)
        {
            //Debug.Log((command.x - x) / 1000f+" "+ (command.y - y) / 1000f);
            StateManager.Instance.AddBullet(x, y, new Vector2((command.x - x)/1000f, (command.y - y)/1000f),command.clientId);
        }
    }

    public override SnapShot GetShot()
    {
        PlayerSnapShot playerSnapShot = new();
        playerSnapShot.x = x;
        playerSnapShot.y = y;
        playerSnapShot.dir = dir;
        playerSnapShot.speed = speed;
        playerSnapShot.id = id;
        playerSnapShot.hp = hp;

        return playerSnapShot;
    }

    public override void ApplyShot(SnapShot snapShot)
    {
        PlayerSnapShot playerSnapShot = snapShot as PlayerSnapShot;
        x = playerSnapShot.x;
        y = playerSnapShot.y;
        speed = playerSnapShot.speed;
        dir = playerSnapShot.dir;
        hp = playerSnapShot.hp;

        player.QuickMove(new Vector2(x/1000f, y/ 1000f));
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="harmness"></param>
    public void Attacked(int harmness)
    {
        hp = Mathf.Max(hp - harmness, 0);
        if (hp == 0)
        {
            GameManager.Instance.EndGame(id);
        }
    }
}