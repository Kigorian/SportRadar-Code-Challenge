using Newtonsoft.Json.Linq;
using NHL_API.Model;
using System;

namespace NHL_API.Resources.JsonConverters
{
    public static class SeasonJsonConverter
    {
        /// <summary>
        /// Serializes the given JSON into a <see cref="Team"/> object.
        /// </summary>
        /// <param name="basicInfoJson"></param>
        /// <param name="teamStatsJson"></param>
        /// <returns></returns>
        public static Season SerializeToObject(string seasonInfoJson)
        {
            var season = new Season();

            var info = (JObject)JObject.Parse(seasonInfoJson)
                .SelectToken("seasons[0]");

            if (info == null)
            {
                return season;
            }

            // Fill in the info.
            season.ID = (int)info["seasonId"];
            season.RegularSeasonStartDate = (DateTime)info["regularSeasonStartDate"];
            season.RegularSeasonEndDate = (DateTime)info["regularSeasonEndDate"];
            season.SeasonEndDate = (DateTime)info["seasonEndDate"];
            season.NumberOfGames = (int)info["numberOfGames"];
            season.AreTiesInUse = (bool)info["tiesInUse"];
            season.IsOlympicsParticipation = (bool)info["olympicsParticipation"];
            season.AreConferencesInUse = (bool)info["conferencesInUse"];
            season.AreDivisionsInUse = (bool)info["divisionsInUse"];
            season.IsWildCardInUse = (bool)info["wildCardInUse"];

            return season;
        }
    }
}
