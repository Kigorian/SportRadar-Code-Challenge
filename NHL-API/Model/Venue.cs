using System;

namespace NHL_API.Model
{
    public class Venue
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public string City { get; set; }

        public DateTimeOffset Timezone { get; set; }
    }
}
