using Fantasy.Entitas;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Fantasy
{
    public class ClientManager: Fantasy.Entitas.Entity
    {
        public Dictionary<long, Client> clientDic = new();

        public void AddClient(Client client)
        {
            if (clientDic.ContainsKey(client.Id))
            {
                return;
            }

            clientDic.Add(client.Id, client);
            Log.Debug(client.Id.ToString());
        }

        public void BroadCast<T>(T message) where T:IMessage
        {
            //if(message is OneFrameCommand oneFrameCommand)
            //{
            //    Log.Debug(oneFrameCommand.frameId.ToString());
            //}
            foreach (Client client in clientDic.Values)
            {
                if ((Session)client.session == null)
                {
                    continue;
                }

                ((Session)client.session).Send(message);
            }
        }

        /// <summary>
        /// 重置客户端管理器
        /// </summary>
        public void Clear()
        {
            clientDic.Clear();
        }
    }
}
