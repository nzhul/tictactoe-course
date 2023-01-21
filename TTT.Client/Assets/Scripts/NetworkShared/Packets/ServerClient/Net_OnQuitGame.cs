using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerClient
{
    public struct Net_OnQuitGame : INetPacket
    {
        public PacketType Type => PacketType.OnQuitGame;

        public string Quitter { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Quitter = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Quitter);
        }
    }
}
