using Newtonsoft.Json.Linq;
using NHL_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHL_API.Resources.JsonConverters
{
    public static class ScheduleJsonConverter
    {
        /// <summary>
        /// Serializes the given JSON into a <see cref="Team"/> object.
        /// </summary>
        /// <param name="basicInfoJson"></param>
        /// <param name="teamStatsJson"></param>
        /// <returns></returns>
        public static Schedule SerializeToObject(string scheduleJson)
        {
            var schedule = new Schedule();

            var scheduleJObject = JObject.Parse(scheduleJson);
            var gameDatesJArray = (JArray)scheduleJObject.SelectToken("dates");

            if (gameDatesJArray == null)
            {
                return schedule;
            }

            var games = new List<Game>();
            foreach (var gameDate in gameDatesJArray)
            {
                var gamesJArray = (JArray)gameDate.SelectToken("games");

                foreach (var gameJObject in gamesJArray) {
                    var gameJson = gameJObject.ToString();

                    var game = GameJsonConverter.SerializeToObject(gameJson);
                    games.Add(game);
                }
            }
            schedule.Games = games;

            return schedule;
        }
    }
}
