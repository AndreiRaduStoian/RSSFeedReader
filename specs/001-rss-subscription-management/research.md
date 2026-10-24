# Research: RSS Subscription Management

## Decision: Preserve the stakeholder-selected application stack

**Decision**: Use an ASP.NET Core Web API backend and a Blazor WebAssembly frontend, with C# as
the application language.

**Rationale**: This is the stack explicitly selected for the RSS Feed Reader in
`StakeholderDocuments/TechStack.md`. No source projects currently exist from which to infer a
target framework version, so the initial project scaffold will select a supported .NET version
consistent with the repository's development environment.

**Alternatives considered**: Replacing the specified stack with another framework or a single
standalone UI. Neither is needed for this MVP or consistent with the stakeholder decision.

## Decision: Keep subscription data in process memory

**Decision**: Store submitted subscription URLs in an in-memory collection for the lifetime of
the running backend. Do not add persistence.

**Rationale**: The project goals explicitly choose memory-only data for the MVP and defer
persistence. A backend-owned collection remains accessible to the UI through the documented API
boundary.

**Alternatives considered**: Browser storage and a database would retain data after restart,
contradicting the MVP's session-only requirement.

## Decision: Expose a minimal collection API for the frontend

**Decision**: Provide `GET /api/subscriptions` to return the current collection and
`POST /api/subscriptions` to add a submitted URL. Use JSON objects with a `url` string property.
Return `200 OK` with the created subscription on successful submission and `400 Bad Request` for
an empty URL. Treat repeated submissions as separate entries.

**Rationale**: The stakeholder documentation specifies that the backend exposes add and list
operations and that the frontend calls the backend. This small contract supports precisely those
actions and the specification's duplicate and empty-input behavior without introducing deletion,
item, or persistence endpoints. A `200 OK` response avoids inventing a resource identifier for a
subscription whose only specified data is its URL.

**Alternatives considered**: Returning a bare string would provide no structured contract;
inventing identifiers and item routes would add data and behavior outside the MVP.

## Decision: Do not perform feed network or parsing operations

**Decision**: Treat the submitted URL as text. Do not make an outbound request, parse RSS/Atom
content, check URL reachability, or validate feed syntax.

**Rationale**: These capabilities are expressly excluded by the feature specification and
constitution. Not dereferencing user URLs also avoids introducing SSRF and network-resource risks
into this feature.

**Alternatives considered**: Fetching a feed during add or validating its content belongs to the
separately scoped Extended-MVP, not this subscription-management increment.

## Decision: Validate empty input but preserve all other submitted text

**Decision**: Reject a missing or empty URL and provide visible feedback. Do not normalize,
deduplicate, or otherwise validate non-empty submitted URL text.

**Rationale**: The specification explicitly requires empty-input feedback and states that valid
URLs are assumed. It also requires each repeated submission to create a separate entry.

**Alternatives considered**: URL parsing, canonicalization, trimming, and deduplication would add
validation or alter accepted values not required by the MVP. The UI and API must agree on the
empty-input rule.

## Decision: Keep displayed URLs as text

**Decision**: Render subscription URLs as text in the list rather than as active links.

**Rationale**: The MVP only requires users to see submitted URLs; opening them is not required.
Rendering as text avoids introducing navigation behavior or interpreting user input as markup.

**Alternatives considered**: Clickable links are unnecessary for the stated subscription
management goal and could expose users to an unexpected navigation action.
