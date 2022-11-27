using LiteNetLib.Utils;
using NetworkShared;

namespace TTT.Server.NetworkShared.Packets.ServerClient
{
    public struct Net_OnAuth : INetPacket
    {
        public PacketType Type => PacketType.OnAuth;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
