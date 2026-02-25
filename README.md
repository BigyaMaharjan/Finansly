💰 Finansly – Personal Finance Tracker API

Finansly is a clean architecture-based ASP.NET Core Web API for managing personal finances.
It supports JWT authentication, transaction management, category tracking, pagination, sorting, and reporting.

🚀 Tech Stack

.NET 8

ASP.NET Core Web API

Entity Framework Core

Dapper (for reporting & optimized queries)

JWT Authentication

SQL Server

Clean Architecture

Swagger / OpenAPI

📂 Project Structure
Finansly
│
├── Finansly.Api              → API controllers & middleware
├── Finansly.Application      → Business logic & DTOs
├── Finansly.Domain           → Core domain entities
├── Finansly.Infrastructure   → EF Core, Dapper, Repositories
├── Finansly.Presentation     → OpenAPI, Extensions
├── Finansly.Tests            → Unit & Integration tests
🔐 Authentication

Finansly uses JWT Bearer Authentication.

Login Flow

User logs in with email/password

API generates JWT token

Token must be passed in header:

Authorization: Bearer {your_token}
⚙️ Configuration

Update your appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=FinanslyDb;Trusted_Connection=True;TrustServerCertificate=True"
},
"JwtSettings": {
  "Key": "your-super-secret-key",
  "Issuer": "Finansly",
  "Audience": "FinanslyUsers",
  "DurationInMinutes": 60
}
🧱 Database Setup
1️⃣ Apply Migrations
dotnet ef database update

Or if using Package Manager Console:

Update-Database
📌 API Features
✅ User Management

Register

Login

JWT Token generation

✅ Transactions

Create transaction

Update transaction

Delete transaction

Get paginated transactions

Sorting & filtering support

✅ Categories

Create category

Assign to transactions

✅ Reporting (Dapper Optimized)

Summary reports

Aggregated financial data

🔎 Pagination & Sorting

Example request:

GET /api/transactions?pageNumber=1&pageSize=10&sorting=Amount&sortType=Descending

Supported sorting fields:

CreatedAt

Amount

Title

🧪 Running Tests
dotnet test

Unit tests are located in:

Finansly.Tests
🧠 Architecture Highlights

Clean separation of concerns

CancellationToken propagated through async calls

Transaction handling via Unit of Work

Hybrid EF Core (tracking) + Dapper (performance queries)

Strong DTO usage (no entity leakage)

Centralized API response wrapper

🛡 Security Considerations

JWT expiration handling

Bearer authentication enforced globally

Sorting whitelist to prevent SQL injection

Request validation with proper response typing

📖 Swagger Documentation

Run the project and navigate to:

https://localhost:{port}/swagger

JWT authentication is supported directly in Swagger UI.

🏁 Running the Project
dotnet build
dotnet run
👨‍💻 Author

Developed as a clean architecture finance tracking backend project.

⭐ Future Improvements

Refresh token implementation

Role-based authorization

Docker support

CI/CD pipeline

Integration tests with Testcontainers

Caching (Redis)

Rate limiting
