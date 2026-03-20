# BloxBin

A pastebin-style REST API built with ASP.NET Core. 
Supports public and private pastes, with a one-time view key 
mechanic for private bins — keys are generated on demand, 
hashed before storage, and burned after a single use.

## Tech Stack
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core + SQLite
- JWT Authentication
- BCrypt password hashing
- Scalar API docs

## Running Locally

### Prerequisites
- .NET 10 SDK

### Setup
1. Clone the repo
2. Set the JWT secret via user-secrets:
```
   dotnet user-secrets init
   dotnet user-secrets set "Jwt:Key" "your-secret-key-min-32-chars"
```
3. Run migrations:
```
   dotnet ef database update
```
4. Start the server:
```
   dotnet run --launch-profile http
```
5. Open Scalar docs at `http://localhost:5299/scalar/v1`

## API Endpoints
All endpoints and request/response schemas are documented 
and testable via Scalar at `/scalar/v1`.