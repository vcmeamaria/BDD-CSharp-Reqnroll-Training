using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BddTraining.Drivers;

public static class DriverManager
{
    private static IWebDriver? _driver;

    public static bool IsDriverStarted =>
        _driver is not null;

    public static IWebDriver Driver =>
        _driver ?? throw new InvalidOperationException(
            "The WebDriver has not been started.");

    public static void StartDriver()
    {
        if (_driver is not null)
        {
            return;
        }

        _driver = new ChromeDriver();

        _driver.Manage().Window.Maximize();
    }

    public static void QuitDriver()
    {
        if (_driver is null)
        {
            return;
        }

        _driver.Quit();
        _driver.Dispose();

        _driver = null;
    }
}