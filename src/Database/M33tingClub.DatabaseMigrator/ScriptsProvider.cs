using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.ScriptProviders;

namespace M33tingClub.DatabaseMigrator;

internal class ScriptsProvider : IScriptProvider
{
    private readonly string _path;

    public ScriptsProvider(string path)
    {
        _path = path;
    }
    
    public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
    {
        var fileSystemScriptProvider = new FileSystemScriptProvider(_path, new FileSystemScriptOptions
        {
            IncludeSubDirectories = true
        });

        return fileSystemScriptProvider.GetScripts(connectionManager).OrderBy(x => x.Name).ToList();
    }
}