using System;

namespace NHL_API.Resources.Attributes
{
    /// <summary>
    /// Specifies the description for an enum.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
    {
        public DescriptionAttribute() { }
        public DescriptionAttribute(string description)
            : base(description) { }

        public new string Description
        {
            get { return DescriptionValue; }
            set { DescriptionValue = value; }
        }
    }
}
