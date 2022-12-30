using NetworkShared;
using NetworkShared.Attributes;
using TTT.Server.Games;
using TTT.Server.Matchmaking;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.FindOpponentRequest)]
    public class FindOpponentRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly Matchmaker _matchmaker;

        public FindOpponentRequestHandler(UsersManager usersManager, Matchmaker matchmaker)
        {
            _usersManager = usersManager;
            _matchmaker = matchmaker;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            _matchmaker.RegisterPlayer(connection);
        }
    }
}
