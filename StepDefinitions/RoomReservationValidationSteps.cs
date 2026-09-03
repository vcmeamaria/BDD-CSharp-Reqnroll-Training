using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BddTraining.StepDefinitions;

[Binding]
public sealed class RoomReservationValidationSteps
{
    private IWebDriver? _driver;
    private WebDriverWait? _wait;

    [BeforeScenario("@ui")]
    public void StartBrowser()
    {
        _driver = new ChromeDriver();

        _driver.Manage().Window.Maximize();

        _wait = new WebDriverWait(
            _driver,
            TimeSpan.FromSeconds(10));
    }

    [Given("the guest opens the Shady Meadows booking site")]
    public void OpenBookingSite()
    {
        _driver!.Navigate().GoToUrl(
            "https://automationintesting.online/");

        Visible(
            By.XPath("//button[normalize-space()='Check Availability']"));
    }

    [When("the guest searches for the displayed one-night stay")]
    public void SearchDisplayedStay()
    {
        ClickWhenReady(
            By.CssSelector("#booking button.btn-primary"));
    }

    [When("opens the first available room")]
    public void OpenFirstAvailableRoom()
    {
        var link = _wait!.Until(driver =>
            driver
                .FindElements(
                    By.CssSelector("#rooms a.btn.btn-primary"))
                .FirstOrDefault(element =>
                    element.Displayed &&
                    element.Text.Equals(
                        "Book now",
                        StringComparison.OrdinalIgnoreCase)));

        Assert.That(
            link,
            Is.Not.Null,
            "No available room was displayed.");

        ClickElement(link!);

        Visible(
            By.XPath("//h2[normalize-space()='Book This Room']"));
    }

    [When("starts the reservation")]
    public void StartReservation()
    {
        ClickWhenReady(
            By.XPath("//button[normalize-space()='Reserve Now']"));

        Visible(
            By.CssSelector("input[name='firstname']"));
    }

    [When("submits the guest details form without entering details")]
    public void SubmitEmptyGuestForm()
    {
        ClickWhenReady(
            By.XPath("//button[normalize-space()='Reserve Now']"));
    }

    [Then("booking validation errors should be displayed")]
    public void VerifyValidationErrors()
    {
        var message =
            Visible(By.CssSelector(".alert.alert-danger")).Text;

        Assert.Multiple(() =>
        {
            Assert.That(
                message,
                Does.Contain("Firstname should not be blank"));

            Assert.That(
                message,
                Does.Contain("Lastname should not be blank"));
        });
    }

    private IWebElement Visible(By by)
    {
        return _wait!.Until(driver =>
        {
            var element = driver.FindElement(by);

            return element.Displayed
                ? element
                : null;
        })!;
    }

    private void ClickWhenReady(By by)
    {
        var element = Visible(by);

        ClickElement(element);
    }

    private void ClickElement(IWebElement element)
    {
        ((IJavaScriptExecutor)_driver!)
            .ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center'});",
                element);

        _wait!.Until(_ =>
            element.Displayed &&
            element.Enabled);

        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)_driver!)
                .ExecuteScript(
                    "arguments[0].click();",
                    element);
        }
    }

    [AfterScenario("@ui")]
    public void StopBrowser()
    {
        _driver?.Quit();
    }
}