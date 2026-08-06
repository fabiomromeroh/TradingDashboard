# Trading Dashboard - Architecture & Project Documentation

## Project Overview

**Trading Dashboard** is a comprehensive full-stack web application for managing and analyzing trading activity across multiple brokers. It provides users with centralized dashboard for tracking trades, managing broker accounts, syncing with broker APIs, and importing trading data.

### Key Features
- **Multi-Broker Support**: Integration with IBKR (Interactive Brokers) and extensible for other brokers
- **Trade Management**: Create, track, and analyze trades with execution details
- **Broker Synchronization**: Sync trading data directly from broker APIs
- **Data Import**: Upload and parse trading data from various broker file formats
- **User Management**: User registration, authentication, and role-based access control
- **Dashboard Analytics**: Real-time summary of trading performance and portfolio status
- **Account Management**: Manage multiple trading accounts with broker credentials

---

## Architecture Overview

### Architecture Pattern: **Clean Architecture / Layered Architecture**

The solution follows Clean Architecture principles with clear separation of concerns across multiple layers:

```
┌─────────────────────────────────────────────────────────────┐
│                        API Layer                             │
│           (TradingDashboard.API)                             │
│  - Controllers, Middleware, Dependency Injection Setup       │
└─────────────────────────────────────────────────────────────┘
							│
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                         │
│         (TradingDashboard.Application)                       │
│  - MediatR Commands/Queries, Business Logic, DTOs           │
│  - Validation, Mapping, Behavioral Pipelines                │
└─────────────────────────────────────────────────────────────┘
							│
┌─────────────────────────────────────────────────────────────┐
│                     Domain Layer                             │
│           (TradingDashboard.Domain)                          │
│  - Entities, Value Objects, Domain Events, Enums            │
│  - No external dependencies                                  │
└─────────────────────────────────────────────────────────────┘
							│
┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                        │
│         (TradingDashboard.Infrastructure)                    │
│  - Database (EF Core), Repositories, External Services      │
│  - Broker Integrations (IBKR), Azure Services               │
└─────────────────────────────────────────────────────────────┘
```

---

## Project Structure & File Organization

### Directory Layout

