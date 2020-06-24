using System;
using System.Xml.Serialization;

namespace CdsSchemaFileGenerator.Models
{
    public sealed class Relationship
    {
        /// <summary>
        /// Name of relationship.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("manyToMany")]
        public bool ManyToMany
        {
            get => true;
            set { if (!value) throw new NotSupportedException("Value must be True."); }
        }

        [XmlAttribute("isreflexive")]
        public bool IsReflexive { get; set; }

        /// <summary>
        /// Name of intersect (many-to-many association) entity type.
        /// </summary>
        [XmlAttribute("relatedEntityName")]
        public string RelatedEntity { get; set; }

        /// <summary>
        /// Other entity type in relationship.
        /// </summary>
        [XmlAttribute("m2mTargetEntity")]
        public string TargetEntity { get; set; }

        /// <summary>
        /// Primary key for other entity type in relationship.
        /// </summary>
        [XmlAttribute("m2mTargetEntityPrimaryKey")]
        public string TargetEntityPrimaryKey { get; set; }

        public bool ShouldSerializeIsReflexive() => IsReflexive;
    }
}