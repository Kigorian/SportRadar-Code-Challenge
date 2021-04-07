using Newtonsoft.Json.Linq;
using NHL_API.Model;

namespace NHL_API.Resources.JsonConverters
{
    public static class VenueJsonConverter
    {
        public static Venue SerializeToObject(JObject jObject)
        {
            if (jObject == null)
            {
                return new Venue();
            }

            return new Venue()
            {
                Name = (string)jObject["name"],
            };
        }
    }
}