```
TradingDashboard/
├── src/
│   ├── TradingDashboard.API/                 # Presentation Layer (ASP.NET Core Web API)
│   │   ├── Controllers/                      # HTTP endpoint controllers
│   │   │   ├── AccountsController.cs
│   │   │   ├── BrokersController.cs
│   │   │   ├── DashboardController.cs
│   │   │   ├── ImportsController.cs
│   │   │   ├── TradesController.cs
│   │   │   └── UsersController.cs
│   │   ├── Extensions/                       # Utility extensions
│   │   │   ├── ClaimsPrincipalExtensions.cs
│   │   │   ├── DateTimeExtensions.cs
│   │   │   └── ResultExtensions.cs
│   │   ├── Middleware/                       # HTTP middleware
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   └── RequestLoggingMiddleware.cs
│   │   ├── wwwroot/                          # Static files (frontend assets)
│   │   ├── Program.cs                        # Startup configuration
│   │   ├── appsettings.json                  # Configuration (base)
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Production.json
│   │   └── TradingDashboard.API.csproj
│   │
│   ├── TradingDashboard.Application/         # Business Logic Layer
│   │   ├── Abstractions/
│   │   │   ├── IUnitOfWork.cs                # Data access abstraction
│   │   │   ├── Models/
│   │   │   │   └── IResult.cs                # Operation result pattern
│   │   │   ├── Repositories/                 # Repository interfaces
│   │   │   │   ├── IAccountRepository.cs
│   │   │   │   ├── IBrokerRepository.cs
│   │   │   │   ├── ITradeRepository.cs
│   │   │   │   ├── IExecutionRepository.cs
│   │   │   │   ├── IUserRepository.cs
│   │   │   │   ├── IRefreshTokenRepository.cs
│   │   │   │   ├── IBrokerAccountCredentialRepository.cs
│   │   │   │   └── IImportSessionRepository.cs
│   │   │   └── Services/
│   │   │       ├── BrokerSync/              # Broker synchronization contracts
│   │   │       │   ├── IBrokerSyncService.cs
│   │   │       │   ├── IBrokerSyncFactory.cs
│   │   │       │   ├── IBrokerAccountCredentialService.cs
│   │   │       │   ├── Ibkr/
│   │   │       │   │   ├── IIbkrFlexApiClient.cs
│   │   │       │   │   ├── IIbkrFlexReportParser.cs
│   │   │       │   │   └── IbkrFlexCredentials.cs
│   │   │       │   └── Models/
│   │   │       │       ├── BrokerCredentials.cs
│   │   │       │       ├── BrokerSyncResult.cs
│   │   │       │       └── ParsedExecution.cs
│   │   │       ├── FileUpload/              # File parsing contracts
│   │   │       │   ├── IBrokerParser.cs
│   │   │       │   ├── IBrokerParserFactory.cs
│   │   │       │   ├── IImportService.cs
│   │   │       │   └── Models/
│   │   │       │       ├── ParsedImportResult.cs
│   │   │       │       └── RawExecutionRow.cs
│   │   │       ├── Dashboard/
│   │   │       │   └── IDashboardQueryService.cs
│   │   │       └── IJwtTokenService.cs      # Authentication contract
│   │   ├── Common/
│   │   │   ├── Behaviors/                   # MediatR pipeline behaviors
│   │   │   │   ├── LoggingBehavior.cs
│   │   │   │   └── ValidationBehavior.cs
│   │   │   ├── Exceptions/
│   │   │   │   ├── NotFoundException.cs
│   │   │   │   ├── ValidationException.cs
│   │   │   │   └── StringExtensions.cs
│   │   │   ├── Extensions/
│   │   │   │   ├── DateTimeExtensions.cs
│   │   │   │   └── EnumExtensions.cs
│   │   │   ├── Helpers/
│   │   │   │   └── TokenHasher.cs
│   │   │   ├── Mappings/
│   │   │   │   └── MappingProfile.cs        # AutoMapper configuration
│   │   │   └── Models/
│   │   │       ├── QueryFilter.cs
│   │   │       └── Result.cs                # Generic result wrapper
│   │   ├── Features/                        # Feature-based organization
│   │   │   ├── Accounts/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateAccount/
│   │   │   │   │   ├── UpdateAccount/
│   │   │   │   │   ├── DeleteAccount/
│   │   │   │   │   └── SetBrokerCredentials/
│   │   │   │   ├── Queries/
│   │   │   │   │   ├── GetAccountById/
│   │   │   │   │   └── GetAccountsByUser/
│   │   │   │   └── Dtos/
│   │   │   │       └── AccountDto.cs
│   │   │   ├── Trades/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateTrade/
│   │   │   │   │   └── DeleteTrade/
│   │   │   │   ├── Queries/
│   │   │   │   │   ├── GetTradeById/
│   │   │   │   │   ├── GetTradesByAccountId/
│   │   │   │   │   └── GetExecutionsByTradeId/
│   │   │   │   └── Dtos/
│   │   │   │       ├── TradeDto.cs
│   │   │   │       └── ExecutionDto.cs
│   │   │   ├── Users/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── RegisterUser/
│   │   │   │   │   ├── LoginUser/
│   │   │   │   │   ├── LogoutUser/
│   │   │   │   │   ├── RefreshTokenUser/
│   │   │   │   │   ├── UpdateUser/
│   │   │   │   │   └── DeleteUser/
│   │   │   │   ├── Queries/
│   │   │   │   │   ├── GetUserById/
│   │   │   │   │   └── GetUsers/
│   │   │   │   └── Dtos/
│   │   │   │       ├── UserDto.cs
│   │   │   │       ├── LoginResponseDto.cs
│   │   │   │       └── RefreshTokenDto.cs
│   │   │   ├── ImportSessions/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── UploadImport/
│   │   │   │   │   ├── ConfirmImport/
│   │   │   │   │   ├── DeleteImport/
│   │   │   │   │   └── SyncBrokerImport/
│   │   │   │   ├── Queries/
│   │   │   │   │   ├── GetImportSessionById/
│   │   │   │   │   ├── GetImportSessionsByAccount/
│   │   │   │   │   └── GetBrokers/
│   │   │   │   └── Dtos/
│   │   │   │       ├── ImportSessionDto.cs
│   │   │   │       ├── ImportPreviewDto.cs
│   │   │   │       ├── PreviewRowDto.cs
│   │   │   │       ├── BrokerDto.cs
│   │   │   │       └── SyncBrokerDto.cs
│   │   │   └── Dashboard/
│   │   │       ├── Queries/
│   │   │       │   └── GetDashboardSummaryQuery/
│   │   │       └── Dtos/
│   │   │           └── DashboardSummaryDto.cs
│   │   ├── DependencyInjection.cs            # Application services registration
│   │   └── TradingDashboard.Application.csproj
│   │
│   ├── TradingDashboard.Domain/             # Core Domain Layer
│   │   ├── Common/
│   │   │   └── BaseEntity.cs                # Base class for all entities
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Account.cs
│   │   │   ├── Trade.cs
│   │   │   ├── Execution.cs
│   │   │   ├── Broker.cs
│   │   │   ├── BrokerAccountCredential.cs
│   │   │   ├── ImportSession.cs
│   │   │   └── RefreshToken.cs
│   │   ├── Enums/
│   │   │   ├── UserRole.cs
│   │   │   ├── TradeStatus.cs
│   │   │   ├── TradeDirection.cs
│   │   │   ├── Side.cs
│   │   │   ├── OrderType.cs
│   │   │   ├── InstrumentType.cs
│   │   │   ├── CurrencyType.cs
│   │   │   ├── ImportSessionStatus.cs
│   │   │   └── ImportSourceType.cs
│   │   ├── ValueObjects/
│   │   │   ├── Money.cs
│   │   │   └── Symbol.cs
│   │   ├── Events/
│   │   │   └── TradeClosed.cs              # Domain event
│   │   └── TradingDashboard.Domain.csproj
│   │
│   └── TradingDashboard.Infrastructure/     # Technical Implementation Layer
│       ├── Persistence/
│       │   ├── AppDbContext.cs              # EF Core DbContext
│       │   ├── Configurations/              # Entity configurations (Fluent API)
│       │   │   ├── UserConfiguration.cs
│       │   │   ├── AccountConfiguration.cs
│       │   │   ├── TradeConfiguration.cs
│       │   │   ├── ExecutionConfiguration.cs
│       │   │   ├── BrokerConfiguration.cs
│       │   │   ├── ImportSessionConfiguration.cs
│       │   │   ├── BrokerAccountCredentialConfiguration.cs
│       │   │   └── RefreshTokenConfiguration.cs
│       │   ├── Repositories/                # Repository implementations
│       │   │   ├── UserRepository.cs
│       │   │   ├── AccountRepository.cs
│       │   │   ├── TradeRepository.cs
│       │   │   ├── ExecutionRepository.cs
│       │   │   ├── BrokerRepository.cs
│       │   │   ├── ImportSessionRepository.cs
│       │   │   ├── BrokerAccountCredentialRepository.cs
│       │   │   └── RefreshTokenRepository.cs
│       │   ├── Migrations/                  # EF Core migrations
│       │   │   ├── 20260510203814_InitialCreate.cs
│       │   │   ├── 20260519104328_ImportSessionAccountUserExecutionBroker.cs
│       │   │   └── ... (other migrations)
│       │   ├── UnitOfWork.cs                # Unit of Work implementation
│       │   └── Seed/                        # Database seeding
│       │       ├── SeedAdmin.cs
│       │       └── InfrastructureSeedExtensions.cs
│       ├── Services/
│       │   ├── BrokerSync/
│       │   │   ├── BrokerSyncFactory.cs     # Factory for broker services
│       │   │   ├── BrokerAccountCredentialService.cs
│       │   │   └── Ibkr/                    # Interactive Brokers integration
│       │   │       ├── IbkrSyncService.cs
│       │   │       ├── IbkrFlexApiClient.cs
│       │   │       ├── IbkrFlexReportParser.cs
│       │   │       ├── IbkrFlexOptions.cs
│       │   │       ├── IbkrFlexException.cs
│       │   │       └── IbkrFlexParseException.cs
│       │   ├── FileUpload/
│       │   │   ├── BrokerParserFactory.cs   # Factory for file parsers
│       │   │   ├── ImportSessionService.cs
│       │   │   └── Ibkr/
│       │   │       ├── IbkrCsvParser.cs
│       │   │       ├── IbkrRowMap.cs
│       │   │       └── IbkrRawRecord.cs
│       │   ├── Dashboard/
│       │   │   └── DashboardQueryService.cs
│       │   └── Identity/
│       │       ├── JwtTokenService.cs       # JWT token generation/validation
│       │       └── JwtSettingsOptions.cs
│       ├── Azure/
│       │   ├── BlobStorageService.cs        # Azure Blob Storage
│       │   ├── KeyVaultService.cs           # Azure Key Vault
│       │   ├── ServiceBusPublisher.cs       # Azure Service Bus
│       │   └── KeyVaultSettings.cs
│       ├── DependencyInjection.cs           # Infrastructure services registration
│       └── TradingDashboard.Infrastructure.csproj
│
├── tests/
│   ├── TradingDashboard.UnitTests/          # Unit tests
│   │   └── TradingDashboard.UnitTests.csproj
│   │
│   └── TradingDashboard.IntegrationTests/   # Integration tests
│       └── TradingDashboard.IntegrationTests.csproj
│
└── TradingDashboard.slnx                     # Solution file (.NET 10)
```

