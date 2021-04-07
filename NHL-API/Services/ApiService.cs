using Newtonsoft.Json.Linq;
using NHL_API.Model;
using NHL_API.Resources.JsonConverters;
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
            // Get the season for the given year.
            var season = GetSeasonFromYear(year);

            // Get the JSON from the API.
            var baseUrl = ConfigurationManager.AppSettings["NhlApiBaseUrl"];
            var basicInfoJson = GetJsonResponse($"{baseUrl}/teams/{teamId}");
            var teamStatsJson = GetJsonResponse(
                $"{baseUrl}/teams/{teamId}/stats?stats=statsSingleSeason&season={season}"
            );
            
            // Serialize the JSON to a Team object.
            var team = TeamJsonConverter.SerializeToObject(basicInfoJson, teamStatsJson);

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
            // Get the Player's stats for the given season.
            var season = GetSeasonFromYear(year);

            // Get the JSON from the API.
            var baseUrl = ConfigurationManager.AppSettings["NhlApiBaseUrl"];
            var basicInfoJson = GetJsonResponse($"{baseUrl}/people/{playerId}");
            var playerStatsJson = GetJsonResponse(
                $"{baseUrl}/people/{playerId}/stats?stats=statsSingleSeason&season={season}"
            );

            // Serialize the JSON to a Player object.
            var player = PlayerJsonConverter.SerializeToObject(basicInfoJson, playerStatsJson);

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
