# Room Reservation Validation - Lab Evidence

## Scenario

**Feature:** Room reservation validation

**Scenario:** Required guest details are validated

The scenario validates the required guest fields on the Shady Meadows B&B booking form without creating a reservation.

---

## Manual Exploration

The application was manually explored before validating the automated scenario.

The following journey was completed:

```text
Open Shady Meadows
        ↓
Check Availability
        ↓
Open the first available room
        ↓
Select Reserve Now
        ↓
Leave all guest details empty
        ↓
Select Reserve Now again
        ↓
Validation alert displayed