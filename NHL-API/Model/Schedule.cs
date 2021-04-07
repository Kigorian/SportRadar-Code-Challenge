using System.Collections.Generic;

namespace NHL_API.Model
{
    /// <summary>
    /// Represents the schedule of NHL hockey games.
    /// </summary>
    public class Schedule
    {
        public Schedule()
        {
            Games = new List<Game>();
        }

        public ICollection<Game> Games { get; set; }
    }
}
