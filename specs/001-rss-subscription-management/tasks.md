---

description: "Implementation tasks for RSS Subscription Management"
---

# Tasks: RSS Subscription Management

**Input**: Design documents from `specs/001-rss-subscription-management/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, and `contracts/`

**Tests**: Automated backend tests are included because the project constitution requires tests
for changed behavior.

**Organization**: Tasks are grouped by user story to enable independent implementation and
testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because the tasks change different files and have no incomplete
  task dependencies.
- **[Story]**: Identifies the user story served by the task.
- Every task includes the concrete repository path it changes.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the solution and scaffold the selected projects.

- [X] T001 Create `RSSFeedReader.sln` and the planned `backend/` and `frontend/` project directories.
- [X] T002 [P] Scaffold the ASP.NET Core Web API project at `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.
- [X] T003 [P] Scaffold the Blazor WebAssembly project at `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`.
- [X] T004 Create the xUnit test project at `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj`, add its reference to `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`, and add all three projects to `RSSFeedReader.sln`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Configure local frontend/backend communication before implementing the user journey.

- [X] T005 [P] Configure backend CORS for the stakeholder-documented frontend development origins in `backend/RSSFeedReader.Api/Program.cs` and `backend/RSSFeedReader.Api/Properties/launchSettings.json`.
- [X] T006 [P] Configure the frontend API base URL for the stakeholder-documented backend origin in `frontend/RSSFeedReader.UI/wwwroot/appsettings.json` and register the configured `HttpClient` in `frontend/RSSFeedReader.UI/Program.cs`.

**Checkpoint**: Solution projects are scaffolded and the UI can address the local API. User story
work can begin.

---

## Phase 3: User Story 1 - Add and View Subscriptions (Priority: P1) 🎯 MVP

**Goal**: Let the local user add feed URL text and immediately see every submitted entry,
including duplicates, without fetching or parsing feed content.

**Independent Test**: Start the application with an empty list, add a non-empty URL and verify it
appears, add a second and a duplicate URL and verify all entries remain visible, submit an empty
value and verify the list is unchanged with clear feedback, then restart and verify the list is
empty.

### Tests for User Story 1

- [X] T007 [US1] Add API integration tests using `WebApplicationFactory<Program>` in `backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs` for initial empty GET, POST then GET, duplicate submissions, and missing or empty URL rejection without changing the list; use a non-empty test URL and assert it is returned as submitted without requiring feed access.

### Implementation for User Story 1

