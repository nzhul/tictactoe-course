using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerClient
{
    public struct Net_OnServerStatus : INetPacket
    {
        public PacketType Type => PacketType.OnServerStatus;

        public ushort PlayersCount { get; set; }

        public PlayerNetDto[] TopPlayers { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            PlayersCount = reader.GetUShort();

            var topPlayersLenght = reader.GetUShort();
            TopPlayers = new PlayerNetDto[topPlayersLenght];
            for (int i = 0; i < topPlayersLenght; i++)
            {
                TopPlayers[i] = reader.Get<PlayerNetDto>();
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(PlayersCount);

            writer.Put((ushort)TopPlayers.Length);
            for (int i = 0; i < TopPlayers.Length; i++)
            {
                writer.Put(TopPlayers[i]);
            }
        }
    }

    public struct PlayerNetDto : INetSerializable
    {
        public string Username { get; set; }

        public ushort Score { get; set; }

        public bool IsOnline { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Username = reader.GetString();
            Score = reader.GetUShort();
            IsOnline = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Username);
            writer.Put(Score);
            writer.Put(IsOnline);
        }
    }
}