---

## Naming Conventions

### Project Naming
- **Pattern**: `TradingDashboard.<Layer>`
- **Examples**: `TradingDashboard.API`, `TradingDashboard.Application`, `TradingDashboard.Domain`, `TradingDashboard.Infrastructure`

### Namespace Naming
- **Format**: `TradingDashboard.<LayerPrefix>.<FeatureFolder>.<SubCategory>`
- **Examples**:
  - `TradingDashboard.Application.Features.Accounts.Commands`
  - `TradingDashboard.Application.Features.Trades.Queries`
  - `TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr`

### File/Class Naming
- **Controllers**: `<Entity>Controller.cs` (e.g., `AccountsController.cs`, `TradesController.cs`)
- **Commands**: `<Action><Entity>Command.cs` (e.g., `CreateAccountCommand.cs`, `DeleteTradeCommand.cs`)
- **Queries**: `Get<Entity|Entities><Qualifier>Query.cs` (e.g., `GetAccountByIdQuery.cs`, `GetTradesByAccountIdQuery.cs`)
- **Handlers**: `<Command|Query>Handler.cs` or `<Action><Entity>CommandHandler.cs`
  - Example: `CreateAccountCommandHandler.cs`, `GetAccountByIdQueryHandler.cs`
- **Validators**: `<Command|Query>Validator.cs` or `<Action><Entity>CommandValidator.cs`
  - Example: `CreateAccountCommandValidator.cs`, `LoginUserCommandValidator.cs`
