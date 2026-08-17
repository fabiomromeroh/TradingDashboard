## Backend architecture

Use the existing Clean Architecture structure:

- API: HTTP endpoints, request/response models, authentication, middleware, and composition root.
- Application: use cases, commands, queries, handlers, DTOs, validators, and application interfaces.
- Core: domain entities, value objects, enums, domain rules, and domain interfaces.
- Infrastructure: EF Core, SQL Server, repositories, migrations, external integrations, and persistence implementations.

Dependency rules:
- API may depend on Application.
- Infrastructure may depend on Application and Core.
- Application may depend on Core.
- Core must not depend on API, Infrastructure, EF Core, ASP.NET Core, or external frameworks.
- Do not bypass the Application layer from API controllers.
- Keep controllers thin.
- Keep business rules out of controllers and EF Core configurations.

## Backend coding standards

- Use the existing .NET and C# version configured by the solution.
- Follow the existing naming, folder, namespace, and formatting conventions.
- Use nullable reference types correctly.
- Prefer async APIs for database and I/O operations.
- Propagate CancellationToken through API, application, and infrastructure layers where supported.
- Do not use async void.
- Use dependency injection instead of static service access.
- Prefer explicit types and simple control flow when it improves readability.
- Avoid premature generic abstractions.
- Avoid reflection and dynamic code unless already used by the project.
- Preserve existing error-handling and logging conventions.
- Never expose EF Core entities directly from API responses.
- Use DTOs for API contracts.
- Do not silently catch exceptions.
- Do not log secrets, tokens, credentials, or sensitive trading data.

## CQRS and application layer

- Use commands for state-changing operations.
- Use queries for read-only operations.
- Keep handlers focused on one use case.
- Reuse existing pipeline behaviors, validation, mapping, and result/error patterns.
- Do not create a handler for trivial logic if the project already has a simpler established pattern.
- Keep business rules in domain or application services, depending on whether the rule is domain-specific.
- Avoid unnecessary repository methods; add only methods required by a use case.
- For read-only queries, use efficient projections with Select instead of loading complete entities.
- Use AsNoTracking for read-only EF Core queries.
- Avoid N+1 queries and unnecessary Include calls.
- Apply filtering, sorting, and pagination in the database rather than in memory.

## EF Core and SQL Server

- Follow the existing DbContext, entity configuration, and repository patterns.
- Prefer explicit entity configurations over excessive attributes when that is the existing project convention.
- Do not modify the database schema without explaining the required migration.
- When changing persistence models, identify whether a migration is required.
- Use database constraints and indexes where they protect correctness or improve important queries.
- Avoid loading large datasets into memory.
- Consider transaction boundaries for imports and other multi-record writes.
- Do not change existing migrations; create a new migration instead.
- Never include generated migration files unless the requested change requires them.

## API

- Follow the existing REST, routing, status-code, validation, and error-response conventions.
- Keep endpoint methods small.
- Validate input at the API/application boundary.
- Do not return internal exception messages to clients.
- Do not introduce a new response-wrapper type if an existing API response pattern exists.
- Update OpenAPI or endpoint documentation only when the API contract changes.

## Authentication and authorization

- Preserve the existing JWT and Admin/User authorization approach.
- Do not introduce ASP.NET Core Identity, a role table, or a new authentication provider unless explicitly requested.
- Enforce authorization on the API, not only in React.
- Do not place authorization decisions solely in the frontend.

## Testing

- Add or update tests only for behavior affected by the change.
- Follow the existing xUnit, Moq, FluentAssertions, and WebApplicationFactory conventions.
- Prefer testing observable behavior over implementation details.
- For new business logic, include success, validation failure, and important edge cases.
- For import functionality, test duplicate records, malformed rows, missing values, decimal values, dates, fees, and partial failures where applicable.
- Do not create low-value tests for trivial property accessors or framework behavior.
- Run the smallest relevant test project first.
- Do not claim tests passed unless they were actually run.