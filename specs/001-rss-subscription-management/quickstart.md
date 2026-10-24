# Quickstart: RSS Subscription Management

This guide validates the MVP end to end after the application projects have been scaffolded in
the planned `backend/` and `frontend/` locations. The repository currently has no solution,
application projects, or test projects, so the run commands become executable once those
foundational projects exist.

## Prerequisites

- A supported .NET SDK matching the target framework selected when the projects are scaffolded.
- A modern browser.
- Backend and frontend local origins matching the API URL and CORS settings. The stakeholder
  development defaults are backend `http://localhost:5151` and frontend
  `http://localhost:5213`.

## Run the focused automated checks

From the repository root:

```sh
dotnet test backend/RSSFeedReader.Api.Tests
dotnet build backend/RSSFeedReader.Api
dotnet build frontend/RSSFeedReader.UI
```

Expected results: tests pass, both projects build, and no feed-fetching or parsing dependency is
needed for the MVP.

## Run the application

In separate terminals from the repository root:

```sh
dotnet run --project backend/RSSFeedReader.Api
```

```sh
dotnet run --project frontend/RSSFeedReader.UI
```

Open the frontend's reported local URL. Verify that the frontend origin is allowed by the
backend's CORS configuration and that its configured API base URL points to the running backend.

## Validate the subscription workflow

1. Open the subscription page and confirm the list starts empty.
2. Submit `https://devblogs.microsoft.com/dotnet/feed/`.
3. Confirm the exact URL appears in the list within 2 seconds without feed items or network
   retrieval from that feed URL.
4. Submit a second URL and confirm both entries remain visible.
5. Submit the same URL again and confirm it appears as another entry.
6. Submit an empty field and confirm the list is unchanged and perceivable feedback is shown.
7. Close and restart the application; confirm earlier subscriptions are no longer present.

The HTTP request and response shapes are defined in
[`contracts/subscriptions-api.yaml`](contracts/subscriptions-api.yaml); the entity rules are
documented in [`data-model.md`](data-model.md).
