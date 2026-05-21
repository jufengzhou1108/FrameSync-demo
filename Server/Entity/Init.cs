using Fantasy;
using Fantasy.Async;
using Fantasy.Event;

namespace Entity
{
    public class Init : AsyncEventSystem<OnCreateScene>
    {
        protected override async FTask Handler(OnCreateScene self)
        {
            self.Scene.AddComponent<ClientManager>();
            self.Scene.AddComponent<FSManager>();

            await FTask.CompletedTask;
        }
    }
}
