namespace MongoDbService.Models;

public class MongoDbSettings
{
    public readonly string ConnString;
    public readonly string DatabaseName;

    public MongoDbSettings(string connString, string databaseName)
    {
        ConnString = connString;
        DatabaseName = databaseName;
    }
}