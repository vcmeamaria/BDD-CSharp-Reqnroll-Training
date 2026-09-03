using NUnit.Framework;
using Reqnroll;

namespace BddTraining.StepDefinitions;

[Binding]
public sealed class LoginSteps
{
    private string? _username;
    private string? _password;
    private bool _isLoggedIn;

    [Given("the user is on the login page")]
    public void GivenTheUserIsOnTheLoginPage()
    {
        Console.WriteLine("User is on the login page");
    }

    [When("they enter a valid username and password")]
    public void WhenTheyEnterValidCredentials()
    {
        _username = "testuser";
        _password = "password123";

        _isLoggedIn =
            !string.IsNullOrWhiteSpace(_username) &&
            !string.IsNullOrWhiteSpace(_password);
    }

    [Then("they should be redirected to the dashboard")]
    public void ThenTheyShouldBeRedirectedToTheDashboard()
    {
        Assert.That(_isLoggedIn, Is.True, "User is not logged in");
    }
}