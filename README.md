# POS C# Application

This repository contains a point-of-sale system built with ASP.NET Core, Blazor
Razor Components, SQL Server, Entity Framework Core, and Dapper. The solution is
split into a server API, a server-rendered web app, shared contracts, database
models, and shared infrastructure helpers.

## Solution Architecture

```text
Pos.sln
|-- Pos.App                  Blazor web app and POS user interface
|-- Pos.BackendApi           ASP.NET Core Web API and feature modules
|-- Pos.BackendApi.Models    Request, response, and view models
|-- Pos.BackendApi.DbService EF Core database context and table models
|-- Pos.BackendApi.Shared    Shared services such as Dapper and JWT helpers
|-- database                 SQL Server installer and migrations
|-- tests                    Repository verification scripts and test projects
```

### Runtime Flow

```text
Browser
  |
  v
Pos.App
  - Static server-rendered Razor pages
  - Cookie authentication
  - TempData flash messages
  - BackendApiClient for API calls
  |
  | HTTPS + Bearer access token
  v
Pos.BackendApi
  - JWT authentication and authorization
  - Feature controllers under /api/v1/*
  - BL_* business logic classes
  - DL_* data access classes and SaleDraftService
  |
  v
SQL Server
  - AppDbContext EF Core entities
  - Stored procedures and reports
  - Dapper support for query-oriented paths
```

## Project Responsibilities

### Pos.App

`Pos.App` is the user-facing web application. It uses static server-rendered
Razor components rather than interactive Blazor Server. Forms use normal server
postbacks with antiforgery protection and `[SupplyParameterFromForm]` binding.

The app stores the signed-in session in an encrypted ASP.NET Core cookie named
`Pos.Auth`. Access and refresh tokens are stored in the cookie authentication
properties, not in browser local storage. `BackendApiClient` attaches the access
token to API requests and refreshes it through `api/v1/auth/refresh` when it is
near expiry.

Common UI areas:

- `Components/Pages` contains dashboard, resource CRUD, reports, invoices, sale
  drafts, checkout, receipt, and login pages.
- `Components/Layout` contains the authenticated main layout and login layout.
- `Services` contains the API client, auth session handling, flash messages, and
  token refresh middleware.
- `Styles/app.css` is compiled into `wwwroot/css/site.generated.css` during
  build.

### Pos.BackendApi

`Pos.BackendApi` exposes the HTTP API. `Program.cs` configures controllers,
Swagger, CORS, JWT bearer authentication, authorization, EF Core, Dapper, and
feature services. All mapped controllers require authorization by default via
`app.MapControllers().RequireAuthorization()`. Authentication endpoints opt out
with `[AllowAnonymous]`.

Most features follow this shape:

```text
Features/<Feature>/<Feature>Controller.cs
Features/<Feature>/BL_<Feature>.cs
Features/<Feature>/DL_<Feature>.cs
```

The controller handles HTTP concerns, the `BL_*` class handles validation and
business flow, and the `DL_*` class handles EF Core or database access. Feature
routes are versioned under `api/v1`, for example:

- `api/v1/auth/login`, `api/v1/auth/refresh`, `api/v1/auth/revoke`
- `api/v1/products`
- `api/v1/product-categories`
- `api/v1/customers`
- `api/v1/staffs`
- `api/v1/shops`
- `api/v1/taxes`
- `api/v1/sale-drafts`
- `api/v1/sale-invoices`
- `api/v1/report`
- `api/v1/dashboard`

### Pos.BackendApi.Models

`Pos.BackendApi.Models` contains DTOs and response models shared by the API and
the web app. Keep request and response shape changes here when a UI page and API
endpoint need the same contract.

### Pos.BackendApi.DbService

`Pos.BackendApi.DbService` contains `AppDbContext` and EF Core table models such
as customers, products, product categories, invoices, sale drafts, refresh
tokens, staff, shops, tax, states, and townships.

### Pos.BackendApi.Shared

`Pos.BackendApi.Shared` contains infrastructure used by the API, including:

- `DapperService` for direct SQL access.
- `JwtTokenGenerate` for access-token generation.
- Shared extension/helper code used across backend features.

## Authentication Workflow

