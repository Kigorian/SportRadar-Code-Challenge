using Newtonsoft.Json.Linq;
using NHL_API.Model;
using NHL_API.Resources.Attributes;
using NHL_API.Resources.Enums;
using System;

namespace NHL_API.Resources.JsonConverters
{
    public static class GameJsonConverter
    {
        /// <summary>
        /// Serializes the given JSON into a <see cref="Team"/> object.
        /// </summary>
        /// <param name="basicInfoJson"></param>
        /// <param name="teamStatsJson"></param>
        /// <returns></returns>
        public static Game SerializeToObject(string gameJson)
        {
            var game = new Game();

            var gameJObject = JObject.Parse(gameJson);

            if (gameJObject == null)
            {
                return game;
            }

            // Fill in the basic info.
            game.PK = (int)gameJObject["gamePk"];
            game.GameType = AttributeHelper.GetValueFromDescription<GameType>(
                (string)gameJObject["gameType"]
            );
            game.Season = (int)gameJObject["season"];
            game.GameDate = (DateTimeOffset)gameJObject["gameDate"];

            // Get the teams info.
            var teamsJObject = (JObject)gameJObject.SelectToken("teams");
            var homeTeamJObject = teamsJObject
                .SelectToken("home")
                .SelectToken("team");
            var awayTeamJObject = teamsJObject
                .SelectToken("away")
                .SelectToken("team");

            // Fill in the team values we need.
            game.HomeTeam = new Team()
            {
                ID = (int)homeTeamJObject["id"],
                Name = (string)homeTeamJObject["name"],
            };
            game.AwayTeam = new Team()
            {
                ID = (int)awayTeamJObject["id"],
                Name = (string)awayTeamJObject["name"],
            };

            return game;
        }
    }
}
