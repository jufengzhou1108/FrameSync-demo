using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class G2C_StartFrameSyncHandler : Message<G2C_StartFrameSync>
{
    protected override async FTask Run(Session session, G2C_StartFrameSync message)
    {
        //保证对局已初始化 
        while (!GameManager.Instance.isStart)
        {
            await Task.Run(()=>SpinWait.SpinUntil(() => GameManager.Instance.isStart));
        }
        FSManager.Instance.StartFrameSync();
        GameManager.Instance.StartGame();

        //场景初始化
        for(int i=0;i<2;i++)
        {
            StateManager.Instance.AddPlayer(message.clientIdList[i], message.positionList[i]);
        }

        Log.Debug("开始帧同步");

        await FTask.CompletedTask;
    }
}