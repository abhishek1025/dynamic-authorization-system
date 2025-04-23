
# 🛡️ Dynamic Authorization API (.NET 8 - Onion Architecture)

A clean and scalable .NET 8 solution implementing **dynamic role-based authorization** using **Dapper**, **MS SQL Server**, and **Onion Architecture** principles. Built to demonstrate modularity, flexibility, and maintainability for real-world API development.

---

## 📁 Project Structure

```
├── Domain           # Entity models, Enums, Interfaces
├── Application      # DTOs, Service Interfaces & Implementations
├── Infrastructure   # Data access layer (Dapper), Repository implementations
└── Presentation     # ASP.NET Core Web API (Controllers, Middleware, Utils)
```

---

## 🔧 Tech Stack

- **.NET 8**
- **Dapper** for lightweight data access
- **MS SQL Server** (via `Microsoft.Data.SqlClient`)
- **JWT Authentication** for secure APIs
- **BCrypt** for password hashing
- **Swagger / OpenAPI** for API documentation
- **Onion Architecture** for maintainable, layered structure

---

## 🚀 Getting Started

1. **Clone the Repository:**

```bash
git clone https://github.com/abhishek1025/dynamic-authorization-system.git
```

2. **Configure Connection Strings and JWT:**

Edit `appsettings.Development.json` in the `Presentation/dynamic-authorization.api` folder:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=authorizationProject;User Id=sa;Password=your_password;"
},
"JwtSettings": {
    "Secret": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "ExpiresInMinutes": 60
}
```

3. **Run Database Scripts / Migrations:**

Ensure **SQL Server** is running and your database schema (Users, Permissions, etc.) is prepared.

4. **Run the API:**

```bash
cd Presentation/dynamic-authorization.api
dotnet run
```

5. **Explore API via Swagger UI:**

```
https://localhost:<port>/swagger
```

---

## 🧩 Features

- Modular **Onion Architecture** with four layers (Domain, Application, Infrastructure, Presentation).
- **Dynamic Authorization** with role and resource-based permissions.
- **JWT Bearer Authentication**.
- **Dapper-based Repositories** for efficient database interaction.
- **Custom Middleware** for error handling and consistent API responses.
- **Swagger UI** for interactive API exploration.

---

## 📝 Example API Endpoints

- **POST** `/api/auth/signin` – User sign-in with JWT token generation.
- **POST** `/api/auth/signup` – User registration.
- **GET** `/api/users` – Retrieve user list (protected).
- **POST** `/api/permissions` – Assign permissions to users.

---

## 🛠️ Build & Run

```bash
# Build the entire solution
dotnet build

# Run the API project
dotnet run --project Presentation/dynamic-authorization.api
```

---

## 🤝 Contributing

1. Fork the repo
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add YourFeature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

---

## 📜 License

This project is licensed under the MIT License.

---
