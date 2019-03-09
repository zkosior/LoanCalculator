#addin "Cake.FileHelpers&version=3.1.0"
#addin nuget:?package=Cake.ArgumentHelpers&version=0.3.0
#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=ReportGenerator&version=4.0.14"

// Setup Parameters
var runtime = ArgumentOrEnvironmentVariable("Runtime", "") ?? "win81-x64";
var configuration = "Release";
var solution = "../LoanCalculator.sln";

// Log Parameters
Information("========================================");
Information("BUILD PARAMETERS");
Information("========================================");
Information($"Configuration: {configuration}");
Information($"Runtime: {runtime}");

DotNetCoreTestSettings TestSettings
{
    get
    {
        return new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
            ArgumentCustomization = args => args.Append("-v normal"),
        };
    }
}

Task("Clean")
    .Does(() =>
    {
        CleanDirectories("../**/**/bin");
        Information("Cleaning 'bin' folders.");
        CleanDirectories("../**/**/obj");
        Information("Cleaning 'obj' folders.");
        CleanDirectories("../publish");
        Information("Cleaning 'publish' folder.");
        CleanDirectories("../coverage");
        Information("Cleaning 'coverage' folder.");
    });

Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore(
            solution,
            new DotNetCoreRestoreSettings
            {
                Sources = new[]
                {
                    "https://api.nuget.org/v3/index.json",
                },
				Runtime = runtime,
            });
    });

Task("BuildForTests")
    .Does(() =>
    {
        DotNetCoreBuild(
            solution,
            new DotNetCoreBuildSettings
            {
                Configuration = configuration,
                NoRestore = true,
            });
    });

Task("UnitTests")
    .Does(() =>
    {
        var testSettings = TestSettings;
        DotNetCoreTest(
            "../test/Tests/Tests.csproj",
        testSettings);
    });

Task("Coverage")
    .Does(() =>
    {
        var outputFolder = "../coverage";
        var reportFile = new FilePath($"{outputFolder}/coverage.xml");
        EnsureDirectoryExists(outputFolder);
        OpenCover(tool =>
        {
            tool.DotNetCoreTest(
                solution,
                TestSettings);
        },
        reportFile,
        new OpenCoverSettings()
        {
            ArgumentCustomization = args => args.Append("-oldstyle"),
        }
        .WithFilter("+[LoanCalculator*]*")
        .WithFilter("-[*Tests*]*")
        .ExcludeByAttribute("*.ExcludeFromCodeCoverageAttribute"));
        try
        {
            ReportGenerator(reportFile, outputFolder);
        }
        catch (Exception e)
        {
            Information("Generating coverage report failed.");
            Information(e);
        }
    });

Task("BuildConsoleApp")
    .Does(() =>
    {
        DotNetCorePublish(
            "../src/ConsoleApp/ConsoleApp.csproj",
            new DotNetCorePublishSettings
            {
                Framework = "netcoreapp2.1",
                Configuration = configuration,
				Runtime = runtime,
                OutputDirectory = "../publish",
                SelfContained = false,
            });
    });

Task("Test")
    .IsDependentOn("BuildForTests")
    .IsDependentOn("Coverage");

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    //.IsDependentOn("Test")
    .IsDependentOn("BuildConsoleApp");

var target = Argument("target", "Default");

RunTarget(target);