using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    public Session session;
    public Scene scene;
    public long Id;

    private static NetManager instance;
    public static NetManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartAsync().Coroutine();
    }

    public async FTask StartAsync()
    {
        //创建链接
        await Fantasy.Platform.Unity.Entry.Initialize();
        scene = await Scene.Create(SceneRuntimeMode.MainThread);
        session = scene.Connect("127.0.0.1:20000", NetworkProtocolType.KCP, OnConnectComplete, OnConnectFail, OnConnectDisconnect,false,5000);

        //发送心跳
        session.AddComponent<SessionHeartbeatComponent>().Start(5000, 8000);
    }

    public void OnConnectComplete()
    {
        Debug.Log("Fantasy初始化成功");
    }

    public void OnConnectFail()
    {
        Debug.Log("Fantasy初始化失败");
    }

    public void OnConnectDisconnect()
    {
        Debug.Log("Fantasy断开连接");
    }

    private void OnDestroy()
    {
        scene?.Dispose();
    }
}


