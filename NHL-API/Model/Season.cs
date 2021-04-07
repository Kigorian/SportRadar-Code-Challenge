using System;

namespace NHL_API.Model
{
    /// <summary>
    /// Represents a season of NHL hockey.
    /// </summary>
    public class Season
    {
        public int ID { get; set; }

        public string IdString {
            get
            {
                return ID.ToString();
            }
        }

        public int StartYear {
            get
            {
                var startYearString = IdString
                    .Substring(0, IdString.Length / 2);
                
                return int.Parse(startYearString);
            }
        }

        public DateTime RegularSeasonStartDate { get; set; }

        public DateTime RegularSeasonEndDate { get; set; }

        public DateTime SeasonEndDate { get; set; }

        public int NumberOfGames { get; set; }

        public bool AreTiesInUse { get; set; }

        public bool IsOlympicsParticipation { get; set; }

        public bool AreConferencesInUse { get; set; }

        public bool AreDivisionsInUse { get; set; }

        public bool IsWildCardInUse { get; set; }
    }
}
