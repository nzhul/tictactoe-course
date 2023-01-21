using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using TTT.Server.Games;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.SurrenderRequest)]
    public class SurrenderRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GamesManager _gamesManager;
        private readonly NetworkServer _server;

        public SurrenderRequestHandler(
            UsersManager usersManager, 
            GamesManager gamesManager,
            NetworkServer server)
        {
            _usersManager = usersManager;
            _gamesManager = gamesManager;
            _server = server;
        }


        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            var game = _gamesManager.FindGame(connection.User.Id);
            var opponentId = game.GetOpponent(connection.User.Id);
            game.AddWin(opponentId);
            _usersManager.IncreaseScore(opponentId);

            var rmsg = new Net_OnSurrender
            {
                Winner = opponentId
            };

            var opponentConnection = _usersManager.GetConnection(opponentId);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);
            _server.SendClient(connectionId, rmsg);
        }
    }
}
