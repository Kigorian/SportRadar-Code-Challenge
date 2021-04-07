using Newtonsoft.Json.Linq;
using NHL_API.Model;

namespace NHL_API.Resources.JsonConverters
{
    public static class TeamJsonConverter
    {
        public static Team SerializeToObject(JObject jObject)
        {
            if (jObject == null)
            {
                return new Team();
            }

            return new Team()
            {
                ID = (int)jObject["id"],
                Name = (string)jObject["name"],
                //Venue = VenueJsonConverter.SerializeToObject((JObject)jObject["venue"]),
                GamesPlayed = (int)jObject[""],
                //Wins
                //Losses
                //Points
                //GoalsPergame
            };
        }
    }
}
