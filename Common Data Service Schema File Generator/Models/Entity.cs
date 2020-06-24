using System.Collections.Generic;
using System.Xml.Serialization;

namespace CdsSchemaFileGenerator.Models
{
    public sealed class Entity
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("displayname")]
        public string DisplayName { get; set; }

        [XmlAttribute("etc")]
        public int EntityTypeCode { get; set; }

        [XmlAttribute("primaryidfield")]
        public string PrimaryIdField { get; set; }

        [XmlAttribute("primarynamefield")]
        public string PrimaryNameField { get; set; }

        [XmlAttribute("disableplugins")]
        public bool DisablePlugins { get; set; }

        [XmlAttribute("skipupdate")]
        public bool SkipUpdate { get; set; }

        [XmlArray("fields")]
        [XmlArrayItem("field")]
        public List<Field> Fields { get; set; }

        [XmlArray("relationships")]
        [XmlArrayItem("relationship")]
        public List<Relationship> Relationships { get; set; }

        [XmlElement("filter")]
        public string Filter { get; set; }

        public bool ShouldSerializeFields() => Fields?.Count > 0;
        public bool ShouldSerializeSkipUpdate() => SkipUpdate;
    }
}