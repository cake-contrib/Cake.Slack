#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease&version=0.3.0-unstable0350

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

Build.RunDotNetCore();