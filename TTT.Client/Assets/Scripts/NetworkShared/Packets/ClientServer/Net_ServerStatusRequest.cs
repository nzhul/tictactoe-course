using LiteNetLib.Utils;
using NetworkShared;

namespace Networkshared.Packets.ClientServer
{
    public struct Net_ServerStatusRequest : INetPacket
    {
        public PacketType Type => PacketType.ServerStatusRequest;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
