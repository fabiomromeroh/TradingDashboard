# TradingDashboard

A full-stack **trading journal + analytics dashboard** built with **.NET 10** and **React (Vite + TypeScript)**.

The goal is to make it easy to **capture trades consistently** and then explore performance through **clean, queryable analytics** (e.g., by strategy, instrument, time window, and risk parameters).

## Tech stack

- **Backend:** ASP.NET Core Web API (**.NET 10**)
- **Architecture:** layered solution (Domain / Application / Infrastructure / API)
- **Application patterns:** **CQRS** + **MediatR**
- **Persistence:** EF Core
- **Frontend:** **React + TypeScript** with **Vite**
- **Local dev:** Docker Compose

## Solution structure

- **TradingDashboard.Domain** — Entities, value objects, enums, and domain events
- **TradingDashboard.Application** — CQRS commands/queries, MediatR behaviors, interfaces
- **TradingDashboard.Infrastructure** — EF Core, Identity, Azure services, external APIs
- **TradingDashboard.API** — ASP.NET Core Web API
- **TradingDashboard.Client** — Vite + React frontend

## Getting started

### Docker (recommended)

```bash
docker-compose up --build
```

This brings up the API + client (and any dependent services defined in `docker-compose.yml`).

### Run locally (without Docker)

**Backend**

```bash
dotnet restore

dotnet build

dotnet run --project src/TradingDashboard.API
```

**Frontend**

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

## Notes

- The frontend project also contains its own README under `src/TradingDashboard.Client/`.
- If you’d like, I can add a small "Preview" section once you have a screenshot/GIF and confirm the default ports.

## License

If you plan to share or reuse this project, consider adding a license (MIT/Apache-2.0).
