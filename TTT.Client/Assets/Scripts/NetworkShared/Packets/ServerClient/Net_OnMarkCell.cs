using LiteNetLib.Utils;
using NetworkShared.Models;

namespace NetworkShared.Packets.ServerClient
{
    public struct Net_OnMarkCell : INetPacket
    {
        public PacketType Type => PacketType.OnMarkCell;

        public string Actor { get; set; }

        public byte Index { get; set; }

        public MarkOutcome Outcome { get; set; }

        public WinLineType WinLineType { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Actor = reader.GetString();
            Index = reader.GetByte();
            Outcome = (MarkOutcome)reader.GetByte();
            WinLineType = (WinLineType)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Actor);
            writer.Put(Index);
            writer.Put((byte)Outcome);
            writer.Put((byte)WinLineType);
        }
    }
}
