using DbUp;

namespace M33tingClub.DatabaseMigrator;

class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("Database Migrator!");

        var targetEnvironment = args[0];
        
        var connectionString = args[0];

        var migrationScriptsPath = args[1];

        Console.WriteLine($"Path: {migrationScriptsPath}");
        
        var migrator = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScripts(new ScriptsProvider(migrationScriptsPath))
            .JournalToPostgresqlTable("app", "migrations_journal")
            .Build();

        var result = migrator.PerformUpgrade();
        if (!result.Successful)
        {
            Console.WriteLine("Error during migrations...");
            return -1;
        }

        return 0;
    }
}