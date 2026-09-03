# C# BDD Reqnroll Framework

A Behaviour-Driven Development automation testing framework built with C#, Reqnroll, NUnit and Selenium WebDriver.

The framework uses Gherkin feature files, reusable step definitions, Page Object Model, browser hooks and Selenium WebDriver to create readable and maintainable automated tests.

## Run

Restore the project dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run all tests:

```bash
dotnet test
```

Tests can also be executed through Visual Studio Test Explorer.

## Demo Test Suite

Two BDD scenarios are currently included for demonstrating Reqnroll bindings and browser automation.

| Scenario | Type | Description |
|---|---|---|
| `Successful login with valid credentials` | BDD / Binding | Demonstrates how Gherkin steps are mapped to C# Reqnroll step definitions |
| `Required guest details are validated` | UI / Validation | Opens the Shady Meadows booking site and verifies required-field validation on the reservation form |

The room reservation scenario runs against:

```text
https://automationintesting.online/
```

The test:

```text
Open booking site
        ↓
Search displayed stay
        ↓
Open first available room
        ↓
Start reservation
        ↓
Submit empty guest form
        ↓
Verify validation errors
```

No reservation is created.

## Project Structure

```text
Drivers/
└── DriverManager.cs                         # Creates, provides and closes Selenium WebDriver

Features/
├── Login.feature                            # Introductory BDD login scenario
└── RoomReservationValidation.feature        # UI reservation validation scenario

Pages/
└── BookingPage.cs                           # Shady Meadows selectors and browser interactions

StepDefinitions/
├── LoginSteps.cs                            # Login Gherkin bindings
└── RoomReservationValidationSteps.cs        # Reservation validation Gherkin bindings

Support/
└── Hooks.cs                                 # Starts and closes Chrome for @ui scenarios

BddTraining.csproj                           # Project dependencies
BddTraining.slnx                             # Visual Studio solution
.gitignore                                   # Git exclusions
README.md                                    # Project documentation
```

## Framework Flow

```text
Gherkin Feature
      ↓
Step Definitions
      ↓
Page Object
      ↓
Driver Manager
      ↓
Selenium WebDriver
      ↓
Browser
```

Reqnroll connects the business-readable Gherkin scenarios to executable C# step definitions.

The Page Object Model keeps Selenium selectors and browser interactions separate from the BDD step definitions.

## Reliability

The Shady Meadows application is a shared public testing environment, so room availability and displayed dates may change.

The framework therefore avoids hard-coding:

- Room names
- Room prices
- Calendar dates

Instead, the test uses the currently displayed stay and selects the first available room.

The framework also handles intercepted Selenium clicks by:

1. Scrolling the target element into view.
2. Waiting until the element is displayed and enabled.
3. Attempting a normal Selenium click.
4. Using a JavaScript click only when an `ElementClickInterceptedException` occurs.

## Features

- Behaviour-Driven Development
- Reqnroll
- NUnit
- Gherkin feature files
- Given, When and Then scenarios
- Selenium WebDriver
- Page Object Model
- Reusable step definitions
- Browser lifecycle hooks
- `@ui` scenario tags
- Explicit Selenium waits
- Dynamic room selection
- Required-field validation testing
- Click interception handling
- Visual Studio Test Explorer support
- .NET CLI test execution
- Git feature-branch workflow