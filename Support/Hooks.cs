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

    [BeforeScenario("@ui")]
    public void BeforeUiScenario()
    {
        DriverManager.StartDriver();
    }

    [AfterScenario("@ui")]
    public void AfterUiScenario()
    {
        try
        {
            if (_scenarioContext.TestError is not null &&
                DriverManager.IsDriverStarted)
            {
                var screenshotPath =
                    ScreenshotHelper.Capture(
                        DriverManager.Driver,
                        _scenarioContext.ScenarioInfo.Title);

                Console.WriteLine(
                    $"Failure screenshot saved: {screenshotPath}");

                AllureApi.AddAttachment(
                    "Failure Screenshot",
                    "image/png",
                    screenshotPath);
            }
        }
        finally
        {
            DriverManager.QuitDriver();
        }
    }
}