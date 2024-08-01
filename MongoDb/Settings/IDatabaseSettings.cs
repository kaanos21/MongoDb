namespace MongoDb.Settings
{
    public interface IDatabaseSettings
    {
        
            string CategoryCollectionName { get; set; }
            string ProductCollectionName { get; set; }
            string ConnectionString { get; set; }
            string DatabaseName { get; set; }
        
    }
}
