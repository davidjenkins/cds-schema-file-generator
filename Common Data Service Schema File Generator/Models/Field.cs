using System;
using System.Xml.Serialization;

namespace CdsSchemaFileGenerator.Models
{
    public sealed class Field
    {
        [XmlAttribute("updateCompare")]
        public bool UpdateCompare { get; set; }

        [XmlAttribute("displayname")]
        public string DisplayName { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public FieldType Type { get; set; }

        [XmlAttribute("primaryKey")]
        public bool PrimaryKey { get; set; }

        [XmlAttribute("lookupType")]
        public string LookupType { get; set; }

        [XmlIgnore]
        public string[] LookupTypes
        {
            get
            {
                return string.IsNullOrEmpty(LookupType) ?
                    new string[] { } :
                    LookupType.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
            set
            {
                LookupType = value == null ? null : string.Join("|", value);
            }
        }

        [XmlAttribute("dateMode")]
        public DateMode DateMode { get; set; }

        [XmlAttribute("customfield")]
        public bool CustomField { get; set; }

        /// <summary>
        /// Field is missing from connected environment.
        /// </summary>
        [XmlIgnore]
        bool? Missing { get; set; }

        /// <summary>
        /// Template.
        /// </summary>
        internal static readonly Field EntityImage = new Field
        {
            DisplayName = "Entity Image",
            Name = "entityimage",
            Type = FieldType.ImageData
        };

        /// <summary>
        /// Template.
        /// </summary>
        internal static readonly Field EntityImageId = new Field
        {
            DisplayName = "Entity Image Id",
            Name = "entityimageid",
            Type = FieldType.Guid
        };

        public bool ShouldSerializeCustomField() => CustomField;
        public bool ShouldSerializeDateMode() => DateMode != default;
        public bool ShouldSerializeLookupType() => Type == FieldType.EntityReference;
        public bool ShouldSerializePrimaryKey() => PrimaryKey && Type == FieldType.Guid;
        public bool ShouldSerializeUpdateCompare() => UpdateCompare;
    }
}