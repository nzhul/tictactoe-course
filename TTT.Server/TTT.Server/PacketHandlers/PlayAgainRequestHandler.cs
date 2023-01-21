using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using TTT.Server.Games;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.PlayAgainRequest)]
    public class PlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GamesManager _gamesManager;
        private readonly NetworkServer _server;

        public PlayAgainRequestHandler(
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
            var userId = connection.User.Id;
            var game = _gamesManager.FindGame(userId);
            game.SetRematchReadiness(userId);

            var rmsg = new Net_OnPlayAgain();

            var opponentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(opponentId);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);
        }
    }
}
