using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using TTT.Server.Data;
using TTT.Server.Games;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.ServerStatusRequest)]
    public class ServerStatusRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly NetworkServer _server;
        private readonly IUserRepository _userRepository;

        public ServerStatusRequestHandler(
            UsersManager usersManager,
            NetworkServer server,
            IUserRepository userRepository)
        {
            _usersManager = usersManager;
            _server = server;
            _userRepository = userRepository;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var rmsg = new Net_OnServerStatus
            {
                PlayersCount = _userRepository.GetTotalCount(),
                TopPlayers = _usersManager.GetTopPlayers()
            };

            _server.SendClient(connectionId, rmsg);
        }
    }
}
