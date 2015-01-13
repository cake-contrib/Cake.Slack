///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target          = Argument<string>("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var isLocalBuild    = !AppVeyor.IsRunningOnAppVeyor;
var solutions       = GetFiles("./**/*.sln");
var solutionDirs    = solutions.Select(solution => solution.GetDirectory());
var releaseNotes    = ParseReleaseNotes("./ReleaseNotes.md");
var version         = releaseNotes.Version.ToString();
var semVersion      = isLocalBuild ? version : (version + string.Concat("-build-", AppVeyor.Environment.Build.Number));
var assemblyInfo    = new AssemblyInfoSettings {
                            Product                 = "Cake.Slack",
                            Company                 = "WCOM AB",
                            Version                 = version,
                            FileVersion             = version,
                            InformationalVersion    = semVersion,
                            Copyright               = string.Format("Copyright © WCOM AB {0}", DateTime.Now.Year),
                            CLSCompliant            = true
                        };

///////////////////////////////////////////////////////////////////////////////
// Output some information about the current build.
///////////////////////////////////////////////////////////////////////////////
Information("Building version {0} of {1} ({2}).", version, assemblyInfo.Product, semVersion);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var solutionDir in solutionDirs)
    {
        Information("Cleaning {0}", solutionDir);
        CleanDirectories(solutionDir + "/**/bin/" + configuration);
        CleanDirectories(solutionDir + "/**/obj/" + configuration);
    }
});

Task("Restore")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        NuGetRestore(solution);
    }
});

Task("SolutionInfo")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var file = "./src/SolutionInfo.cs";
    CreateAssemblyInfo(file, assemblyInfo);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SolutionInfo")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}", solution);
        MSBuild(solution, settings => 
            settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithProperty("TreatWarningsAsErrors","true")
                .WithTarget("Build")
                .SetConfiguration(configuration));
    }
});

Task("Default")
    .IsDependentOn("Build");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
