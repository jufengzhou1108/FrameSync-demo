using Fantasy;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 命令管理器，负责命令的缓存，帧预测和帧回滚的分发
/// </summary>
public class CommandManager:MonoBehaviour
{
    private List<OneFrameCommand> frameCommandList = new();         //待处理帧列表 
    private long lastRealId=-1;      //最后一次权威更新的帧号
    private OneFrameCommand virtualCommand = new(); //虚拟帧，在本游戏中恒为空
    
    public static CommandManager Instance { get; set; }
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField]
    private StateManager stateManager;

    /// <summary>
    /// 每帧处理
    /// </summary>
    public void FrameUpdate()
    {
        //处理所有帧命令
        foreach(OneFrameCommand oneFrameCommand in frameCommandList)
        {
            HandleCommand(oneFrameCommand);
        }

        frameCommandList.Clear();

        //预测到当前帧
        while (lastRealId + stateManager.VirtualStatesCount < FSManager.Instance.frameId)
        {
            stateManager.HandleCommand(virtualCommand, false);
        }
    }

    /// <summary>
    /// 处理输入命令 
    /// </summary>
    /// <param name="oneFrameCommand"></param>
    public void HandleCommand(OneFrameCommand oneFrameCommand)
    {
        //如果和最后一次更新实帧的帧号+1不同，说明发生丢帧，开始断线重连
        if (oneFrameCommand.frameId != lastRealId+1)
        {
            Reconnect();
            return;
        }

       if (stateManager.VirtualStatesCount >0 && oneFrameCommand.commands.Count!=0)
       {   
           //如果命令和当前帧不是空命令,进行回滚
           stateManager.ClearVirtualstates();
           stateManager.BackFramSate();
       }

        //帧更新
        stateManager.HandleCommand(oneFrameCommand);

        lastRealId++;
    }


    /// <summary>
    /// 开始断线重连 
    /// </summary>
    public void Reconnect()
    {
        Log.Debug("开始断线重连");
    }

    /// <summary>
    /// 添加收到的帧  
    /// </summary>
    /// <param name="oneFrameCommand"></param>
    public void AddCommand(OneFrameCommand oneFrameCommand)
    {
        frameCommandList.Add(oneFrameCommand);
    }

    /// <summary>
    /// 停止帧同步
    /// </summary>
    public void StopUpdate()
    {
        frameCommandList.Clear();
        lastRealId = -1;
    }
}
