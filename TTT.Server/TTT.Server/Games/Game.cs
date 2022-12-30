using System;

namespace TTT.Server.Games
{
    public class Game
    {
        public Game(string xUser, string oUser)
        {
            Id = Guid.NewGuid();
            StartTime = DateTime.UtcNow;
            CurrentRoundStartTime = DateTime.UtcNow;
            XUser = xUser;
            OUser = oUser;
            Round = 1;
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
    }
}
