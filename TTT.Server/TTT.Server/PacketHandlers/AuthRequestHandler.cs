using NetworkShared;
using NetworkShared.Attributes;
using System;
using TTT.Server.NetworkShared;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            throw new NotImplementedException();
        }
    }
}
