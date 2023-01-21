using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerClient
{
    public struct Net_OnSurrender : INetPacket
    {
        public PacketType Type => PacketType.OnSurrender;

        public string Winner { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Winner = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Winner);
        }
    }
}