1. The user submits the login form in `Pos.App`.
2. `AuthSessionService` sends credentials to `api/v1/auth/login`.
3. `Pos.BackendApi` validates credentials and returns an access token, refresh
   token, and expiry values.
4. `Pos.App` signs in with cookie authentication and stores token values in the
   server authentication ticket.
5. Page handlers use `BackendApiClient` to call protected API endpoints.
6. If the access token is close to expiry, `BackendApiClient` calls
   `api/v1/auth/refresh`, updates the cookie ticket, and retries future calls
   with the new token.
7. Logout or token revocation clears the app session and can revoke the refresh
   token on the backend.

## Sales Workflow

1. Staff sign in through the web app.
2. Setup resources such as products, product categories, customers, staff, shops,
   taxes, states, and townships are managed through shared resource pages.
3. Sale drafts are created under `/sale-drafts`.
4. Items are added to a draft and quantities can be edited or removed.
5. Checkout converts a draft into a sale invoice.
6. Receipts, invoice lists, dashboard metrics, and reports read invoice and
   aggregate sales data from the backend.

## Database Workflow

The database scripts live under `database/`.

For a fresh local database, run the full installer from the repository root:

```powershell
sqlcmd -S . -E -b -i .\database\Pos.Full.sql
```

For an existing database, run only the idempotent migration:

```powershell
sqlcmd -S . -E -b -d Pos -i .\database\migrations\20260827_add_auth_and_sale_drafts.sql
```

Replace `.` with the SQL Server instance name. Use `-U <user> -P <password>`
instead of `-E` when SQL authentication is required.

After creating or selecting a database, make sure
`Pos.BackendApi/appsettings.json` or `appsettings.Development.json` has a
matching `ConnectionStrings:DbConnection` value.

More database setup notes are in `database/README.md`.

## Local Development

### Prerequisites

- .NET SDK `10.0.400` or later feature-compatible SDK, as configured in
  `global.json`.
- SQL Server.
- `sqlcmd` for database setup.
- Node.js and npm for the `Pos.App` CSS build.

### Build

```powershell
dotnet restore
dotnet build Pos.sln
```

Building `Pos.App` automatically runs:

```powershell
npm ci
npm run build:css
```

The CSS build compiles `Pos.App/Styles/app.css` to
`Pos.App/wwwroot/css/site.generated.css` and copies Font Awesome assets.

### Run The API

```powershell
dotnet run --project .\Pos.BackendApi --launch-profile https
```

Default development URLs:

- `https://localhost:7164`
- `http://localhost:5192`

Swagger is available at `/swagger` in development.

### Run The Web App

In a second terminal:

```powershell
dotnet run --project .\Pos.App --launch-profile https
```

Default development URLs:

- `https://localhost:7288`
- `http://localhost:5048`

`Pos.App/appsettings.json` points `BackendApi:BaseUrl` at
`https://localhost:7164/`, so start the API first or update that setting for
your local ports.

## Development Workflow

When adding or changing a feature:

1. Update or add database tables, stored procedures, or migrations under
   `database/` when persistence changes.
2. Update EF Core models in `Pos.BackendApi.DbService` if table shape changes.
3. Add or update request and response contracts in `Pos.BackendApi.Models`.
4. Implement backend route, validation, and persistence in the feature folder:
   controller, `BL_*`, and `DL_*`.
5. Register new services in `Pos.BackendApi/ModularService.cs`.
6. Add or update web pages and forms in `Pos.App/Components/Pages`.
7. Use `BackendApiClient` for API calls from the web app so token refresh stays
   consistent.
8. Run build and verification commands before handing off the change.

Recommended check:

```powershell
dotnet build Pos.sln
```

The repository also includes `tests/verify-architecture.ps1` for architecture
guardrails. It checks that the solution remains on `net10.0`, uses static SSR
patterns, avoids browser token storage, keeps removed UI libraries out of the
codebase, and preserves the TempData/form-binding contracts used by the app.
Update the script when project count or architecture boundaries change.

## EF Core Database-First Commands

If the SQL Server schema is changed outside EF Core and the table models need to
be regenerated, use a scaffold command like:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=.;Database=Pos;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext -f --project .\Pos.BackendApi.DbService
```

Adjust the connection string to match the local SQL Server instance and database.
