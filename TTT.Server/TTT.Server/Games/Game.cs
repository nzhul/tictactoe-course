using NetworkShared.Models;
using System;
using TTT.Server.Utilities;

namespace TTT.Server.Games
{
    public class Game
    {
        private const int GRID_SIZE = 3;

        public Game(string xUser, string oUser)
        {
            Id = Guid.NewGuid();
            StartTime = DateTime.UtcNow;
            CurrentRoundStartTime = DateTime.UtcNow;
            XUser = xUser;
            OUser = oUser;
            Round = 1;
            Grid = new MarkType[GRID_SIZE, GRID_SIZE];
            CurrentUser = xUser;
        }

        public Guid Id { get; set; }

        public ushort Round { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime CurrentRoundStartTime { get; set; }

        public string OUser { get; set; }

        public ushort OWins { get; set; }

        public bool OWantsRematch { get; set; }

        public string XUser { get; set; }

        public ushort XWins { get; set; }

        public bool XWantRematch { get; set; }

        public string CurrentUser { get; set; }

        public MarkType[,] Grid { get; }

        public MarkResult MarkCell(byte index)
        {
            var (row, col) = BasicExtensions.GetRowCol(index);
            Grid[row, col] = GetPlayerType(CurrentUser);

            var (isWin, lineType) = CheckWin(row, col);
            var draw = CheckDraw();

            var result = new MarkResult();

            if (isWin)
            {
                result.Outcome = MarkOutcome.Win;
                result.WinLineType = lineType;
            }
            else if (draw)
            {
                result.Outcome = MarkOutcome.Draw;
            }

            return result;
        }

        public string GetOpponent(string otherUserId)
        {
            if (otherUserId == XUser)
            {
                return OUser;
            }
            else
            {
                return XUser;
            }
        }

        public void SwitchCurrentPlayer()
        {
            CurrentUser = GetOpponent(CurrentUser);
        }

        internal void AddWin(string winnerId)
        {
            var winnerType = GetPlayerType(winnerId);
            if (winnerType == MarkType.X)
            {
                XWins++;
            }
            else
            {
                OWins++;
            }
        }

        private bool CheckDraw()
        {
            for (int row = 0; row < GRID_SIZE; row++)
            {
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    if (Grid[row, col] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private (bool isWin, WinLineType lineType) CheckWin(byte row, byte col)
        {
            var type = Grid[row, col];

            // check col
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (Grid[row, i] != type) break;
                if (i == GRID_SIZE - 1) return (true, ResolveLineTypeRow(row));
            }

            // check row
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (Grid[i, col] != type) break;
                if (i == GRID_SIZE - 1) return (true, ResolveLineTypeCol(col));
            }

            // check diagonal
            if (row == col)
            {
                // we are on a diagonal
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (Grid[i, i] != type) break;
                    if (i == GRID_SIZE - 1) return (true, WinLineType.Diagonal);
                }
            }

            // check anti-diagonal
            if (row + col == GRID_SIZE - 1)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (Grid[i, (GRID_SIZE - 1) - i] != type) break;
                    if (i == GRID_SIZE - 1) return (true, WinLineType.AntiDiagonal);
                }
            }

            return (false, WinLineType.None);
        }

        private WinLineType ResolveLineTypeCol(byte col)
        {
            return (WinLineType)(col + 3);
        }

        private WinLineType ResolveLineTypeRow(byte row)
        {
            return (WinLineType)(row + 6);
        }

        private MarkType GetPlayerType(string userId)
        {
            if (userId == XUser)
            {
                return MarkType.X;
            }
            else
            {
                return MarkType.O;
            }
        }
    }

    public struct MarkResult
    {
        public MarkOutcome Outcome { get; set; }

        public WinLineType WinLineType { get; set; }
    }
}