- **DTOs**: `<Entity>Dto.cs` (e.g., `AccountDto.cs`, `TradeDto.cs`, `UserDto.cs`)
- **Repositories**: `<Entity>Repository.cs` (e.g., `AccountRepository.cs`, `TradeRepository.cs`)
- **Services**: `<ServiceName>Service.cs` (e.g., `JwtTokenService.cs`, `ImportSessionService.cs`)
- **Middleware**: `<Purpose>Middleware.cs` (e.g., `ExceptionHandlingMiddleware.cs`, `RequestLoggingMiddleware.cs`)
- **Extensions**: `<Type>Extensions.cs` (e.g., `ClaimsPrincipalExtensions.cs`, `ResultExtensions.cs`)

### Folder/Feature Organization
- **Features**: Organized by domain entity (Accounts, Trades, Users, ImportSessions, Dashboard)
- **Sub-folders**: Commands, Queries, Dtos
- **Commands**: One folder per command with Handler and Validator
- **Queries**: One folder per query with Handler

### Database-Related
- **Model Configurations**: `<Entity>Configuration.cs`
- **Migrations**: Timestamp prefix + descriptive name (e.g., `20260510203814_InitialCreate.cs`)

### Enum Naming
- **Pattern**: PascalCase, descriptive terms
- **Examples**: `UserRole`, `TradeStatus`, `ImportSessionStatus`, `InstrumentType`, `CurrencyType`

---

## Technology Stack

### Core Framework
- **.NET 10** (Latest long-term support version)
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for data access
- **MediatR** - CQRS/Mediator pattern implementation

### Architecture Patterns
- **Clean Architecture / Layered Architecture**
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **Factory Pattern** - Broker and parser creation
- **CQRS Pattern** - Separation of reads (Queries) and writes (Commands)
- **Dependency Injection** - Loose coupling throughout

### Authentication & Security
- **JWT (JSON Web Tokens)** - Stateless authentication
- **Refresh Tokens** - Token renewal mechanism
- **HTTPS** - Secure communication
- **Azure Key Vault** - Credential management (production)

