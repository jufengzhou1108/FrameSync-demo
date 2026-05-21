using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Fantasy
{
    public class C2G_JoinRequestHandler : MessageRPC<C2G_JoinRequest, G2C_JoinResponse>
    {
        protected override async FTask Run(Session session, C2G_JoinRequest request, G2C_JoinResponse response, Action reply)
        {
            //如果已经开始帧同步那么拒绝添加
            if (session.Scene.GetComponent<FSManager>().isSync)
            {
                return;
            }

            //添加客户端
            ClientManager clientManager = session.Scene.GetComponent<ClientManager>();
            Client client = Scene.Create<Client>(session.Scene,true,true);
            client.session = session;
            clientManager.AddClient(client);

            //尝试开启帧同步 
            session.Scene.GetComponent<FSManager>().StartFrameSync();

            //处理返回消息
            response.id = client.Id;

            await FTask.CompletedTask;
        }
    }
}