- [X] T008 [P] [US1] Create the `Subscription` model in `backend/RSSFeedReader.Api/Subscriptions/Subscription.cs` with a required `url` string; preserve the data-model rule verbatim: "Must be non-empty. The MVP does not parse, normalize, or check availability of the URL."
- [X] T009 [US1] Implement the session-lifetime ordered in-memory subscription collection in `backend/RSSFeedReader.Api/Subscriptions/SubscriptionStore.cs`; append each accepted submission, allow duplicate URL values, and reject missing or empty URLs without mutating the list.
- [X] T010 [US1] Implement `GET /api/subscriptions` and `POST /api/subscriptions` in `backend/RSSFeedReader.Api/Subscriptions/SubscriptionsEndpoints.cs` to match `specs/001-rss-subscription-management/contracts/subscriptions-api.yaml`; return the ordered list, return the created subscription with 200 for non-empty input, and return 400 for missing or empty URL input without fetching or parsing feeds.
- [X] T011 [US1] Register the subscription store and map the subscriptions endpoints in `backend/RSSFeedReader.Api/Program.cs` so API state is shared for the running backend process and CORS remains enabled; expose the entry point to `WebApplicationFactory<Program>` tests.
- [X] T012 [P] [US1] Implement the frontend subscription API client in `frontend/RSSFeedReader.UI/Services/SubscriptionsApiClient.cs` to retrieve the list and submit new URL values using the contract's JSON shapes.
- [X] T013 [US1] Implement the subscription page in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` with a programmatically labeled URL field, add action, visible URL-text list, and perceivable empty-input feedback; refresh the list after a successful add and render submitted URLs as text rather than active links.
- [X] T014 [US1] Build and run the API integration tests for `RSSFeedReader.sln`, then exercise the add, list, duplicate, empty-input, no-feed-fetch, and restart scenarios from `specs/001-rss-subscription-management/quickstart.md`; resolve any failures in the affected implementation files.

**Checkpoint**: User Story 1 is independently demonstrable and satisfies the MVP's subscription
management scope.

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Confirm that setup guidance and implementation checks remain usable across the
documented local development workflow.

- [ ] T015 Update `specs/001-rss-subscription-management/quickstart.md` if project scaffolding selects different run commands, ports, or prerequisites, and ensure the documented validation still covers every Success Criterion in `specs/001-rss-subscription-management/spec.md`.
- [ ] T016 Run the completed `specs/001-rss-subscription-management/quickstart.md` from a clean application start and verify the frontend loads without browser connection errors and the API permits the configured frontend origin.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: T001 runs first. T002 and T003 depend on T001 and can run in parallel.
  T004 depends on T002 and T003 so it can add the project references to the solution.
- **Foundational (Phase 2)**: T005 and T006 depend on the backend and frontend projects from
  Phase 1; they configure the browser-to-API connection before the story implementation and can
  run in parallel.
- **User Story 1 (Phase 3)**: Depends on Phase 2. T007 establishes expected API behavior before
  the story implementation. T008 precedes T009; T009 precedes endpoint mapping in T010 and T011.
  Frontend integration in T012 and T013 uses the API contract implemented in T010-T011. T014
  validates the complete story after implementation.
- **Polish (Phase 4)**: Depends on the user story being implemented; T016 uses the final
  quickstart instructions.

### User Story Dependencies

- **User Story 1 (P1)**: Can begin after setup and foundational configuration. It has no
  dependencies on other user stories and is the complete MVP.

### Within User Story 1

- Automated API integration tests are defined before implementation.
- The subscription model precedes its in-memory collection.
- The collection precedes endpoint behavior.
- The frontend API client precedes the subscription page.
- Full build, automated, and manual validation follows implementation.

### Parallel Opportunities

- **Setup**: T002 (backend scaffold) and T003 (frontend scaffold) can run in parallel after T001;
  T004 adds both projects to the solution after they exist.
- **Story work**: T008 and T012 can run in parallel after T007 because they change distinct
  backend and frontend files; T012 follows the contract, and T013 follows T012. Run T014 only after
  all story tasks are complete.
- The single user story is independently deliverable; there are no separate stories to parallelize.

## Parallel Example: User Story 1

```text
After T007 and Phase 2:
Task: T008 Create the subscription model in backend/RSSFeedReader.Api/Subscriptions/Subscription.cs
Task: T012 Implement the frontend API client in frontend/RSSFeedReader.UI/Services/SubscriptionsApiClient.cs

After T012:
Task: T013 Implement the subscription page in frontend/RSSFeedReader.UI/Pages/Subscriptions.razor
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete solution and project scaffolding.
2. Configure local backend/frontend communication.
3. Implement and validate User Story 1 only.
4. Run the API tests, build the solution, and complete the quickstart scenarios.
5. Stop when the subscription add-and-list MVP is demonstrable; do not implement deferred feed
   fetching, parsing, item display, persistence, removal, or background polling.

### Incremental Delivery

The specification contains one user story, so completing User Story 1 delivers the whole MVP.
Any Extended-MVP or post-MVP capabilities require a separate approved specification.

## Notes

- `[P]` tasks use distinct files and have no dependencies on unfinished tasks.
- `[US1]` maps to the P1 story in `specs/001-rss-subscription-management/spec.md`.
- Keep the service and endpoint implementation minimal; do not add persistence or feed-network
  behavior.
