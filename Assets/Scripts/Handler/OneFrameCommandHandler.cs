using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

public class OneFrameCommandHandler : Message<OneFrameCommand>
{
    protected override async FTask Run(Session session, OneFrameCommand message)
    {
        CommandManager.Instance.AddCommand(message);

        await FTask.CompletedTask;
    }
}
