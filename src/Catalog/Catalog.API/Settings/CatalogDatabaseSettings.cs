namespace Catalog.API.Settings
{
    public class CatalogDatabaseSettings : ICatalogDatabaseSettings
    {
        public string DatabaseName { get; set; }
        
        public string CollectionString { get; set; }
        
        public string CollectionName { get; set; }
    }
}