### Data & Persistence
- **SQL Server / PostgreSQL** - Relational database
- **Entity Framework Core** - Database ORM
- **Database Migrations** - Version-controlled schema changes

### Validation & Mapping
- **FluentValidation** - Input validation framework
- **AutoMapper** - DTO/Entity mapping

### External Integrations
- **IBKR (Interactive Brokers)**:
  - `IbkrFlexApiClient` - REST API client for data sync
  - `IbkrFlexReportParser` - XML report parsing
  - `IbkrCsvParser` - CSV file parsing
- **Azure Services**:
  - Azure Blob Storage - File/data storage
  - Azure Key Vault - Secrets management
  - Azure Service Bus - Message queuing

### Logging & Monitoring
- **Serilog** - Structured logging
- **Middleware**: Request logging, exception handling

### Development Tools
- **Visual Studio 2026** - IDE
- **Git** - Version control
- **GitHub Actions** - CI/CD pipeline

---

## Key Design Patterns

### 1. **CQRS Pattern** (Command Query Responsibility Segregation)
- **Commands**: Mutate state (Create, Update, Delete)
- **Queries**: Read-only operations
- **MediatR**: Mediates between controllers and handlers
- **Benefit**: Clear separation of concerns, better testability

### 2. **Repository Pattern**
- One repository per entity (e.g., `UserRepository`, `TradeRepository`)
- Abstractions defined in Application layer (`IUserRepository`)
- Implementations in Infrastructure layer
- **Benefit**: Data access logic centralized, easy to mock/test

### 3. **Unit of Work Pattern**
- `IUnitOfWork` coordinates multiple repositories
- Single `SaveChangesAsync()` for transaction management
- Located in Infrastructure (`UnitOfWork.cs`)
- **Benefit**: Consistent database transactions

### 4. **Dependency Injection**
- Configured in `Program.cs` (API layer)
- Extensions methods: `AddApplication()`, `AddInfrastructure()`
- Automatic service discovery and registration
- **Benefit**: Loose coupling, testability

### 5. **Factory Pattern**
- `BrokerSyncFactory` - Creates broker-specific sync services
- `BrokerParserFactory` - Creates broker-specific file parsers
- **Benefit**: Extensibility for new brokers without modifying existing code

### 6. **Result Pattern**
- Generic `Result<T>` and `Result` classes
- Contains success/failure status, data, and errors
- Controllers use `ResultExtensions` for HTTP conversion
- **Benefit**: Standardized response format, explicit error handling

### 7. **Pipeline Behavior Pattern (MediatR)**
- `ValidationBehavior` - Automatic validation before handler execution
- `LoggingBehavior` - Cross-cutting logging
- **Benefit**: Reduced boilerplate, consistent cross-cutting concerns

---

## Core Business Concepts

### Domain Entities

#### User
- Represents an application user
- Properties: Id, Email, PasswordHash, Role (Admin/User), CreatedAt
- Relationships: Multiple Accounts, Multiple RefreshTokens

#### Account
- Trading account belonging to a User
- Properties: Id, UserId, Name, Broker, AccountNumber, Balance, ImportSourceType, CreatedAt
- Relationships: One User, Multiple Trades, Multiple ImportSessions, Multiple BrokerAccountCredentials

#### Trade
- Represents a trading position
- Properties: Symbol, EntryPrice, ClosePrice, Quantity, Direction (Buy/Sell), Status (Open/Closed)
- Derived Calculations: PositionSize, TotalCommissions, AverageEntryPrice, NetReturn, PercentageReturn
- Relationships: One Account, Multiple Executions

#### Execution
- Individual trade execution/leg
- Properties: Symbol, Side (Buy/Sell), Quantity, Price, Commission, Timestamp, BrokerExecutionId
- Relationships: One Trade, One Account, One Broker

#### Broker
- External broker system (e.g., IBKR)
- Properties: Id, Name, ApiEndpoint, IsActive
- Pre-seeded: Interactive Brokers (IBKR), etc.

#### BrokerAccountCredential
- Stores encrypted credentials for broker API access
- Properties: AccountId, BrokerId, ApiKey, EncryptedSecret
- Relationships: One Account, One Broker

