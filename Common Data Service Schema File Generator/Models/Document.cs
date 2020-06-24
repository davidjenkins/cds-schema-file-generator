using System.Collections.Generic;
using System.Xml.Serialization;

namespace CdsSchemaFileGenerator.Models
{
    [XmlRoot(ElementName = "entities")]
    public sealed class Document
    {
        [XmlElement("entity")]
        public List<Entity> Entities { get; set; }
    }
}