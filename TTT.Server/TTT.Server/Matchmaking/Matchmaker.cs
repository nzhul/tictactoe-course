using Microsoft.Extensions.Logging;
using NetworkShared.Packets.ServerClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Server.Games;

namespace TTT.Server.Matchmaking
{
    public class Matchmaker
    {
        private readonly ILogger<Matchmaker> _logger;
        private readonly GamesManager _gamesManager;
        private readonly NetworkServer _server;
        private List<MMRequest> _pool = new List<MMRequest>();

        public Matchmaker(
            ILogger<Matchmaker> logger,
            GamesManager gamesManager,
            NetworkServer server)
        {
            _logger = logger;
            _gamesManager = gamesManager;
            _server = server;
        }

        public void RegisterPlayer(ServerConnection connection)
        {
            if (_pool.Any(x => x.Connection.User.Id == connection.User.Id))
            {
                _logger.LogWarning($"{connection.User.Id} is already registered! Ignoring ...");
                return;
            }

            var request = new MMRequest
            {
                Connection = connection,
                SearchStart = DateTime.UtcNow
            };

            _pool.Add(request);

            _logger.LogInformation($"{request.Connection.User.Id} registered in matchmaking pool.");

            DoMatchmaking();
        }

        public void TryUnregisterPlayer(string username)
        {
            var request = _pool.FirstOrDefault(r => r.Connection.User.Id == username);
            if (request != null)
            {
                _logger.LogInformation($"Removing {request.Connection.User.Id} from the matchmaking pool.");
                _pool.Remove(request);
            }
        }

        private void DoMatchmaking()
        {
            var matchedRequests = new List<MMRequest>();

            foreach (var request in _pool)
            {
                var match = _pool.FirstOrDefault(
                    x => !x.MatchFound &&
                    x.Connection.ConnectionId != request.Connection.ConnectionId);

                if (match == null)
                {
                    continue;
                }

                request.MatchFound = true;
                match.MatchFound = true;
                matchedRequests.Add(request);
                matchedRequests.Add(match);

                var xUser = request.Connection.User.Id;
                var oUser = match.Connection.User.Id;
                var gameId = _gamesManager.RegisterGame(xUser, oUser);
                request.Connection.GameId = gameId;
                match.Connection.GameId = gameId;

                var msg = new Net_OnStartGame
                {
                    GameId = gameId,
                    XUser = request.Connection.User.Id,
                    OUser = match.Connection.User.Id
                };

                var p1 = request.Connection.ConnectionId;
                var p2 = match.Connection.ConnectionId;

                _server.SendClient(p1, msg);
                _server.SendClient(p2, msg);

                _logger.LogInformation($"Match found for players: {request.Connection.User.Id}(X) and {match.Connection.User.Id}(O)");
            }

            foreach (var request in matchedRequests)
            {
                _pool.Remove(request);
            }
        }
    }
}
