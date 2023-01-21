using NetworkShared;
using NetworkShared.Attributes;
using System;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnPlayAgain)]
    public class OnPlayAgainHandler : IPacketHandler
    {
        public static event Action OnPlayAgain;

        public void Handle(INetPacket packet, int connectionId)
        {
            OnPlayAgain?.Invoke();
        }
    }
}
