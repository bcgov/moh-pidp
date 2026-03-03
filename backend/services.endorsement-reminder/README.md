# Endorsement Reminder Service

This service is a standalone .NET console app that performs endorsement request maintenance tasks for OneHealthID.
It runs once triggered by an Openshift cron job, sends reminder emails for stalled endorsement requests, expires old requests, and then exits.

## What it does

On startup, the hosted service executes the following in order:

1. **Send 7-day reminder emails** for endorsement requests in `Created`, `Received`, or `Approved` which are considered as incomplete status, once an endorsement request reaches 7 days and has yet to be completed.
2. **Expire old requests** by marking requests in the incomplete status as `Expired` when they have been incomplete for over 30 days.


## Notes

- This service is intended for scheduled execution, not as a long-running API.

