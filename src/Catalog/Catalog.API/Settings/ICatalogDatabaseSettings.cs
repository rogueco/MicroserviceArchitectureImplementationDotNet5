namespace Catalog.API.Settings
{
    public interface ICatalogDatabaseSettings
    {
        string DatabaseName { get; set; }
        
        string CollectionString { get; set; }
        
        string CollectionName { get; set; }
    }
}