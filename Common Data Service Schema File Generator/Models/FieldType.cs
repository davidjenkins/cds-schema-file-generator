using System.Xml.Serialization;

namespace CdsSchemaFileGenerator.Models
{
    /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode"/>
    public enum FieldType
    {
        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.BigInt"/>
        [XmlEnum("bigint")]
        BigInteger = 18,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Boolean"/>
        [XmlEnum("bool")]
        Boolean = 0,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.DateTime"/>
        [XmlEnum("datetime")]
        DateTime = 2,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Decimal"/>
        [XmlEnum("decimal")]
        Decimal = 3,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Lookup"/>
        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Customer"/>
        [XmlEnum("entityreference")]
        EntityReference = 6,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Double"/>
        [XmlEnum("float")]
        Float = 4,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Uniqueidentifier"/>
        [XmlEnum("guid")]
        Guid = 15,

        [XmlEnum("imagedata")]
        ImageData, // NEED TO SPECIFY A VALUE HERE

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Money"/>
        [XmlEnum("money")]
        Money = 8,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Integer"/>
        [XmlEnum("number")]
        Integer = 5,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Picklist"/>
        [XmlEnum("optionsetvalue")]
        OptionSetValue = 11,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Owner"/>
        [XmlEnum("owner")]
        Owner = 9,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.State"/>
        [XmlEnum("state")]
        State = 12,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Status"/>
        [XmlEnum("status")]
        Status = 13,

        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Memo"/>
        /// <see cref="Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.String"/>
        [XmlEnum("string")]
        String = 14,

        [XmlIgnore]
        Unsupported = -1
    }
}