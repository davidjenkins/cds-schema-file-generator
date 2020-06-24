using CdsSchemaFileGenerator.Models;
using Microsoft.Xrm.Sdk.Metadata;
using System;

namespace CdsSchemaFileGenerator.Copy
{
    static class Metadata
    {
        static bool TryGetRecognizedFieldType(AttributeMetadata attribute, out FieldType fieldType)
        {
            FieldType? _fieldType =
                attribute.AttributeType == AttributeTypeCode.Customer ?
                FieldType.EntityReference :
                attribute.AttributeType == AttributeTypeCode.EntityName ?
                FieldType.String :
                attribute.AttributeType == AttributeTypeCode.Memo ?
                FieldType.String :
                IsUnsupported(attribute) ?
                FieldType.Unsupported :
                Enum.IsDefined(typeof(FieldType), (FieldType)attribute.AttributeType) ?
                (FieldType)attribute.AttributeType :
                (FieldType?)null;
            fieldType = _fieldType ?? FieldType.Unsupported;
            return _fieldType.HasValue;
        }

        internal static bool IsUnsupported(AttributeMetadata attribute)
        {
            return !string.IsNullOrEmpty(attribute.AttributeOf);
        }

        internal static bool IsWritable(AttributeMetadata attribute)
        {
            return attribute.IsValidForCreate.Value || attribute.IsValidForUpdate.Value;
        }

        internal static Field ToField(AttributeMetadata attribute)
        {
            var field = new Field();
            ToField(attribute, field);
            return field;
        }

        internal static void ToField(AttributeMetadata attribute, Field field)
        {
            Log.Silently($"Copying metadata for attribute {attribute.LogicalName} (type {attribute.AttributeType}).");
            field.CustomField = attribute.IsCustomAttribute.Value;
            field.DisplayName = attribute.DisplayName.UserLocalizedLabel?.Label ?? attribute.LogicalName;
            field.LookupTypes = attribute is LookupAttributeMetadata metadata ? metadata.Targets : null;
            field.Name = attribute.LogicalName;
            field.PrimaryKey = attribute.IsPrimaryId.Value;
            if (!TryGetRecognizedFieldType(attribute, out var type))
                throw new NotSupportedException($"Type not recognized: {attribute.LogicalName}");
            field.Type = type;
        }

        internal static Relationship ToRelationship(ManyToManyRelationshipMetadata metadata)
        {
            var schema = new Relationship();
            ToRelationship(metadata, schema);
            return schema;
        }

        internal static void ToRelationship(ManyToManyRelationshipMetadata metadata, Relationship schema)
        {
            Log.Silently($"Copying metadata for relationship {metadata.SchemaName}.");
            schema.IsReflexive = metadata.Entity1LogicalName == metadata.Entity2LogicalName;
            schema.Name = metadata.IntersectEntityName; // CMT doesn't capture SchemaName - but probably should.
            schema.RelatedEntity = metadata.IntersectEntityName;
            schema.TargetEntity = metadata.Entity2LogicalName;
            schema.TargetEntityPrimaryKey = metadata.Entity2IntersectAttribute;
        }
    }
}