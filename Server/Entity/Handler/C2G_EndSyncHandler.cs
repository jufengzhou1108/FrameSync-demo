using Fantasy.Network.Interface;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;

public class C2G_EndSyncHandler : Message<C2G_EndSync>
{
    protected override async FTask Run(Session session, C2G_EndSync message)
    {
        session.Scene.GetComponent<FSManager>().StopSync();
        session.Scene.GetComponent<ClientManager>().Clear();

        await FTask.CompletedTask;
    }
}