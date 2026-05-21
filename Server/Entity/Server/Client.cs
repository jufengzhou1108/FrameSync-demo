using Fantasy.Entitas;
using Fantasy.Network;

namespace Fantasy
{
    public class Client:Fantasy.Entitas.Entity
    {
        public EntityReference<Session> session;
    }
}
