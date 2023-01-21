using LiteNetLib.Utils;
using NetworkShared;

namespace Networkshared.Packets.ClientServer
{
    public struct Net_SurrenderRequest : INetPacket
    {
        public PacketType Type => PacketType.SurrenderRequest;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
