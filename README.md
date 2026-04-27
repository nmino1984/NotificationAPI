# NotificationAPI

A microservices-ready notification service built with **Clean Architecture**, **.NET 10**, and **Docker**.

## Architecture

```
Domain          → Entities (User, Notification), Enums, Value Objects
Application     → Use Cases, DTOs, Interfaces, FluentValidation validators
Infrastructure  → EF Core + SQLite repositories
API             → ASP.NET Core REST controllers, Swagger
Tests           → xUnit + Moq unit tests
```

## Features

- Send notifications to users
- List user notifications
- Input validation with FluentValidation
- OpenAPI / Swagger documentation
- Unit tests (xUnit + Moq) — 14 tests
- CI/CD with GitHub Actions
- Docker containerized
- Railway-ready deployment

## Prerequisites

- .NET 10 SDK
- Docker (optional)

## Quick Start

### Local Development

```bash
# Restore and build
dotnet restore
dotnet build

# Run API
cd API
dotnet run
# Swagger UI: http://localhost:5000/swagger
```

### Run Tests

```bash
dotnet test
```

### Docker

```bash
docker build -t notificationapi:latest .
docker run -p 8080:80 notificationapi:latest
# API available at http://localhost:8080
```

## API Endpoints

### Users
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/users | Create user |
| GET | /api/users/{id} | Get user by ID |

### Notifications
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/notifications/send | Send notification |
| GET | /api/notifications/user/{userId} | Get user notifications |

## Deployment

### Railway

1. Push this repo to GitHub
2. Connect the GitHub repo to Railway
3. Railway auto-builds from the Dockerfile
4. Set `ASPNETCORE_ENVIRONMENT=Production` if needed

### GitHub Actions

CI/CD pipeline runs on every push to `master` or `main`:
1. Restore dependencies
2. Build (Release)
3. Run all unit tests
4. Build Docker image

## Project Status

- [x] Clean Architecture structure
- [x] Domain entities and enums
- [x] Application use cases with FluentValidation
- [x] Infrastructure repositories with EF Core + SQLite
- [x] API controllers with Swagger
- [x] Unit tests — 14 passing
- [x] Docker containerization
- [x] GitHub Actions CI/CD
- [x] Railway-ready deployment

## Author

**Noel Mino**
GitHub: https://github.com/nmino1984
