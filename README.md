# C# BDD Reqnroll Framework

A Behaviour-Driven Development automation testing framework built with C#, Reqnroll, NUnit and Selenium WebDriver.

The framework uses Gherkin feature files, reusable step definitions, Page Object Model, browser hooks, Selenium WebDriver and Allure reporting to create readable and maintainable automated tests.

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

### Allure Report

Run the tests to generate the Allure result files:

```bash
dotnet test
```

Open the Allure report:

```bash
allure serve bin/Debug/net8.0/allure-results
```

The Allure report includes:

- BDD features and scenarios
- Passed and failed test results
- Scenario execution details
- Failure information
- Screenshots attached automatically when a UI scenario fails

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

Evidence/
└── RoomReservationValidationEvidence.md     # Lab evidence and reliability notes

Features/
├── Login.feature                            # Introductory BDD login scenario
└── RoomReservationValidation.feature        # UI reservation validation scenario

Pages/
└── BookingPage.cs                           # Shady Meadows selectors and browser interactions

StepDefinitions/
├── LoginSteps.cs                            # Login Gherkin bindings
└── RoomReservationValidationSteps.cs        # Reservation validation Gherkin bindings

Support/
└── Hooks.cs                                 # Browser lifecycle, failure detection and Allure attachments

Utilities/
└── ScreenshotHelper.cs                      # Captures timestamped screenshots on failure

BddTraining.csproj                           # Project dependencies
BddTraining.slnx                             # Visual Studio solution
.gitignore                                   # Git exclusions
README.md                                    # Project documentation
```

Generated test artifacts are stored locally and excluded from Git:

```text
artifacts/
└── screenshots/
    └── Scenario_name_YYYY-MM-DD_HH-mm-ss-fff.png
```

Allure result files are generated under:

```text
bin/Debug/net8.0/allure-results/
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

For UI scenarios tagged with `@ui`, Reqnroll hooks automatically start and close the browser.

```text
@ui Scenario
      ↓
BeforeScenario Hook
      ↓
Start Chrome
      ↓
Execute Scenario
      ↓
AfterScenario Hook
      ↓
Failure?
 ┌────┴────┐
No        Yes
│          ↓
│     Capture Screenshot
│          ↓
│     Attach to Allure
│          ↓
└────→ Close Chrome
```

## Screenshots on Failure

Screenshots are captured automatically when a UI scenario fails.

The screenshot filename contains:

- Scenario name
- Date
- Time

Example:

```text
Required_guest_details_are_validated_2026-09-04_11-09-47-474.png
```

Failure screenshots are saved under:

```text
artifacts/screenshots/
```

They are also attached directly to the failed scenario inside the Allure report.

Screenshots are not created for successful scenarios.

## Allure Reporting

The framework uses Allure with Reqnroll to provide readable test reports.

The report displays:

- Features
- Scenarios
- Test status
- Execution duration
- Failed steps
- Error messages
- Failure screenshots

Run:

```bash
dotnet test
```

Then open the report with:

```bash
allure serve bin/Debug/net8.0/allure-results
```

## Reliability

The Shady Meadows application is a shared public testing environment, so room availability, displayed dates and page behaviour may change.

The framework therefore avoids hard-coding:

- Room names
- Room prices
- Calendar dates

Instead, the test uses the currently displayed stay and selects the first available room.

### Click Interception

The application can occasionally cause another page element to intercept a Selenium click.

The framework handles this by:

1. Scrolling the target element into view.
2. Waiting until the element is displayed and enabled.
3. Attempting a normal Selenium click.
4. Using a JavaScript click only when an `ElementClickInterceptedException` occurs.

### Stale Elements

The application can also re-render elements after availability is checked.

This can cause Selenium to receive a `StaleElementReferenceException`.

The framework handles this by:

1. Detecting the stale element.
2. Locating the current version of the element again.
3. Retrying the interaction through the explicit wait.

This improves reliability on the shared demonstration environment.

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
- Stale element retry handling
- Failure screenshot capture
- Timestamped screenshot filenames
- Allure reporting
- Failure screenshots attached to Allure
- Visual Studio Test Explorer support
- .NET CLI test execution
- Git feature-branch workflow