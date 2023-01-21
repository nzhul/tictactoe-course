using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using System;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnSurrender)]
    public class OnSurrenderHandler : IPacketHandler
    {
        public static event Action<Net_OnSurrender> OnSurrender;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnSurrender)packet;
            OnSurrender?.Invoke(msg);
        }
    }
}
