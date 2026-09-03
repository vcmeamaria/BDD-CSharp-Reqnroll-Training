using BddTraining.Drivers;
using Reqnroll;

namespace BddTraining.Support;

[Binding]
public sealed class Hooks
{
    [BeforeScenario("@ui")]
    public void BeforeUiScenario()
    {
        DriverManager.StartDriver();
    }

    [AfterScenario("@ui")]
    public void AfterUiScenario()
    {
        DriverManager.QuitDriver();
    }
}