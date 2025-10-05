#tool dotnet:?package=DPI&version=2021.12.8.49
#load nuget:https://pkgs.dev.azure.com/cake-contrib/Home/_packaging/addins/nuget/v3/index.json?package=Cake.Recipe&version=4.1.0-alpha0036

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.Slack",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.Slack",
                            appVeyorAccountName: "cakecontrib",
                            shouldRunInspectCode: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Task("DPI")
    .IsDependeeOf("DotNet-Build")
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

Build.RunDotNet();
