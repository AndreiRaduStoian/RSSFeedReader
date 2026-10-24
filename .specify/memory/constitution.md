<!--
Sync Impact Report
Version change: Unversioned template -> 1.0.0 (initial adoption)
Modified principles: None (the source document contained placeholders only)
Added principles:
- Security by Design
- Maintainable, Layered Implementation
- Behavior-Driven Quality
- MVP Scope and Incremental Delivery
- Simple, Accessible User Experience
Added sections: Project Constraints; Development Workflow and Quality Gates
Removed sections: None
Follow-up TODO: Confirm the original adoption date for RATIFICATION_DATE.
-->
# RSS Feed Reader Constitution

## Core Principles

### I. Security by Design
All user-provided URLs and feed content MUST be treated as untrusted input. The MVP MUST NOT
fetch or parse feed URLs. Any later feature that performs network requests MUST restrict requests
to HTTP and HTTPS, reject loopback, private, link-local, and metadata-service destinations, and
recheck destinations after redirects. Requests MUST use bounded timeouts and response sizes.
Secrets and sensitive user data MUST NOT be logged. These controls reduce the risk of server-side
request forgery, resource exhaustion, and accidental disclosure as network functionality is added.

### II. Maintainable, Layered Implementation
Changes MUST follow the existing ASP.NET Core Web API and Blazor WebAssembly architecture, keeping
UI presentation, API responsibilities, and application data handling distinct. Implementations
MUST use the simplest design that meets the approved scope; abstractions without a current need
MUST NOT be added. Names, public contracts, and error behavior MUST be clear and consistent with
nearby code. This keeps the proof of concept understandable and allows later capabilities to be
added without obscuring the MVP.

### III. Behavior-Driven Quality
Automated tests MUST cover changed behavior at the narrowest practical level. Subscription changes
MUST verify that a URL can be added and appears in the displayed list. New network or parsing
behavior MUST include tests for successful results and relevant failure or malformed-input cases.
Changes MUST build successfully before completion, and user-visible behavior MUST be checked
through the relevant UI or API path. Tests and checks make regressions visible without imposing
unnecessary process on this small application.

### IV. MVP Scope and Incremental Delivery
The MVP MUST support adding a subscription by URL and displaying the current subscription list.
MVP subscriptions MAY be held in memory, and the MVP MUST NOT fetch, parse, or validate feed URLs
or add persistence, deletion, or background polling. Features beyond this scope MUST be described
and approved as a distinct increment before implementation. This preserves the stated proof-of-
concept goal and prevents future possibilities from becoming accidental MVP requirements.

### V. Simple, Accessible User Experience
The subscription interface MUST make URL entry, adding a subscription, and viewing the resulting
list clear and usable with standard browser interaction. Controls MUST have programmatic labels,
and validation or operation feedback MUST be perceivable without relying on color alone. The UI
MUST remain functional rather than adding polish or interaction complexity that does not serve the
approved feature set.

## Project Constraints

The application targets a single local user and MUST remain suitable for development on Windows,
macOS, and Linux. The current stack is an ASP.NET Core Web API backend and a Blazor WebAssembly
frontend. MVP state is in-memory and is lost when the application stops. Feed fetching, parsing,
and item display belong to a later increment; persistence and other enhancements require separate
scope approval. Changes MUST update directly affected project documentation when documented
behavior or setup requirements change.

## Development Workflow and Quality Gates

Before implementation, plans and task breakdowns MUST be checked against this constitution and
the current approved feature scope. During implementation, developers MUST use the repository's
existing conventions and test/build tools. Before declaring a change complete, developers MUST
run the smallest relevant tests and build checks, and report any required check that could not be
run. Review MUST verify scope, security implications, test coverage, and consistency between the
API and UI wherever both are affected. A conflict MUST be resolved by changing the proposal or
formally amending this constitution; deviations MUST NOT be silently treated as compliant.

## Governance

This constitution governs project specifications, plans, implementation, and review. Amendments
MUST be proposed as changes to this file, explain their motivation and impact, and be reviewed
against the project goals and affected documentation before adoption.

Constitution versions use semantic versioning. A MAJOR increment denotes removal or incompatible
redefinition of a principle; a MINOR increment denotes a new principle or section or materially
expanded requirements; a PATCH increment denotes clarifications and non-semantic refinements.
Every amendment MUST update the version and last-amended date. The ratification date records the
original adoption date and MUST NOT change for later amendments.

Specifications, plans, and code changes MUST be reviewed for compliance with the applicable
principles. Reviewers MUST identify conflicts and require resolution before approval. Any
constitution amendment MUST include a compliance review of the affected project guidance so
related documents do not contradict the amended rules.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): confirm original adoption date | **Last Amended**: 2026-10-24
