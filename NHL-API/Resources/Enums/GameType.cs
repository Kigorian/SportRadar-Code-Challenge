using NHL_API.Resources.Attributes;

namespace NHL_API.Resources.Enums
{
    public enum GameType : short
    {
        [Description("PR")]
        PreSeason,

        [Description("R")]
        RegularSeason,
            
        [Description("P")]
        Playoffs,

        [Description("A")]
        AllStarGame,

        [Description("WA")]
        AllStarWomensGame,

        [Description("O")]
        OlympicGame,

        [Description("E")]
        Exhibition,
        
        [Description("WCOH_EXH")]
        WorldCupOfHockey_ExhibitionOrPreseasonGame,
        
        [Description("WCOH_PRELIM")]
        WorldCupOfHockey_PreliminaryGame,
        
        [Description("WCOH_FINAL")]
        WorldCupOfHockey_SemiFinalsAndFinals,
    }
}
