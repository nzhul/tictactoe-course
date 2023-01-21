using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using TTT.Server.Games;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.QuitGameRequest)]
    public class QuitGameRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GamesManager _gamesManager;
        private readonly NetworkServer _server;

        public QuitGameRequestHandler(
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
            var conn = _usersManager.GetConnection(connectionId);

            var rmsg = new Net_OnQuitGame
            {
                Quitter = conn.User.Id
            };

            if (_gamesManager.GameExists(conn.User.Id))
            {
                var closedGame = _gamesManager.CloseGame(conn.User.Id);
                var opponent = closedGame.GetOpponent(conn.User.Id);
                var opponentConn = _usersManager.GetConnection(opponent);
                _server.SendClient(opponentConn.ConnectionId, rmsg);
            }

            _server.SendClient(conn.ConnectionId, rmsg);
        }
    }
}
