using Newtonsoft.Json.Linq;
using NHL_API.Model;

namespace NHL_API.Resources.JsonConverters
{
    public static class PlayerJsonConverter
    {
        /// <summary>
        /// Serializes the given JSON into a <see cref="Player"/> object.
        /// </summary>
        /// <param name="basicInfoJson"></param>
        /// <param name="playerStatsJson"></param>
        /// <returns></returns>
        public static Player SerializeToObject(string basicInfoJson, string playerStatsJson)
        {
            var player = new Player();

            var basicInfo = (JObject)JObject.Parse(basicInfoJson)
                .SelectToken("people[0]");

            if (basicInfo == null)
            {
                return player;
            }

            // Fill in the basic info.
            player.ID = (int)basicInfo["id"];
            player.Name = (string)basicInfo["fullName"];
            player.Age = (int)basicInfo["currentAge"];
            player.Number = (int)basicInfo["primaryNumber"];
            player.IsRookie = (bool)basicInfo["rookie"];

            // Get the current team name.
            var currentTeam = (JObject)basicInfo.SelectToken("currentTeam");
            player.CurrentTeamName = (string)currentTeam["name"];

            // Get the primary position name.
            var primaryPosition = (JObject)basicInfo.SelectToken("primaryPosition");
            player.PositionName = (string)primaryPosition["name"];

            // Get the stats.
            var playerStats = JObject.Parse(playerStatsJson)
                .SelectToken("stats[0].splits[0].stat");
            if (playerStats == null)
            {
                return player;
            }

            // Fill in the stats.
            player.Assists = (int)playerStats["assists"];
            player.Goals = (int)playerStats["goals"];
            player.Games = (int)playerStats["games"];
            player.Hits = (int)playerStats["hits"];
            player.Points = (int)playerStats["points"];

            return player;
        }
    }
}