#### ImportSession
- Tracks file import process
- Properties: AccountId, BrokerId (optional), Status (Pending/Confirmed/Archived), CreatedAt
- Statuses: Pending → Confirmed → Archived
- Relationships: One Account, Zero or One Broker

#### RefreshToken
- Stores issued refresh tokens for session management
- Properties: UserId, Token, ExpiresAt
- Relationships: One User

---

## Feature Overview

### Authentication & User Management
- **Register**: Create new user account with email/password
- **Login**: Authenticate and receive JWT + Refresh Token
- **Refresh Token**: Extend session without re-authentication
- **Logout**: Invalidate refresh token
- **Update User**: Modify user profile
- **Admin**: Delete users

### Account Management
- **Create Account**: Add trading account for a user
- **Update Account**: Modify account details
- **Get Accounts**: Retrieve user's accounts
- **Set Broker Credentials**: Store encrypted API credentials for broker sync

### Trade Management
- **Create Trade**: Manually add a trade (or auto-created via import)
- **Get Trades**: Retrieve all trades for an account
- **Get Trade Details**: Execution history for a specific trade
- **Delete Trade**: Remove a trade
- **Derived Calculations**: Automatic profit/loss, average price, position size

### Import Management
- **Upload File**: Upload broker CSV/export files
- **Preview Imports**: Show parsed trades before confirmation
- **Confirm Import**: Finalize imported trades to account
- **Delete Import**: Discard unparsed import session
- **Supported Brokers**: IBKR (extensible)

### Broker Synchronization
- **Sync Broker Data**: Pull execution data directly from broker API (IBKR)
- **Broker Credentials**: Manage API keys/tokens per account
- **Parse Broker Reports**: Convert broker XML/exports to internal Trade/Execution format

### Dashboard Analytics
- **Summary View**: Aggregate statistics (total accounts, open trades, net P&L, etc.)
- **Real-time Updates**: Calculated metrics based on current holdings

---

## Dependency Flow & Layering Rules

### What Depends on What
```
API Layer
  ├─ depends on → Application Layer
  ├─ depends on → Infrastructure Layer (services)
  └─ NOT on Domain (directly injected)

Application Layer
  ├─ depends on → Domain Layer
  ├─ defines abstractions → Infrastructure (via Interfaces)
  ├─ implements → CQRS Handlers, Validators, Mappings
  └─ NOT on Infrastructure implementations (only interfaces)

Domain Layer
  ├─ NO external dependencies
  ├─ defines → Entities, ValueObjects, Enums, Events
  └─ agnostic to data storage/external services

Infrastructure Layer
  ├─ depends on → Application Layer (interfaces)
  ├─ implements → Repositories, Services, EF Core
  └─ integrates → 3rd party APIs (IBKR, Azure)
```

### Dependency Injection Registration
1. **API Layer** (`Program.cs`): Registers all services
   ```csharp
   services.AddApplication();        // Application services
   services.AddInfrastructure(config); // Infrastructure services
   ```

2. **Application Layer** (`DependencyInjection.cs`):
   ```csharp
   - MediatR registration
   - FluentValidation registration
   - AutoMapper registration
   - Pipeline behaviors
   ```

3. **Infrastructure Layer** (`DependencyInjection.cs`):
   ```csharp
   - DbContext
   - Repository implementations
   - External service implementations (JWT, IBKR, etc.)
   - Azure services
   ```

---

## Data Flow Examples

### 1. **Create Trade** (Command Flow)
```
Controller.CreateTrade(dto)
  ↓
Mediator.Send(CreateTradeCommand)
  ↓
ValidationBehavior → CreateTradeCommandValidator
  ↓
LoggingBehavior → log request
  ↓
CreateTradeCommandHandler
  ├─ Map Dto → Domain Model
  ├─ Fetch Account via AccountRepository
  ├─ Create Trade entity with validations
  ├─ Save via UnitOfWork.Trades.Add()
  ├─ UnitOfWork.SaveChangesAsync()
  └─ Return Result<TradeDto>
  ↓
Controller converts Result → HTTP Response
```

