using Microsoft.Extensions.Logging;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using TTT.Server.Games;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.AcceptPlayAgainRequest)]
    public class AcceptPlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GamesManager _gamesManager;
        private readonly ILogger<AcceptPlayAgainRequestHandler> _logger;
        private readonly NetworkServer _server;

        public AcceptPlayAgainRequestHandler(
            UsersManager usersManager,
            GamesManager gamesManager,
            ILogger<AcceptPlayAgainRequestHandler> logger,
            NetworkServer server)
        {
            _usersManager = usersManager;
            _gamesManager = gamesManager;
            _logger = logger;
            _server = server;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            var userId = connection.User.Id;
            var game = _gamesManager.FindGame(userId);
            game.SetRematchReadiness(userId);

            if (!game.BothPlayersReady())
            {
                _logger.LogWarning("Bad state! Players are not ready!");
            }

            game.NewRound();

            var opponentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(opponentId);

            var rmsg = new Net_OnNewRound();
            _server.SendClient(connection.ConnectionId, rmsg);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);
        }
    }
}
