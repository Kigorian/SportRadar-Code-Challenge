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
    }
}
