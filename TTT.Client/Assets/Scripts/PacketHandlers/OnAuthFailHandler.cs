using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using System;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthFail)]
    public class OnAuthFailHandler : IPacketHandler
    {
        public static event Action<Net_OnAuthFail> OnAuthFail;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnAuthFail)packet;
            OnAuthFail?.Invoke(msg);
        }
    }
}
