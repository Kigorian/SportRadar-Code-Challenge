using Newtonsoft.Json.Linq;
using NHL_API.Model;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace NHL_API.Services
{
    public class ApiService
    {
        /// <summary>
        /// Fetches Team data from the API, and returns a hydrated <see cref="Team"/>.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static Team GetTeamData(int teamId, int year)
        {
            var baseUrl = ConfigurationManager.AppSettings["NhlApiBaseUrl"];

            // Get the basic Team info.
            var basicInfoJson = GetJsonResponse($"{baseUrl}/teams/{teamId}");
            var basicInfoJObject = (JObject)JObject.Parse(basicInfoJson)
                .SelectToken("teams[0]");

            // Create and fill the Team object.
            //var team = TeamJsonConverter.SerializeToObject(basicInfoJObject);
            var team = new Team();

            team.ID = (int)basicInfoJObject["id"];
            team.Name = (string)basicInfoJObject["name"];

            var venueJObject = (JObject)basicInfoJObject.SelectToken("venue");
            team.VenueName = (string)venueJObject["name"];

            // Get the Team's stats for the given season.
            var season = GetSeasonFromYear(year);

            var teamStatsJson = GetJsonResponse(
                $"{baseUrl}/teams/{teamId}/stats?stats=statsSingleSeason&season={season}"
            );
            var teamStatsJObject = JObject.Parse(teamStatsJson)
                .SelectToken("stats[0].splits[0].stat");

            // Fill in the stats.
            team.GamesPlayed = (int)teamStatsJObject["gamesPlayed"];
            team.Wins = (int)teamStatsJObject["wins"];
            team.Losses = (int)teamStatsJObject["losses"];
            team.Points = (int)teamStatsJObject["pts"];
            team.GoalsPerGame = (int)teamStatsJObject["goalsPerGame"];

            return team;
        }

        /// <summary>
        /// Fetches Player data from the API, and returns a hydrated <see cref="Player"/>.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static Player GetPlayerData(int playerId, int year)
        {
            var baseUrl = ConfigurationManager.AppSettings["NhlApiBaseUrl"];

            // Get the basic Player info.
            var basicInfoJson = GetJsonResponse($"{baseUrl}/people/{playerId}");
            var basicInfoJObject = (JObject)JObject.Parse(basicInfoJson)
                .SelectToken("people[0]");

            // Create and fill the Player object.
            //var player = PlayerJsonConverter.SerializeToObject(basicInfoJObject);
            var player = new Player();

            player.ID = (int)basicInfoJObject["id"];
            player.Name = (string)basicInfoJObject["fullName"];
            player.Age = (int)basicInfoJObject["currentAge"];
            player.Number = (int)basicInfoJObject["primaryNumber"];
            player.IsRookie = (bool)basicInfoJObject["rookie"];

            var currentTeamJObject = (JObject)basicInfoJObject.SelectToken("currentTeam");
            player.CurrentTeamName = (string)currentTeamJObject["name"];

            var primaryPositionJObject = (JObject)basicInfoJObject.SelectToken("primaryPosition");
            player.PositionName = (string)primaryPositionJObject["name"];

            // Get the Player's stats for the given season.
            var season = GetSeasonFromYear(year);

            var playerStatsJson = GetJsonResponse(
                $"{baseUrl}/people/{playerId}/stats?stats=statsSingleSeason&season={season}"
            );
            var playerStatsJObject = JObject.Parse(playerStatsJson)
                .SelectToken("stats[0].splits[0].stat");

            // Fill in the stats.
            player.Assists = (int)playerStatsJObject["assists"];
            player.Goals = (int)playerStatsJObject["goals"];
            player.Games = (int)playerStatsJObject["games"];
            player.Hits = (int)playerStatsJObject["hits"];
            player.Points = (int)playerStatsJObject["points"];

            return player;
        }

        /// <summary>
        /// Calls the API for the given url, and returns a string containing the JSON response.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetJsonResponse(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = responseStream.ReadToEnd();
            webresponse.Close();

            return result;
        }

        #region Helpers

        /// <summary>
        /// Returns the string ID of the season that starts on the given year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static string GetSeasonFromYear(int year)
        {
            var nextYear = year + 1;
            return $"{year}{nextYear}";
        }

        #endregion Helpers
    }
}
