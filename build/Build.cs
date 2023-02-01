using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    AbsolutePath OutputDatabaseMigratorBuildDirectory => OutputDirectory / "DatabaseMigrator";

    AbsolutePath DatabaseMigratorPath => OutputDatabaseMigratorBuildDirectory / "M33tingClub.DatabaseMigrator.dll";

    AbsolutePath MigrationScriptsPath => RootDirectory / "src" / "Database" / "M33tingClub.Database" / "Migrations";
    
    Target CompileDbUpMigrator => _ => _
        .Executes(() =>
        {
            var dbUpMigratorProject = Solution.GetProject("M33tingClub.DatabaseMigrator");
            DotNetBuild(s => s
                .SetProjectFile(dbUpMigratorProject)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(OutputDatabaseMigratorBuildDirectory)
            );
        });
    
    [Parameter("Connnection string")] readonly string ConnectionString;
    Target MigrateDatabase => _ => _
        .Requires(() => ConnectionString != null)
        .DependsOn(CompileDbUpMigrator)
        .Executes(() =>
        {
            DotNet($"{DatabaseMigratorPath} {ConnectionString} {MigrationScriptsPath}");
        });
}
