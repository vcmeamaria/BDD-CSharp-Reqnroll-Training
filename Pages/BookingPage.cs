using System;
using System.Linq;
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
        _driver.Navigate().GoToUrl(
            "https://automationintesting.online/");

        Visible(_checkAvailabilityButton);
    }

    public void SearchDisplayedStay()
    {
        ClickWhenReady(_checkAvailabilityButton);
    }

    public void OpenFirstAvailableRoom()
    {
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
                    JavaScriptClick(roomLink);
                }

                return true;
            }
            catch (StaleElementReferenceException)
            {
                // The page refreshed/re-rendered.
                // Reqnroll/Selenium will retry and locate
                // the current version of the room element.
                return false;
            }
        });

        if (!clicked)
        {
            throw new InvalidOperationException(
                "No available room could be opened.");
        }

        Visible(_bookThisRoomHeading);
    }

    public void StartReservation()
    {
        ClickWhenReady(_reserveNowButton);

        Visible(_firstNameInput);
    }

    public void SubmitEmptyGuestForm()
    {
        ClickWhenReady(_reserveNowButton);
    }

    public string GetValidationMessage()
    {
        return Visible(_validationAlert).Text;
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
                    JavaScriptClick(element);
                }

                return true;
            }
            catch (StaleElementReferenceException)
            {
                // Find the element again on the next wait attempt.
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