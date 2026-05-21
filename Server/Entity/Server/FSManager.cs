
using Fantasy.Async;
using Fantasy.DataStructure.Collection;
using Fantasy.Entitas;
using MongoDB.Driver;

namespace Fantasy
{
    public class FSManager: Fantasy.Entitas.Entity
    {
        public int frameId=0;
        public int startCount=2;
        public const int frameTime = 1000 / 30;
        public OneFrameCommand frameCommand=new();
        private global::Timer timer = new();
        public bool isSync = false;

        public void ReceiveCommand(Command command)
        {
            //Log.Debug((frameId- command.frameId).ToString());
            //如果命令的帧号和当前帧相隔太久就直接丢弃
            if (command.frameId > frameId + 3 || command.frameId < frameId - 3)
            {
                return;
            }

            frameCommand.commands.Add(command);
        }

        public void StartFrameSync()
        {
            if (Scene.GetComponent<ClientManager>().clientDic.Count < startCount&&!isSync)
            {
                return;
            }
            isSync = true;

            //通知客户端开始帧同步
            G2C_StartFrameSync  startSyncMessage= new G2C_StartFrameSync();
            foreach(long id in Scene.GetComponent<ClientManager>().clientDic.Keys)
            {
                startSyncMessage.clientIdList.Add(id);
                startSyncMessage.positionList.Add(new Position() { x = 10000, y = 10000 });
                startSyncMessage.positionList.Add(new Position() { x = -10000, y = -10000 });
            }
            Scene.GetComponent<ClientManager>().BroadCast<G2C_StartFrameSync>(startSyncMessage);

            timer.StartRepeatTimer(frameTime, FrameUpdate);
            Log.Debug("开始帧同步");
        }

        public void FrameUpdate()
        {
            //处理当前帧命令
            SendFrameCommand();

            //重置
            frameCommand = new OneFrameCommand();

            //增加序号
            frameId++;
        }

        public void SendFrameCommand()
        {
            //发送上一帧收集到的命令
            frameCommand.frameId = frameId;
            Scene.GetComponent<ClientManager>().BroadCast(frameCommand);
            //foreach(Command command in frameCommand.commands)
            //{
            //    Log.Debug($"clientId:{command.clientId} commandType:{command.commandType} x:{command.x} y:{command.y} z:{command.z}");
            //}
        }

        /// <summary>
        /// 停止同步
        /// </summary>
        public void StopSync()
        {
            timer.End();
            frameId = 0;
            frameCommand.commands.Clear();
            isSync = false;
        }
    }
}
