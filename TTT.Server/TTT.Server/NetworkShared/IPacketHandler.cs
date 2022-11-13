using NetworkShared;

namespace TTT.Server.NetworkShared
{
    public interface IPacketHandler
    {
        void Handle(INetPacket packet, int connectionId);
    }
}
