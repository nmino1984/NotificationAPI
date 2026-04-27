# NotificationAPI

A modern microservices-ready notification service built with **Clean Architecture**, **.NET 10**, and **Docker**.

## ??? Architecture

- **Domain**: Core business entities (User, Notification)
- **Application**: Use cases and validators (FluentValidation)
- **Infrastructure**: EF Core + SQLite repositories  
- **API**: ASP.NET Core REST endpoints with Swagger

## ? Features

? Send notifications to users  
? List user notifications  
? Mark notifications as read  
? Input validation with FluentValidation  
? Comprehensive unit tests (xUnit + Moq)  
? OpenAPI/Swagger documentation  
? CI/CD with GitHub Actions  
? Docker containerized  

## ?? Prerequisites

- .NET 10 SDK
- Docker (optional)

## ?? Quick Start

### Local Development

\\\ash
# Restore and build
dotnet restore
dotnet build

# Run API
cd API
dotnet run

# API runs on http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
\\\

### Run Tests

\\\ash
dotnet test
\\\

### Docker

\\\ash
docker build -t notificationapi:latest .
docker run -p 80:80 notificationapi:latest
\\\

## ?? API Endpoints

### Users
- **POST** /api/users - Create user
- **GET** /api/users/{id} - Get user by ID

### Notifications
- **POST** /api/notifications/send - Send notification
- **GET** /api/notifications/user/{userId} - Get user notifications

## ?? Deployment

### Railway

1. Push to GitHub
2. Connect GitHub repo to Railway  
3. Railway auto-builds from Dockerfile
4. Environment variables: Add DATABASE_URL if needed

### GitHub Actions

CI/CD pipeline runs on every push to \main\ or \develop\:
- Restores dependencies
- Builds solution
- Runs all tests
- Builds Docker image (optional)

## ?? Project Status

- [x] Clean Architecture structure
- [x] Domain entities and enums
- [x] Application use cases
- [x] Infrastructure repositories with EF Core
- [x] API controllers with Swagger
- [x] Unit tests (xUnit + Moq)
- [x] Docker containerization
- [x] GitHub Actions CI/CD
- [x] Railway-ready deployment

## ????? Author

**Noel Miño**  
Email: o.kindelan@gmail.com  
GitHub: https://github.com/nmino1984

---

**Note**: This project demonstrates microservices architecture patterns and is ready for scalability. Each layer can be deployed independently as separate services in the future.
