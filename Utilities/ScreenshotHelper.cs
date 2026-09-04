using OpenQA.Selenium;

namespace BddTraining.Utilities;

public static class ScreenshotHelper
{
    public static string Capture(
        IWebDriver driver,
        string scenarioName)
    {
        var projectRoot = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                ".."));

        var screenshotDirectory = Path.Combine(
            projectRoot,
            "artifacts",
            "screenshots");

        Directory.CreateDirectory(screenshotDirectory);

        var timestamp =
            DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");

        var safeScenarioName =
            MakeSafeFileName(scenarioName);

        var fileName =
            $"{safeScenarioName}_{timestamp}.png";

        var filePath = Path.Combine(
            screenshotDirectory,
            fileName);

        var screenshot =
            ((ITakesScreenshot)driver).GetScreenshot();

        screenshot.SaveAsFile(filePath);

        return filePath;
    }

    private static string MakeSafeFileName(string value)
    {
        foreach (var invalidCharacter
                 in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(
                invalidCharacter,
                '_');
        }

        return value.Replace(' ', '_');
    }
}