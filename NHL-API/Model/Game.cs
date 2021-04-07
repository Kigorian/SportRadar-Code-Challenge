using NHL_API.Resources.Enums;
using System;

namespace NHL_API.Model
{
    /// <summary>
    /// Represents a single NHL hockey game.
    /// </summary>
    public class Game
    {
        public int PK { get; set; }

        public GameType GameType { get; set; }

        public int Season { get; set; }

        public DateTimeOffset GameDate { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }
    }
}
