using LiteNetLib.Utils;
using NetworkShared;

namespace Networkshared.Packets.ClientServer
{
    public class Net_AuthRequest : INetPacket
    {
        public PacketType Type => PacketType.AuthRequest;

        public string Username { get; set; }

        public string Password { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Username = reader.GetString();
            Password = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Username);
            writer.Put(Password);
        }
    }
}
