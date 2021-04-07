namespace NHL_API.Model
{
    public class Player
    {
		public int ID { get; set; }

		public string Name { get; set; }

		public string CurrentTeamName { get; set; }

		public int Age { get; set; }

		public int Number { get; set; }

		public string PositionName { get; set; }

		public bool IsRookie { get; set; }

		public int Assists { get; set; }

		public int Goals { get; set; }

		public int Games { get; set; }

		public int Hits { get; set; }

		public int Points { get; set; }
	}
}
