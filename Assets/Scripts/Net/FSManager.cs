using Fantasy;
using Fantasy.Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 帧同步管理器，负责帧同步的调度 
/// </summary>
public class FSManager : MonoBehaviour
{
    public int frameId = 0;
    public int frameTime = 1000 / 30;
    public CommandManager commandManager;
    private UnityTimer timer = new();

    private static FSManager instance;
    public static FSManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 开启帧同步
    /// </summary>
    public void StartFrameSync()
    {
        timer.StartRepeatTimer(frameTime, FrameUpdate);
    }

    /// <summary>
    /// 每帧更新处理 
    /// </summary>
    public void FrameUpdate()
    {
        Debug.Log(1);
        //进行帧更新
        //Debug.Log(frameId);
        commandManager.FrameUpdate();

        frameId++;
    }

    public void StopSync()
    {
        timer.End();
        frameId = 0;
        commandManager.StopUpdate();
        StateManager.Instance.Clear();
    }
}
