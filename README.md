# DefaultOnionArchitecture

A clean and extensible starter template for ASP.NET Core applications using **Onion Architecture**, **CQRS**, and **MediatR** patterns. This template is designed for scalable enterprise projects with built-in support for validation, caching, authentication, Swagger documentation, and multi-layered separation of concerns.

---

## 🧰 Tech Stack

- **ASP.NET Core 8**
- **Entity Framework Core**
- **Onion Architecture**
- **CQRS with MediatR**
- **FluentValidation**
- **JWT Authentication**
- **Swagger / Swashbuckle**
- **AutoMapper**
- **SQL Server**
- **Serilog Logging**
- **In-Memory Caching**
- **Global Error Handling**
- **Pagination + Overflow Protection**

---

## 🗂️ Project Structure

```
/src
 ├── Application       # Business rules, CQRS handlers, DTOs, Validators
 ├── Domain            # Domain models & interfaces
 ├── Infrastructure    # Database access, JWT, services, caching etc.
 ├── Persistence       # EF Core DbContext and configurations
 └── WebAPI or UI            # API/UI layer, Controllers, Middlewares
```

---

## 📦 Features

### ✅ Clean Architecture (Onion Style)
- Separation of concerns across layers (Domain, Application, Infrastructure, API)
- Loose coupling and testability

### 📤 CQRS & MediatR
- Queries and commands are fully separated
- Each request handled by its own handler
- Supports pipeline behaviors like validation

### 🛡️ FluentValidation
- All incoming requests are validated using custom validators
- Plugged into MediatR pipeline for automatic execution

### 🔐 JWT Authentication
- Secure token-based authentication
- Configurable expiry, signing key, and roles

### 📚 Swagger UI
- Interactive API documentation at `/swagger`
- Token support for secured endpoints

### 🛑 Global Exception Handling
- Centralized error middleware
- Returns standard error response model

### 🧠 AutoMapper
- DTO ↔ Entity mapping handled cleanly
- Centralized profile registration

### 🧩 Overflow Protection
- Pagination endpoints contain `MaxPageSize` enforcement to prevent memory overuse or abuse.
- Prevents clients from requesting excessive page sizes (default max: 100).

### 🗃️ In-Memory Caching
- Simple caching layer for queries
- Improves performance of repetitive data fetching

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Clone the Repo

```bash
git clone https://github.com/sevketardam/DefaultOnionArchitecture.git
cd DefaultOnionArchitecture
```

### Update DB Connection

Edit `appsettings.json` in **WebAPI** project:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=YourDb;Trusted_Connection=True;"
}
```

### Run Migrations

```bash
cd src/WebAPI
dotnet ef database update
```

### Launch the API

```bash
dotnet run
```

Visit `https://localhost:5001/swagger` to explore the endpoints.

---

## 🚀 Sample Endpoints

| Method | Endpoint             | Description                 |
|--------|----------------------|-----------------------------|
| GET    | `/api/products`      | Get paginated product list |
| POST   | `/api/products`      | Create a new product        |
| PUT    | `/api/products/{id}` | Update existing product     |
| DELETE | `/api/products/{id}` | Delete a product            |

> Swagger includes example request bodies and responses.

---

## 🧪 Testing & Validation

- Requests without valid data will return 400 with detailed validation messages.
- Invalid JWT tokens are rejected automatically.
- Over-limit pagination requests are blocked with proper error message.

---

## 📌 Notes

- This template is designed to be a **starting point** for production-level projects.
- Easily extendable to support:
  - Redis or Distributed Cache
  - Background Jobs (e.g., Hangfire)
  - RabbitMQ / Kafka
  - Role-based Authorization
  - Docker support

---

## 🙋 Author

Developed by [Şevket Arda](https://github.com/sevketardam)

---

## 📜 License

This project is open-source under the MIT license.
