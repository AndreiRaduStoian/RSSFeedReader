# Data Model: RSS Subscription Management

## Subscription

Represents one URL submission displayed to the local user.

| Field | Type | Required | Rules |
|-------|------|----------|-------|
| `url` | String | Yes | Must be non-empty. The MVP does not parse, normalize, or check availability of the URL. |

Each submission is a separate list entry. Duplicate URL values are allowed. No identity,
timestamps, feed content, or persistence are required by this feature.

## Subscription list

An ordered collection of `Subscription` entries held for the lifetime of the running
application/backend process. It starts empty on startup; adding an entry appends one subscription
and leaves existing entries visible. The collection is discarded when the application stops.

## Validation and state transitions

- A missing or empty `url` is rejected without changing the collection.
- A non-empty `url` is accepted as supplied and appended.
- There is no URL syntax validation, feed retrieval, deduplication, update, or delete transition.
