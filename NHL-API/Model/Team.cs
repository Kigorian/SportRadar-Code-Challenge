namespace NHL_API.Model
{
    public class Team
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string VenueName { get; set; }
        //public Venue Venue { get; set; }

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Points { get; set; }

        public int GoalsPerGame { get; set; }
    }
}
