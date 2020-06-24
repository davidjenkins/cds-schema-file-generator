using CdsSchemaFileGenerator.Extensions;
using CdsSchemaFileGenerator.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CdsSchemaFileGenerator
{
    public sealed class CdsSchemaFileGenerator
    {
        static class Fields
        {
            /// <summary>
            /// Fields that are always read-only (cannot control value) or are best to exclude.
            /// </summary>
            /// <remarks>Static list for now, but want to detect these intelligently in the future.</remarks>
            /// <see cref="RemoveAllInstanceFields"/>
            /// <seealso cref="Copy.Metadata.IsWritable(AttributeMetadata)"/>
            readonly internal static string[] AlwaysExclude = new[]
            {
                "createdby",
                "createdbyexternalparty",
                "createdonbehalfby",
                "importsequencenumber",
                "overriddencreatedon",
                "modifiedby",
                "modifiedbyexternalparty",
                "modifiedon",
                "modifiedonbehalfby",
                "organizationid",
                "owningbusinessunit",
                "owningteam",
                "owninguser",
                "versionnumber"
            };

            readonly internal static string[] Business = new[]
            {
                "exchangerate",
                "transactioncurrencyid",
            };

            readonly internal static string[] Creation = new[]
            {
                "createdon"
            };

            readonly internal static string[] Owner = new[]
            {
                "ownerid"
            };

            readonly internal static string[] Portal = new[]
            {
                "adx_createdbyipaddress",
                "adx_createdbyusername",
                "adx_modifiedbyipaddress",
                "adx_modifiedbyusername",
                "adx_primarydomainname"
            };

            readonly internal static string[] TimeZone = new[]
            {
                "timezoneruleversionnumber",
                "utcconversiontimezonecode"
            };
        }

        public CdsSchemaFileGenerator(IOrganizationService cds)
        {
            Cds = cds;
        }

        /// <summary>
        /// Add fields not included in schema.
        /// </summary>
        /// <param name="entity"></param>
        void AddMissingFields(Models.Entity entity)
        {
        }

        /// <summary>
        /// Add relationships not included in schema.
        /// </summary>
        /// <param name="entity"></param>
        void AddMissingRelationships(Models.Entity entity)
        {
        }

        public void AddEntity(string name, bool includeAllFields = true, bool includeAllRelationships = true)
        {
            throw new NotImplementedException();
        }

        public void AddField(string name, string entityName)
        {
            throw new NotImplementedException();
        }

        public void AddRelationship(string name)
        {
            throw new NotImplementedException();
        }

        public void ExportSchemaFile(string schemaFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Document));
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (var stream = new FileStream(schemaFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, CurrentDocument, namespaces);
                writer.Close();
            }
        }

        public void ImportSchemaFile(string schemaFilePath)
        {
            var serializer = new XmlSerializer(typeof(Document));
            using (Stream stream = new FileStream(schemaFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                CurrentDocument = (Document)serializer.Deserialize(stream);
            }
        }

        public void RemoveAllEntitiesFilter()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.Filter = null;
            }
        }

        /// <summary>
        /// Remove fields whose values do not translated naturally between instances or is not appropriate for copying between instances.
        /// </summary>
        public void RemoveAllInstanceFields()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.Fields = entity.Fields
                    .Where(field => !Fields.AlwaysExclude.Contains(field.Name))
                    .ToList();
            }
        }

        /// <summary>
        /// Evaluated conditionally. Should be last step in field selection process.
        /// </summary>
        public void RemoveAllIrrelevantFields()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                if (!entity.Fields.Any(field => field.Type == FieldType.DateTime))
                {
                    entity.Fields = entity.Fields
                        .Where(field => !Fields.TimeZone.Contains(field.Name))
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Remove fields whose values are typically considered specific to a single business organization and would not be appropriate for providing to outside organizations.
        /// </summary>
        public void RemoveAllTenantFields()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.Fields = entity.Fields
                    .Where(field =>
                        !Fields.Business.Contains(field.Name) &&
                        !Fields.Creation.Contains(field.Name) &&
                        !Fields.Owner.Contains(field.Name)
                    )
                    .ToList();
            }
        }

        public void RemoveAllInstancePortalFields()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.Fields = entity.Fields.Where(field => !Fields.Portal.Contains(field.Name)).ToList();
            }
        }

        /// <summary>
        /// Remove fields from schema that are not present in connected environment.
        /// </summary>
        public void RemoveMissingFields(Models.Entity entity)
        {
        }

        /// <summary>
        /// Remove relationships from schema that are not present in connected environment.
        /// </summary>
        public void RemoveMissingRelationships(Models.Entity entity)
        {
        }

        /// <summary>
        /// Remove fields from schema that are not present in connected environment.
        /// </summary>
        public void RemoveUnsupportedFields(Models.Entity entity)
        {
        }

        /// <summary>
        /// Remove relationships from schema that are not present in connected environment.
        /// </summary>
        public void RemoveUnsupportedRelationships(Models.Entity entity)
        {
        }

        public void SetAllEntitiesFilterOnDate(DateTime onOrAfter)
        {
            var filterXml = $@"
<filter type=""and"">
    <condition attribute=""modifiedon"" operator=""on-or-after"" value=""{onOrAfter:yyyy-MM-dd}"" />
</filter>".Trim();
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.Filter = filterXml;
            }
        }

        public void SetAllEntitiesCompareOnPrimaryKey()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                if (!string.IsNullOrEmpty(entity.PrimaryIdField) && !entity.SkipUpdate && entity.Fields?.Any() == true)
                {
                    var primaryKeyField = entity.Fields.SingleOrDefault(field => field.Name == entity.PrimaryIdField);
                    if (primaryKeyField != null)
                    {
                        foreach (var field in entity.Fields)
                        {
                            field.UpdateCompare = field == primaryKeyField;
                        }
                    }
                    else
                    {
                        Log.Error($"Unable to compare entity {entity.Name} on primary key. Primary key field {entity.PrimaryIdField} does not exist in schema.");
                    }
                }
            }
        }

        public void SetAllEntitiesDisablePlugins()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.DisablePlugins = true;
            }
        }

        public void SortAll()
        {
            SortAllEntities();
            SortAllFields();
        }

        public void SortAllEntities()
        {
            CurrentDocument.Entities = CurrentDocument.Entities.OrderBy(entity => entity.DisplayName).ToList();
        }

        public void SortAllFields()
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                entity.Fields = entity.Fields.OrderBy(field => field.DisplayName).ToList();
            }
        }

        /// <summary>
        /// Update existing schema and, add missing fields/relationships, remove invalid or missing fields/relationships.
        /// </summary>
        public void UpdateAllEntities(bool intelligently = false)
        {
            foreach (var entity in CurrentDocument.Entities)
            {
                Log.Progress($"Updating {entity.Name} entity type.");
                UpdateFields(entity, intelligently);
                UpdateEntityImageFields(entity);
                UpdateRelationships(entity, intelligently);
            }
        }

        void UpdateEntityImageFields(Models.Entity entity)
        {
            Log.Progress($"Updating fields for {entity.Name} entity type.");
            var attributesRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = entity.Name
            };
            var meta = ((RetrieveEntityResponse)Cds.Execute(attributesRequest)).EntityMetadata;
            var imageField = entity.Fields.Get("entityimage");
            var imageIdField = entity.Fields.Get("entityimageid");
            if (!string.IsNullOrEmpty(meta.PrimaryImageAttribute))
            {
                if (imageField == null)
                {
                    // Add field.
                    Log.Information($"Field {Field.EntityImage.Name} being ADDED to schema.");
                    entity.Fields.Add(Field.EntityImage);
                }
                else
                {
                    // Update field.
                    Log.Verbose($"Field {imageField.Name} being UPDATED in schema.");
                    entity.Fields.Set(Field.EntityImage);
                }
                if (imageIdField == null)
                {
                    // Add field.
                    Log.Information($"Field {Field.EntityImageId.Name} being ADDED to schema.");
                    entity.Fields.Add(Field.EntityImageId);
                }
                else
                {
                    // Update field.
                    Log.Verbose($"Field {imageIdField.Name} being UPDATED in schema.");
                    entity.Fields.Set(Field.EntityImageId);
                }
            }
            else
            {
                if (imageField != null)
                {
                    // Feature not enabled connected environment.
                    Log.Warning($"Field {imageField.Name} being REMOVED from schema because this feature is not in the connected environment.");
                    entity.Fields.Remove(imageField);
                }
                if (imageIdField != null)
                {
                    // Feature not enabled connected environment.
                    Log.Warning($"Field {imageIdField.Name} being REMOVED from schema because this feature is not in the connected environment.");
                    entity.Fields.Remove(imageIdField);
                }
            }
        }

        void UpdateFields(Models.Entity entity, bool intelligently = false)
        {
            if (intelligently)
            {
                if (entity.Fields?.Any() != true && entity.Relationships?.Any() == true)
                {
                    // Assuming only the associations are wanted.
                    Log.Progress($"Skip updating fields for {entity.Name} entity type.");
                    return;
                }
            }
            Log.Progress($"Updating fields for {entity.Name} entity type.");
            var attributesRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entity.Name
            };
            var attributes = ((RetrieveEntityResponse)Cds.Execute(attributesRequest)).EntityMetadata.Attributes;
            var fields = entity.Fields
                .Where(f => f.Name != Field.EntityImage.Name && f.Name != Field.EntityImageId.Name)
                .ToArray();
            var attributesCorrelatedLeft = from a in attributes
                                           join f in fields
                                           on a.LogicalName equals f.Name into joinGroup
                                           from j in joinGroup.DefaultIfEmpty()
                                           select (a, j);
            var attributesCorrelatedRight = from f in fields
                                            join a in attributes
                                            on f.Name equals a.LogicalName into joinGroup
                                            from j in joinGroup.DefaultIfEmpty()
                                            select (j, f);
            var attributesCorrelated = attributesCorrelatedLeft
                .Union(attributesCorrelatedRight)
                .OrderBy(x => x.Item1 != null ? x.Item1.LogicalName : x.Item2.Name)
                .ToArray();
            foreach (var (attribute, field) in attributesCorrelated)
            {
                if (attribute == null)
                {
                    // Attribute does not exist in connected environment.
                    Log.Warning($"Field {field.Name} being REMOVED from schema because it does not exist in the connected environment.");
                    entity.Fields.Remove(field);
                }
                else if (Copy.Metadata.IsUnsupported(attribute))
                {
                    Log.Silently($"Attribute {attribute.LogicalName} being IGNORED because it is not supported.");
                    if (field != null)
                    {
                        Log.Warning($"Field {field.Name} being REMOVED from schema because it is not supported.");
                        entity.Fields.Remove(field);
                    }
                }
                else if (!Copy.Metadata.IsWritable(attribute))
                {
                    Log.Silently($"Attribute {attribute.LogicalName} being IGNORED because it is not writable.");
                    if (field != null)
                    {
                        Log.Warning($"Field {field.Name} being REMOVED from schema because it is not writable.");
                        entity.Fields.Remove(field);
                    }
                }
                else if (field == null)
                {
                    // Attribute does not exist in schema.
                    Log.Information($"Attribute {attribute.LogicalName} being ADDED to schema.");
                    entity.Fields.Add(Copy.Metadata.ToField(attribute));
                }
                else
                {
                    // Update field.
                    Log.Verbose($"Attribute {attribute.LogicalName} being UPDATED in schema.");
                    Copy.Metadata.ToField(attribute, field);
                }
            }
        }

        void UpdateRelationships(Models.Entity entity, bool intelligently = false)
        {
            Log.Progress($"Updating relationships for {entity.Name} entity type.");
            var relationshipsRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Relationships,
                LogicalName = entity.Name
            };
            var relationships = ((RetrieveEntityResponse)Cds.Execute(relationshipsRequest)).EntityMetadata.ManyToManyRelationships
                .Where(r => r.Entity1LogicalName == entity.Name)
                .ToArray();
            // NOTE: CMT doesn't capture SchemaName as name for relationship - but probably should. It uses IntersectEntityName instead.
            var relationshipsCorrelatedLeft = from meta in relationships
                                              join schema in entity.Relationships
                                              on meta.IntersectEntityName equals schema.Name into joinGroup
                                              from j in joinGroup.DefaultIfEmpty()
                                              select (meta, j);
            var relationshipsCorrelatedRight = from schema in entity.Relationships
                                               join meta in relationships
                                               on schema.Name equals meta.IntersectEntityName into joinGroup
                                               from j in joinGroup.DefaultIfEmpty()
                                               select (j, schema);
            var relationshipsCorrelated = relationshipsCorrelatedRight
                .Union(relationshipsCorrelatedLeft)
                .OrderBy(x => x.Item1 != null ? x.Item1.IntersectEntityName : x.Item2.Name)
                .ToArray();
            foreach (var (meta, schema) in relationshipsCorrelated)
            {
                if (meta == null)
                {
                    // Relationship does not exist in connected environment.
                    Log.Warning($"Relationship {schema.Name} being REMOVED from schema.");
                    entity.Relationships.Remove(schema);
                }
                else if (schema == null)
                {
                    if (intelligently)
                    {
                        if (meta.IsCustomRelationship != true)
                        {
                            // Check if this is a foreign relationship.
                            var isForeignRelationship = !CurrentDocument.Entities.Exists(e => e.Name == meta.Entity2LogicalName);
                            if (isForeignRelationship)
                            {
                                // Check if schema already includes relationships with entities not included in schema.
                                var entityHasForeignRelationshipsInSchema = entity.Relationships.Any(r => !CurrentDocument.Entities.Exists(e => e.Name == r.TargetEntity));
                                if (!entityHasForeignRelationshipsInSchema)
                                {
                                    // Assuming that non-custom foreign relationships are not appropriate for automatic inclusion.
                                    Log.Information($"Relationship {meta.SchemaName} being IGNORED.");
                                    continue;
                                }
                            }
                        }
                    }
                    // Relationship does not exist in schema.
                    Log.Information($"Relationship {meta.SchemaName} being ADDED to schema.");
                    entity.Relationships.Add(Copy.Metadata.ToRelationship(meta));
                }
                else
                {
                    // Update relationship.
                    Log.Verbose($"Relationship {meta.SchemaName} being UPDATED in schema.");
                    Copy.Metadata.ToRelationship(meta, schema);
                }
            }
        }

        Document CurrentDocument = new Document();
        readonly IOrganizationService Cds;
    }
}
