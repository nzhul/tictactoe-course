using LiteNetLib.Utils;

namespace NetworkShared.Packets.ClientServer
{
    public struct Net_MarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkCellRequest;

        public byte Index { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Index = reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Index);
        }
    }
}
