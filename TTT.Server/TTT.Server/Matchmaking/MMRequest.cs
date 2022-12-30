using System;
using TTT.Server.Games;

namespace TTT.Server.Matchmaking
{
    public class MMRequest
    {
        public ServerConnection Connection { get; set; }

        public DateTime SearchStart { get; set; }

        public bool MatchFound { get; set; }
    }
}
