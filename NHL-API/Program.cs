using Newtonsoft.Json.Linq;
using NHL_API.Model;
using NHL_API.Resources.Attributes;
using NHL_API.Resources.Enums;
//using NHL_API.Resources.JsonConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace NHL_API
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the user's pipeline type choice.
            var pipelineType = GetPipelineTypeFromConsole();
            var pipelineTypeDescription = AttributeHelper.GetDescription(pipelineType);

            // Get the ID and year from the user.
            var entityId = GetEntityIdFromConsole(pipelineTypeDescription);
            var year = GetYearFromConsole();

            string csv;
            switch (pipelineType)
            {
                case PipelineType.Team:
                    var team = GetTeamData(entityId, year);
                    csv = ToCsv(new List<Team>() { team });
                    break;

                case PipelineType.Player:
                    var player = GetPlayerData(entityId, year);
                    csv = ToCsv(new List<Player>() { player });
                    break;

                default:
                    return;
            }

            // Get the file path info from the user.
            var directoryPath = GetFilePathFromConsole();
            var fileName = GetFileNameFromConsole(directoryPath);
            var filePath = $"{directoryPath}/{fileName}";
            
            // Save the CSV file.
            File.WriteAllText(filePath, csv);
        }

        /// <summary>
        /// Asks the user for the pipeline type using the console.
        /// </summary>
        /// <returns></returns>
        private static PipelineType GetPipelineTypeFromConsole()
        {
            while (true)
            {
                Console.WriteLine("Would you like to query for Team or Player results?");
                var pipelineTypeChoice = Console.ReadLine();

                switch (pipelineTypeChoice.ToLower().Trim().TrimEnd('s'))
                {
                    case "team":
                        return PipelineType.Team;

                    case "player":
                        return PipelineType.Player;

                    default:
                        Console.WriteLine();
                        WriteLineInColor(
                            $"\"{pipelineTypeChoice}\" is not a valid option.",
                            ConsoleColor.Red
                        );
                        break;
                }
            }
        }

        private static int GetEntityIdFromConsole(string pipelineTypeDescription)
        {
            int id;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Please enter the ID for the {pipelineTypeDescription} " +
                    $"you would like to view."
                );
                var userInput = Console.ReadLine();

                if (int.TryParse(userInput, out id) && id > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    WriteLineInColor(
                        $"\"{userInput}\" is not a valid option. " +
                        $"Please enter the ID as a positive integer.",
                        ConsoleColor.Red
                    );
                }
            }

            return id;
        }

        private static int GetYearFromConsole()
        {
            // Set some boundaries.
            var minYear = 1900;

            //var currentYear = DateTime.Now.Year;
            var currentSeasonUrl = "https://statsapi.web.nhl.com/api/v1/seasons/current";
            var currentSeasonJson = GetJsonResponse(currentSeasonUrl);
            var currentSeasonJObject = (JObject)JObject.Parse(currentSeasonJson)
                .SelectToken("seasons[0]");

            var currentSeasonIdString = (string)currentSeasonJObject["seasonId"];
            var currentSeasonStartYearString = currentSeasonIdString
                .Substring(0, currentSeasonIdString.Length / 2);
            var currentSeasonStartYear = int.Parse(currentSeasonStartYearString);

            int year;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Which year would you like to view?");
                var userInput = Console.ReadLine();

                if (int.TryParse(userInput, out year) && year > minYear && year <= currentSeasonStartYear)
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    WriteLineInColor(
                        $"\"{userInput}\" is not a valid option. " +
                        $"Please enter a valid year between {minYear} and {currentSeasonStartYear}",
                        ConsoleColor.Red
                    );
                }
            }

            return year;
        }

        private static Team GetTeamData(int teamId, int year)
        {
            var baseUrl = "https://statsapi.web.nhl.com/api/v1";
            //var base = ConfigurationManager.AppSettings["NHL-API"];

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

            // Get the Team's Stats.
            var teamStatsJson = GetJsonResponse($"{baseUrl}/teams/{teamId}/stats");
            var teamStatsJObject = JObject.Parse(teamStatsJson)
                .SelectToken("stats[0].splits[0].stat");

            team.GamesPlayed = (int)teamStatsJObject["gamesPlayed"];
            team.Wins = (int)teamStatsJObject["wins"];
            team.Losses = (int)teamStatsJObject["losses"];
            team.Points = (int)teamStatsJObject["pts"];
            team.GoalsPerGame = (int)teamStatsJObject["goalsPerGame"];

            return team;
        }

        private static Player GetPlayerData(int playerId, int year)
        {
            var baseUrl = "https://statsapi.web.nhl.com/api/v1";
            //var base = ConfigurationManager.AppSettings["NHL-API"];

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

            // Get the Player's Stats for the given season.
            var nextYear = year + 1;
            var season = $"{year}{nextYear}";

            var playerStatsJson = GetJsonResponse(
                $"{baseUrl}/people/{playerId}/stats?stats=statsSingleSeason&season={season}"
            );
            var playerStatsJObject = JObject.Parse(playerStatsJson)
                .SelectToken("stats[0].splits[0].stat");

            player.Assists = (int)playerStatsJObject["assists"];
            player.Goals = (int)playerStatsJObject["goals"];
            player.Games = (int)playerStatsJObject["games"];
            player.Hits = (int)playerStatsJObject["hits"];
            player.Points = (int)playerStatsJObject["points"];

            return player;
        }

        private static string GetFilePathFromConsole()
        {
            string path;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please specify the directory to save the file to.");
                path = Console.ReadLine();

                if (Directory.Exists(path))
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    WriteLineInColor(
                        $"No directory found at \"{path}\". " +
                        $"Please enter the path to an existing directory.",
                        ConsoleColor.Red
                    );
                }
            }

            return path;
        }
        
        private static string GetFileNameFromConsole(string directoryPath)
        {
            string name;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose a name for your file (without the extension).");
                name = Console.ReadLine();

                if (File.Exists($"{directoryPath}/{name}.csv"))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "A file with that name already exists. " +
                        "Do you wish to overwrite it? (Y/N)"
                    );
                    var isOverwrite = Console.ReadLine();

                    if (isOverwrite.ToLower() == "y" || isOverwrite.ToLower() == "yes")
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
            }

            return $"{name}.csv";
        }

        /// <summary>
        /// Calls Console.WriteLine(), but prints in the specified color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="message"></param>
        private static void WriteLineInColor(string value, ConsoleColor color)
        {
            // Keep track of original color so we can set it again.
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = originalColor;
        }

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

        public static string ToCsv<T>(IEnumerable<T> objectlist)
        {
            Type t = typeof(T);
            var properties = t.GetProperties();

            string header = string.Join(',', properties.Select(f => f.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
            {
                csvdata.AppendLine(ToCsvFields(properties, o));
            }

            return csvdata.ToString();
        }

        private static string ToCsvFields(PropertyInfo[] properties, object o)
        {
            StringBuilder csv = new StringBuilder();

            foreach (var property in properties)
            {
                if (csv.Length > 0)
                {
                    csv.Append(',');
                }

                var x = property.GetValue(o);

                if (x != null)
                {
                    csv.Append(x.ToString());
                }
            }

            return csv.ToString();
        }
    }
}
