# TradingDashboard

A full-stack **trading journal + analytics dashboard** built with **.NET 10** and **React (Vite + TypeScript)**. It’s designed to help traders **log trades**, **analyze performance**, and **track risk/edge** with a clean domain model and scalable backend architecture.

> Recruiter note: This repo focuses on real-world engineering practices—clean architecture, domain-driven design concepts, CQRS, and a modern web UI.

## Highlights

- **Backend:** ASP.NET Core Web API on **.NET 10**
- **Architecture:** Clean Architecture layers (Domain / Application / Infrastructure / API)
- **Patterns:** **CQRS** + **MediatR** (commands/queries, pipeline behaviors)
- **Data:** EF Core (persistence layer)
- **Frontend:** **React + TypeScript** with **Vite**
- **Dev Experience:** Docker-first local setup

## What you can evaluate in this codebase

- **API design & layering:** clear separation of concerns and dependency direction
- **Domain modeling:** entities/value objects/enums and domain events
- **Application design:** commands/queries, validation, and cross-cutting behaviors
- **Infrastructure integration:** persistence, identity, and external services boundaries
- **Frontend structure:** component-driven UI with typed data flow
- **Containerized workflow:** consistent local environment via Docker Compose

## Repository structure

- **TradingDashboard.Domain** — Entities, value objects, enums, and domain events
- **TradingDashboard.Application** — CQRS commands/queries, MediatR behaviors, interfaces
- **TradingDashboard.Infrastructure** — EF Core, Identity, Azure services, external APIs
- **TradingDashboard.API** — ASP.NET Core Web API
- **TradingDashboard.Client** — Vite + React frontend

## Getting started (recommended)

### Run with Docker

```bash
docker-compose up --build
```

When the containers are up, you should have:
- API available on the API container’s exposed port
- React client available on the client container’s exposed port

> If you want, I can update this section with the exact URLs/ports once you confirm them (or I can infer them from `docker-compose.yml`).

## Getting started (without Docker)

### Backend (API)

```bash
dotnet restore

dotnet build

dotnet run --project src/TradingDashboard.API
```

### Frontend (Client)

```bash
cd src/TradingDashboard.Client
npm install
npm run dev
```

## Configuration

Environment-specific configuration is typically handled via:
- `appsettings.json` / `appsettings.Development.json`
- environment variables
- Docker Compose env configuration

If you run into missing settings (DB connection string, auth config, etc.), check the API project settings and the Compose file.

## Roadmap / Next improvements

Potential enhancements (good “conversation starters” for an interview):
- Add CI (build + test) with GitHub Actions
- Add API versioning + OpenAPI grouping
- Add frontend integration tests (Playwright) and backend tests
- Add observability (structured logging, tracing)
- Add seed/demo data for a quick product tour

## Contributing

PRs and issues are welcome. If you’re reviewing this repo and want a guided walkthrough, open an issue with the area you’d like to discuss (Domain modeling, CQRS flow, EF Core mappings, frontend state management, etc.).

## License

If you plan to make this public for portfolio/recruiting, consider adding a license (MIT/Apache-2.0) to clarify usage.
