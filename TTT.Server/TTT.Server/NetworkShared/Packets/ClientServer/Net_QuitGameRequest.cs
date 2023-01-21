using LiteNetLib.Utils;
using NetworkShared;

namespace Networkshared.Packets.ClientServer
{
    public struct Net_QuitGameRequest : INetPacket
    {
        public PacketType Type => PacketType.QuitGameRequest;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