### 2. **Import Trading Data** (File Upload Flow)
```
Controller.UploadImport(file, accountId)
  ↓
Mediator.Send(UploadImportCommand)
  ↓
Handler.ImportSessionService
  ├─ BrokerParserFactory.GetParser(brokerType)
  ├─ Parse CSV → RawExecutionRow[]
  ├─ Validate rows
  ├─ Create ImportSession (Pending)
  └─ Store preview in session
  ↓
Frontend displays preview
  ↓
Controller.ConfirmImport(sessionId)
  ↓
Mediator.Send(ConfirmImportCommand)
  ↓
Handler
  ├─ Fetch ImportSession
  ├─ Convert RawExecutionRow → Trade + Execution entities
  ├─ Add to Account
  ├─ Update ImportSession status to Confirmed
  └─ SaveChangesAsync()
```

### 3. **Broker Synchronization** (API Sync Flow)
```
Controller.SyncBroker(accountId)
  ↓
Mediator.Send(SyncBrokerImportCommand)
  ↓
Handler.BrokerSyncFactory
  ├─ Get account + broker credentials
  ├─ Factory.GetSyncService(brokerType) → IbkrSyncService
  ├─ IbkrFlexApiClient.FetchData()
  ├─ IbkrFlexReportParser.Parse() → ParsedExecution[]
  ├─ Convert to Trade/Execution entities
  ├─ Merge with existing trades
  └─ SaveChangesAsync()
```

---

## Security Considerations

### Authentication
- **JWT Tokens**: Issued on login, validated on each request
- **Refresh Tokens**: Stored in DB, hashed for security
- **Token Expiry**: Configurable (typically 15 min access, 7 day refresh)

### Authorization
- **Role-Based**: User vs. Admin roles
- **Development Mode**: Open auth for easier testing
- **Production Mode**: Strict authorization policies

### Data Protection
- **Passwords**: Hashed with ASP.NET Identity (or custom hasher)
- **Broker Credentials**: Encrypted in database
- **Azure Key Vault**: Stores connection strings, API keys in production
- **HTTPS**: Enforced in production

### Database Security
- **User Isolation**: Users can only access their own data (enforced in repositories)
- **SQL Injection**: EF Core parameterized queries prevent injection

---

## Configuration Management

### Configuration Files (by Environment)
- **appsettings.json**: Base settings
- **appsettings.Development.json**: Development overrides (relaxed auth, local DB)
- **appsettings.Production.json**: Production overrides (secure auth, Azure services)

### Key Configuration Sections
- **ConnectionStrings**: Database connection
- **JwtSettings**: Issuer, Audience, SecretKey, ExpirationMinutes
- **Azure**: Key Vault URL, Storage Account, Service Bus
- **Logging**: Serilog configuration
- **CORS**: Allowed origins
- **IBKR**: API credentials, endpoints

---

## Testing Strategy

### Test Projects
1. **TradingDashboard.UnitTests** - Fast, isolated tests
   - Unit tests for commands, queries, services, validators
   - Mocked dependencies (repos, external services)

2. **TradingDashboard.IntegrationTests** - Database-level tests
   - Full pipeline tests with real EF Core context
   - Import and broker sync workflows

### Testing Approach
- **Unit Tests**: Commands, Queries, Validators, Mappings
- **Repository Tests**: Integration with EF Core
- **Service Tests**: JwtToken, BrokerSync, FileUpload services
- **Controller Tests** (optional): API endpoint behavior

---

## Extensibility & Future Enhancements

### Adding a New Broker
1. Create `I<BrokerName>SyncService` interface in Application.Abstractions
2. Implement `<BrokerName>SyncService` in Infrastructure.Services.BrokerSync
3. Implement `I<BrokerName>Parser` in Infrastructure.Services.FileUpload
4. Register in `BrokerSyncFactory` switch pattern
5. Seed broker in `SeedAdmin.cs`

### Adding a New Feature
1. Create `Features/<EntityName>` directory
2. Create Command/Query classes in respective subdirectories
3. Create Handlers and Validators
4. Create DTOs
5. Create repository interfaces (if needed) in Application.Abstractions.Repositories
6. Create repository implementations in Infrastructure.Persistence.Repositories
7. Add endpoints in controller
8. Create/update AutoMapper profile
9. Add unit/integration tests

---

## Summary

**Trading Dashboard** is a well-structured, enterprise-grade trading management application built on Clean Architecture principles. The layered structure ensures separation of concerns, testability, and maintainability. The CQRS pattern with MediatR provides a clear command/query separation, while factory patterns enable flexible broker integration. The project is positioned for growth with extensible patterns for adding new brokers and features.
