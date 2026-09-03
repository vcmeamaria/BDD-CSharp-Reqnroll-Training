using BddTraining.Drivers;
using BddTraining.Pages;
using NUnit.Framework;
using Reqnroll;

namespace BddTraining.StepDefinitions;

[Binding]
public sealed class RoomReservationValidationSteps
{
    private readonly BookingPage _bookingPage;

    public RoomReservationValidationSteps()
    {
        _bookingPage = new BookingPage(DriverManager.Driver);
    }

    [Given("the guest opens the Shady Meadows booking site")]
    public void OpenBookingSite()
    {
        _bookingPage.Open();
    }

    [When("the guest searches for the displayed one-night stay")]
    public void SearchDisplayedStay()
    {
        _bookingPage.SearchDisplayedStay();
    }

    [When("opens the first available room")]
    public void OpenFirstAvailableRoom()
    {
        _bookingPage.OpenFirstAvailableRoom();
    }

    [When("starts the reservation")]
    public void StartReservation()
    {
        _bookingPage.StartReservation();
    }

    [When("submits the guest details form without entering details")]
    public void SubmitEmptyGuestForm()
    {
        _bookingPage.SubmitEmptyGuestForm();
    }

    [Then("booking validation errors should be displayed")]
    public void VerifyValidationErrors()
    {
        var message = _bookingPage.GetValidationMessage();

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
}