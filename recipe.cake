#tool dotnet:?package=DPI&version=2021.3.11.25
#load nuget:?package=Cake.Recipe&version=2.2.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.Slack",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.Slack",
                            appVeyorAccountName: "cakecontrib",
                            shouldRunDupFinder: false,
                            shouldRunInspectCode: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                                BuildParameters.RootDirectoryPath + "/src/Cake.Slack/**/*.AssemblyInfo.cs",
                                BuildParameters.RootDirectoryPath + "/src/Cake.Slack/LitJson/**/*.cs" });

Task("DPI")
    .IsDependeeOf("DotNetCore-Build")
    .Does<BuildVersion>(
        (context, buildVersion) => {
    var result = context.StartProcess(
        context.Tools.Resolve("dpi") ?? context.Tools.Resolve("dpi.exe"),
        new ProcessSettings {
            Arguments = new ProcessArgumentBuilder()
                                                .Append("nuget")
                                                .Append("--silent")
                                                .AppendSwitchQuoted("--output", "table")
                                                .Append(
                                                    (
                                                        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("NuGetReportSettings_SharedKey"))
                                                        &&
                                                        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("NuGetReportSettings_WorkspaceId"))
                                                    )
                                                        ? "report"
                                                        : "analyze"
                                                    )
                                                .AppendSwitchQuoted("--buildversion", buildVersion.SemVersion)
        }
    );

    if (result != 0)
    {
        throw new Exception($"Failed to execute DPI ({result}");
    }
});

Build.RunDotNetCore();
