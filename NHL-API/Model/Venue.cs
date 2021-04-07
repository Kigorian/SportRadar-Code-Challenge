using System;

namespace NHL_API.Model
{
    /// <summary>
    /// Represents a venue that an NHL hockey team would play at.
    /// </summary>
    public class Venue
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public string City { get; set; }

        public DateTimeOffset Timezone { get; set; }
    }
}
