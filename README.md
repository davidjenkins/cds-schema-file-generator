# Example

``` c#
var cds = new CrmServiceClient("AuthType=Office365;Url=https://contoso.crm.dynamics.com;Username=bill;Password=gates");
var generator = new CdsSchemaFileGenerator(cds);
var path = @"C:\path\to\schema.xml";
generator.ImportSchemaFile(path);
generator.UpdateAllEntities(true);
generator.SetAllEntitiesFilterOnDate(DateTime.Now);
generator.RemoveAllEntitiesFilter();
generator.SetAllEntitiesCompareOnPrimaryKey();
generator.SetAllEntitiesDisablePlugins();
generator.RemoveAllInstanceFields();
generator.RemoveAllTenantFields();
generator.RemoveAllInstancePortalFields();
generator.RemoveAllIrrelevantFields();
generator.SortAll();
generator.ExportSchemaFile(path);
```