using Microsoft.Extensions.Logging;
using Networkshared.Packets.ClientServer;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using TTT.Server.Data;
using TTT.Server.Games;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        private readonly ILogger<AuthRequestHandler> _logger;
        private readonly UsersManager _usersManager;
        private readonly NetworkServer _server;
        private readonly IUserRepository _userRepository;

        public AuthRequestHandler(
            ILogger<AuthRequestHandler> logger,
            UsersManager usersManager,
            NetworkServer server,
            IUserRepository userRepository)
        {
            _logger = logger;
            _usersManager = usersManager;
            _server = server;
            _userRepository = userRepository;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_AuthRequest)packet;

            _logger.LogInformation($"Received login request for user: {msg.Username} with pass: {msg.Password}");

            var loginSuccess = _usersManager.LoginOrRegister(connectionId, msg.Username, msg.Password);

            INetPacket rmsg;

            if (loginSuccess)
            {
                rmsg = new Net_OnAuth();
            }
            else
            {
                rmsg = new Net_OnAuthFail();
            }

            _server.SendClient(connectionId, rmsg);

            if (loginSuccess)
            {
                NotififyOtherPlayers(connectionId);
            }
        }

        private void NotififyOtherPlayers(int excludedConnectionId)
        {
            var rmsg = new Net_OnServerStatus
            {
                PlayersCount = _userRepository.GetTotalCount(),
                TopPlayers = _usersManager.GetTopPlayers(),
            };

            var otherIds = _usersManager.GetOtherConnectionIds(excludedConnectionId);

            foreach (var connectionId in otherIds)
            {
                _server.SendClient(connectionId, rmsg);
            }
        }
    }
}
