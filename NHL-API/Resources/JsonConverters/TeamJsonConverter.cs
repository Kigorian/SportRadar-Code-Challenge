using Newtonsoft.Json.Linq;
using NHL_API.Model;

namespace NHL_API.Resources.JsonConverters
{
    public static class TeamJsonConverter
    {
        /// <summary>
        /// Serializes the given JSON into a <see cref="Team"/> object.
        /// </summary>
        /// <param name="basicInfoJson"></param>
        /// <param name="teamStatsJson"></param>
        /// <returns></returns>
        public static Team SerializeToObject(string basicInfoJson, string teamStatsJson)
        {
            var team = new Team();

            var basicInfo = (JObject)JObject.Parse(basicInfoJson)
                .SelectToken("teams[0]");

            if (basicInfo == null)
            {
                return team;
            }

            // Fill in the basic info.
            team.ID = (int)basicInfo["id"];
            team.Name = (string)basicInfo["name"];

            // Get the venue name.
            var venue = (JObject)basicInfo.SelectToken("venue");
            team.VenueName = (string)venue["name"];

            // Get the stats.
            var teamStats = JObject.Parse(teamStatsJson)
                .SelectToken("stats[0].splits[0].stat");
            if (teamStats == null)
            {
                return team;
            }

            // Fill in the stats.
            team.GamesPlayed = (int)teamStats["gamesPlayed"];
            team.Wins = (int)teamStats["wins"];
            team.Losses = (int)teamStats["losses"];
            team.Points = (int)teamStats["pts"];
            team.GoalsPerGame = (decimal)teamStats["goalsPerGame"];

            //Game Date of First Game of Season(?)
            //Opponent Name in First Game of Season(?)

            return team;
        }
    }
}
