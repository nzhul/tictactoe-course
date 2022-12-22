using LiteNetLib;
using NetworkShared.Packets.ServerClient;
using System.Collections.Generic;
using System.Linq;
using TTT.Server.Data;

namespace TTT.Server.Games
{
    public class UsersManager
    {
        private Dictionary<int, ServerConnection> _connections;
        private readonly IUserRepository _userRepository;
        private NetworkServer _server;

        public UsersManager(IUserRepository userRepository, NetworkServer server)
        {
            _connections = new Dictionary<int, ServerConnection>();
            _userRepository = userRepository;
            _server = server;
        }

        public PlayerNetDto[] GetTopPlayers()
        {
            return _userRepository.GetQuery()
                .OrderByDescending(x => x.Score)
                .Select(u => new PlayerNetDto
                {
                    Username = u.Id,
                    Score = u.Score,
                    IsOnline = u.IsOnline,
                })
                .Take(9)
                .ToArray();
        }

        public void AddConnection(NetPeer peer)
        {
            _connections.Add(peer.Id, new ServerConnection
            {
                ConnectionId = peer.Id,
                Peer = peer
            });
        }

        public bool LoginOrRegister(int connectionId, string username, string password)
        {
            var dbUser = _userRepository.Get(username);

            if (dbUser != null)
            {
                if (dbUser.Password != password)
                {
                    return false;
                }
            }

            if (dbUser == null)
            {
                var newUser = new User
                {
                    Id = username,
                    Password = password,
                    IsOnline = true,
                    Score = 0,
                };

                _userRepository.Add(newUser);
                dbUser = newUser;
            }

            if (_connections.ContainsKey(connectionId))
            {
                dbUser.IsOnline = true;
                _connections[connectionId].User = dbUser;
            }

            return true;
        }

        internal void Disconnect(int peerId)
        {
            var connection = GetConnection(peerId);
            if (connection.User != null)
            {
                var userId = connection.User.Id;
                _userRepository.SetOffline(userId);

                // matchmaker.Unregister

                // gamesManager.CloseGame


                NotififyOtherPlayers(peerId);
            }

            _connections.Remove(peerId);
        }

        public ServerConnection GetConnection(int peerId)
        {
            return _connections[peerId];
        }

        public int[] GetOtherConnectionIds(int excludedConnectionId)
        {
            return _connections.Keys.Where(v => v != excludedConnectionId).ToArray();
        }

        private void NotififyOtherPlayers(int excludedConnectionId)
        {
            var rmsg = new Net_OnServerStatus
            {
                PlayersCount = _userRepository.GetTotalCount(),
                TopPlayers = GetTopPlayers(),
            };

            var otherIds = GetOtherConnectionIds(excludedConnectionId);

            foreach (var connectionId in otherIds)
            {
                _server.SendClient(connectionId, rmsg);
            }
        }
    }
}
