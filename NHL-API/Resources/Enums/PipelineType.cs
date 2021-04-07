using NHL_API.Resources.Attributes;

namespace NHL_API.Resources.Enums
{
    public enum PipelineType : short
    {
        [Description("teams")]
        Teams,

        [Description("people")]
        Players,
    }
}
