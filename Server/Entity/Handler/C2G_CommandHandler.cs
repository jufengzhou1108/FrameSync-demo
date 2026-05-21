using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Fantasy
{
    public class CommandHandler : Message<Command>
    {
        protected override async FTask Run(Session session, Command message)
        {
            session.Scene.GetComponent<FSManager>().ReceiveCommand(message);
        }
    }
}
