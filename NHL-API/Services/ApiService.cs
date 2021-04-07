using NHL_API.Model;
using NHL_API.Model.Web;
using NHL_API.Resources.JsonConverters;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NHL_API.Services
{
    public class ApiService
    {
        #region Team

        /// <summary>
        /// Attempts to get the Team data for the given ID and year.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="year"></param>
        /// <returns>
        /// A <see cref="DataResult"/> containing either the Team or an exception, depending
        /// on the success of the API request.
        /// </returns>
        public static DataResult TryGetTeamData(int teamId, int year)
        {
            try
            {
                var team = GetTeamData(teamId, year);
                return new DataResult() {
                    EntityResult = team,
                };
            }
            catch (Exception e)
            {
                return new DataResult()
                {
                    Exception = e,
                };
            }
        }

        /// <summary>
        /// Fetches Team data from the API, and returns a hydrated <see cref="Team"/>.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private static Team GetTeamData(int teamId, int year)
        {
            // Get the season for the given year.
            var season = GetSeasonFromYear(year);

            // Get the JSON from the API.
            var baseUrl = ConfigurationManager.AppSettings["NhlApiBaseUrl"];
            var basicInfoJson = GetJsonResponse($"{baseUrl}/teams/{teamId}");
            var teamStatsJson = GetJsonResponse(
                $"{baseUrl}/teams/{teamId}/stats?stats=statsSingleSeason&season={season}"
            );
            var scheduleJson = GetJsonResponse($"{baseUrl}/schedule?season={season}");
            
            // Serialize the JSON to a Team object.
            var team = TeamJsonConverter.SerializeToObject(basicInfoJson, teamStatsJson);

            // Get the first game from this season.
            var firstGame = GetTeamFirstGameFromScheduleJson(team.ID, scheduleJson);

            // Set the info from the first game.
            if (firstGame != null) {
                team.SeasonFirstGameDate = firstGame.GameDate;

                var opponentName = firstGame.HomeTeam.ID == team.ID
                    ? firstGame.AwayTeam.Name
                    : firstGame.HomeTeam.Name;
                team.SeasonFirstGameOpponent = opponentName;
            }

            return team;
        }

        #endregion Team

        #region Player

        /// <summary>
        /// Attempts to get the Player data for the given ID and year.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="year"></param>
        /// <returns>
        /// A <see cref="DataResult"/> containing either the Player or an exception, depending
        /// on the success of the API request.
        /// </returns>
        public static DataResult TryGetPlayerData(int playerId, int year)
        {
            try
            {
                var player = GetPlayerData(playerId, year);
                return new DataResult()
                {
                    EntityResult = player,
                };
            }
            catch (Exception e)
            {
                return new DataResult()
                {
                    Exception = e,
                };
            }
        }

        /// <summary>
        /// Fetches Player data from the API, and returns a hydrated <see cref="Player"/>.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private static Player GetPlayerData(int playerId, int year)
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

        #endregion Player

        #region Season

        /// <summary>
        /// Fetches data from the API for the season that starts on the given year,
        /// and returns a hydrated <see cref="Season"/>.
        /// </summary>
        /// <remarks>
        /// If no year is provided, gets the data for the current season.
        /// </remarks>
        /// <returns></returns>
        public static Season GetSeasonData(int? startYear = null)
        {
            // Get the season for the given year.
            var seasonIdString = startYear.HasValue
                ? GetSeasonFromYear(startYear.Value).ToString()
                : "current";

            var baseUrl = ConfigurationManager.AppSettings["NhlApiBaseUrl"];
            var seasonJson = GetJsonResponse($"{baseUrl}/seasons/{seasonIdString}");

            // Serialize the JSON to a Player object.
            var season = SeasonJsonConverter.SerializeToObject(seasonJson);

            return season;
        }

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

        /// <summary>
        /// Gets the given team's first game in the given schedule.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="scheduleJson"></param>
        /// <returns></returns>
        private static Game GetTeamFirstGameFromScheduleJson(int teamId, string scheduleJson)
        {
            var schedule = ScheduleJsonConverter.SerializeToObject(scheduleJson);

            return schedule.Games
                .OrderBy(g => g.GameDate)
                .FirstOrDefault(
                    g =>
                    g.HomeTeam.ID == teamId
                    || g.AwayTeam.ID == teamId
                );
        }

        #endregion Season

        /// <summary>
        /// Calls the API for the given url, and returns a string containing the JSON response.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetJsonResponse(string url)
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
    }
}
