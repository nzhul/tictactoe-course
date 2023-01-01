using Microsoft.Extensions.Logging;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Models;
using NetworkShared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using System;
using TTT.Server.Games;
using TTT.Server.Utilities;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.MarkCellRequest)]
    public class MarkCellRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly NetworkServer _server;
        private readonly ILogger<MarkCellRequestHandler> _logger;
        private readonly GamesManager _gamesManager;

        public MarkCellRequestHandler(
            UsersManager usersManager,
            GamesManager gamesManager,
            NetworkServer server,
            ILogger<MarkCellRequestHandler> logger)
        {
            _usersManager = usersManager;
            _gamesManager = gamesManager;
            _server = server;
            _logger = logger;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_MarkCellRequest)packet;
            var connection = _usersManager.GetConnection(connectionId);
            var userId = connection.User.Id;
            var game = _gamesManager.FindGame(userId);

            // 1. Validate
            Validate(msg.Index, userId, game);

            var result = game.MarkCell(msg.Index);

            var rmsg = new Net_OnMarkCell
            {
                Actor = userId,
                Index = msg.Index,
                Outcome = result.Outcome,
                WinLineType = result.WinLineType
            };

            var opponentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(opponentId);

            _server.SendClient(connection.ConnectionId, rmsg);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);

            _logger.LogInformation($"`{userId}` marked cell at index `{msg.Index}`!");

            if (result.Outcome == MarkOutcome.None)
            {
                game.SwitchCurrentPlayer();
                return;
            }

            if (result.Outcome == MarkOutcome.Win)
            {
                game.AddWin(userId);
                _usersManager.IncreaseScore(userId);

                _logger.LogInformation($"`{userId}` is a winner! Increasing score and win counter!");
            }
        }

        private void Validate(byte index, string actor, Game game)
        {
            if (game.CurrentUser != actor)
            {
                throw new ArgumentException($"[Bad Request] actor `{actor}` is not the current user!");
            }

            var (row, col) = BasicExtensions.GetRowCol(index);

            if (game.Grid[row, col] != 0)
            {
                throw new ArgumentException($"[Bad Request] cell with index `{index}` at row `{row}` and col `{col}` is already marked!");
            }
        }
    }
}
