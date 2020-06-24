using System.Xml.Serialization;

namespace CdsSchemaFileGenerator.Models
{
    public enum DateMode
    {
        Default = 0,

        [XmlEnum("absolute")]
        Absolute = 1,

        [XmlEnum("relative")]
        Relative = 2
    }
}