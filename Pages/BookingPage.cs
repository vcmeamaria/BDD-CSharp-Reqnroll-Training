using System;
using System.Linq;
using BddTraining.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BddTraining.Pages;

public sealed class BookingPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    private readonly By _checkAvailabilityButton =
        By.CssSelector("#booking button.btn-primary");

    private readonly By _availableRoomLinks =
        By.CssSelector("#rooms a.btn.btn-primary");

    private readonly By _bookThisRoomHeading =
        By.XPath("//h2[normalize-space()='Book This Room']");

    private readonly By _reserveNowButton =
        By.XPath("//button[normalize-space()='Reserve Now']");

    private readonly By _firstNameInput =
        By.CssSelector("input[name='firstname']");

    private readonly By _validationAlert =
        By.CssSelector(".alert.alert-danger");

    public BookingPage(IWebDriver driver)
    {
        _driver = driver;

        _wait = new WebDriverWait(
            _driver,
            TimeSpan.FromSeconds(10));
    }

    public void Open()
    {
        LogManager.Information(
            "Opening Shady Meadows booking site");

        _driver.Navigate().GoToUrl(
            "https://automationintesting.online/");

        Visible(_checkAvailabilityButton);

        LogManager.Information(
            "Shady Meadows booking site loaded");
    }

    public void SearchDisplayedStay()
    {
        LogManager.Information(
            "Searching for the displayed one-night stay");

        ClickWhenReady(_checkAvailabilityButton);

        LogManager.Information(
            "Availability search submitted");
    }

    public void OpenFirstAvailableRoom()
    {
        LogManager.Information(
            "Searching for the first available room");

        var clicked = _wait.Until(driver =>
        {
            try
            {
                var roomLink = driver
                    .FindElements(_availableRoomLinks)
                    .FirstOrDefault(element =>
                        element.Displayed &&
                        element.Enabled &&
                        element.Text.Equals(
                            "Book now",
                            StringComparison.OrdinalIgnoreCase));

                if (roomLink is null)
                {
                    return false;
                }

                ScrollIntoView(roomLink);

                try
                {
                    roomLink.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    LogManager.Warning(
                        "Normal Selenium click was intercepted. Using JavaScript click fallback.");

                    JavaScriptClick(roomLink);
                }

                return true;
            }
            catch (StaleElementReferenceException)
            {
                LogManager.Warning(
                    "Available room element became stale. Retrying with a fresh element.");

                return false;
            }
        });

        if (!clicked)
        {
            throw new InvalidOperationException(
                "No available room could be opened.");
        }

        Visible(_bookThisRoomHeading);

        LogManager.Information(
            "First available room opened");
    }

    public void StartReservation()
    {
        LogManager.Information(
            "Starting room reservation");

        ClickWhenReady(_reserveNowButton);

        Visible(_firstNameInput);

        LogManager.Information(
            "Guest details form displayed");
    }

    public void SubmitEmptyGuestForm()
    {
        LogManager.Information(
            "Submitting guest details form without entering details");

        ClickWhenReady(_reserveNowButton);

        LogManager.Information(
            "Empty guest details form submitted");
    }

    public string GetValidationMessage()
    {
        LogManager.Information(
            "Reading booking validation messages");

        var message = Visible(_validationAlert).Text;

        LogManager.Information(
            "Booking validation alert displayed");

        return message;
    }

    private IWebElement Visible(By by)
    {
        return _wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(by);

                return element.Displayed
                    ? element
                    : null;
            }
            catch (StaleElementReferenceException)
            {
                LogManager.Warning(
                    "Element became stale while waiting for visibility. Retrying.");

                return null;
            }
        })!;
    }

    private void ClickWhenReady(By by)
    {
        _wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(by);

                if (!element.Displayed ||
                    !element.Enabled)
                {
                    return false;
                }

                ScrollIntoView(element);

                try
                {
                    element.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    LogManager.Warning(
                        "Normal Selenium click was intercepted. Using JavaScript click fallback.");

                    JavaScriptClick(element);
                }

                return true;
            }
            catch (StaleElementReferenceException)
            {
                LogManager.Warning(
                    "Element became stale before it could be clicked. Retrying.");

                return false;
            }
        });
    }

    private void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)_driver)
            .ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center'});",
                element);
    }

    private void JavaScriptClick(IWebElement element)
    {
        ((IJavaScriptExecutor)_driver)
            .ExecuteScript(
                "arguments[0].click();",
                element);
    }
}