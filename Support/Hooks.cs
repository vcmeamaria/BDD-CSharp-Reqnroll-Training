using Allure.Net.Commons;
using BddTraining.Drivers;
using BddTraining.Utilities;
using Reqnroll;

namespace BddTraining.Support;

[Binding]
public sealed class Hooks
{
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        LogManager.Initialise();

        LogManager.Information(
            "BDD test run started");
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        LogManager.Information(
            "Starting scenario: {ScenarioName}",
            _scenarioContext.ScenarioInfo.Title);
    }

    [BeforeScenario("@ui")]
    public void BeforeUiScenario()
    {
        LogManager.Information(
            "Starting browser for UI scenario");

        DriverManager.StartDriver();

        LogManager.Information(
            "Chrome browser started");
    }

    [AfterScenario("@ui")]
    public void AfterUiScenario()
    {
        try
        {
            if (_scenarioContext.TestError is not null &&
                DriverManager.IsDriverStarted)
            {
                LogManager.Warning(
                    "UI scenario failed. Capturing screenshot.");

                var screenshotPath =
                    ScreenshotHelper.Capture(
                        DriverManager.Driver,
                        _scenarioContext.ScenarioInfo.Title);

                LogManager.Information(
                    "Failure screenshot saved: {ScreenshotPath}",
                    screenshotPath);

                AllureApi.AddAttachment(
                    "Failure Screenshot",
                    "image/png",
                    screenshotPath);

                LogManager.Information(
                    "Failure screenshot attached to Allure report");
            }
        }
        catch (Exception exception)
        {
            LogManager.Error(
                exception,
                "Failed while processing UI scenario cleanup");
        }
        finally
        {
            if (DriverManager.IsDriverStarted)
            {
                LogManager.Information(
                    "Closing Chrome browser");

                DriverManager.QuitDriver();

                LogManager.Information(
                    "Chrome browser closed");
            }
        }
    }

    [AfterScenario]
    public void AfterScenario()
    {
        if (_scenarioContext.TestError is null)
        {
            LogManager.Information(
                "Scenario passed: {ScenarioName}",
                _scenarioContext.ScenarioInfo.Title);
        }
        else
        {
            LogManager.Error(
                _scenarioContext.TestError,
                "Scenario failed: {ScenarioName}",
                _scenarioContext.ScenarioInfo.Title);
        }
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        LogManager.Information(
            "BDD test run completed");

        LogManager.Close();
    }
}