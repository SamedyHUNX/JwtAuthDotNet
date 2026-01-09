# JwtAuthDotNet

A modern ASP.NET Core 10.0 Web API showcasing JWT (JSON Web Token) authentication with refresh token functionality, role-based authorization, and PostgreSQL database integration.

## 🚀 Features

- **JWT Authentication**: Secure token-based authentication using JSON Web Tokens
- **Refresh Token**: Automatic token refresh mechanism for seamless user experience
- **Role-Based Authorization**: Fine-grained access control with role-based endpoints
- **Entity Framework Core**: PostgreSQL database integration with EF Core 10.0
- **API Documentation**: Interactive API documentation using Scalar
- **Clean Architecture**: Service-based architecture with DTOs and dependency injection

## 🛠️ Tech Stack

- **Framework**: .NET 10.0
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core 10.0
- **Authentication**: JWT Bearer Authentication
- **API Documentation**: Scalar (OpenAPI)
- **Packages**:
  - `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication middleware
  - `Microsoft.IdentityModel.Tokens` - Token validation
  - `Npgsql.EntityFrameworkCore.PostgreSQL` - PostgreSQL provider
  - `Scalar.AspNetCore` - API documentation

## 📋 Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (running on port 5433)
- A code editor (Visual Studio, VS Code, or Rider)

## 🔧 Installation

1. **Clone the repository**

   ```bash
   git clone <https://github.com/SamedyHUNX/JwtAuthDotNet>
   cd JwtAuthDotNet
   ```

2. **Configure the database connection**

   Update the connection string in `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5433;Database=JwtAuthDotnet;Username=your_username"
   }
   ```

3. **Configure JWT settings**

   Update the JWT configuration in `appsettings.json`:

   ```json
   "AppSettings": {
     "Token": "your-super-secret-key-here-make-it-long-and-secure",
     "Issuer": "YourIssuerName",
     "Audience": "YourAudienceName"
   }
   ```

   > [!WARNING]
   > Never commit sensitive credentials to version control. Use **User Secrets** or environment variables for production.

4. **Apply database migrations**

   ```bash
   dotnet ef database update
   ```

5. **Restore dependencies**
   ```bash
   dotnet restore
   ```

## 🚀 Running the Application

### Development Mode

```bash
dotnet run
```

The API will be available at:

- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **API Documentation**: `https://localhost:5001/scalar/v1` (in development mode)

### Build for Production

```bash
dotnet build --configuration Release
dotnet publish --configuration Release
```

## 📚 API Endpoints

### Authentication

#### Register a New User

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "john_doe",
  "password": "SecurePassword123!"
}
```

#### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "SecurePassword123!"
}
```

**Response:**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token-here"
}
```

#### Refresh Token

```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "your-refresh-token-here"
}
```

### Protected Endpoints

#### Authenticated Endpoint

```http
GET /Auth
Authorization: Bearer {your-access-token}
```

#### Admin-Only Endpoint

```http
GET /admin-only
Authorization: Bearer {your-admin-access-token}
```

> [!NOTE]
> Protected endpoints require a valid JWT token in the Authorization header.

## 🗂️ Project Structure

```
JwtAuthDotNet/
├── Controllers/          # API controllers
│   └── AuthController.cs
├── Data/                # Database context
│   └── UserDbContext.cs
├── Dtos/                # Data Transfer Objects
│   ├── RefreshTokenRequest.cs
│   ├── TokenResponseDto.cs
│   └── UserDto.cs
├── Entities/            # Database entities
│   └── User.cs
├── Migrations/          # EF Core migrations
├── Services/            # Business logic services
│   ├── AuthService.cs
│   └── IAuthService.cs
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```

## 🔐 Security

- Passwords are hashed using BCrypt before storage
- JWT tokens are signed using HMAC-SHA256
- Tokens include expiration times for security
- Refresh tokens enable secure token renewal without re-authentication
- Role-based authorization protects sensitive endpoints

> [!CAUTION]
> Always use HTTPS in production environments to protect tokens in transit.

## 🧪 Testing the API

You can test the API using:

1. **Scalar Documentation** (Development): Navigate to `https://localhost:5001/scalar/v1`
2. **Postman**: Import the endpoints and test manually
3. **cURL**: Use command-line requests
4. **HTTPie**: A user-friendly HTTP client

### Example cURL Request

```bash
# Register
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test123!"}'

# Login
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test123!"}'
```

## 🔄 Database Migrations

### Create a new migration

```bash
dotnet ef migrations add MigrationName
```

### Apply migrations

```bash
dotnet ef database update
```

### Remove last migration

```bash
dotnet ef migrations remove
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is available for personal and educational use.

## 👤 Author

**SamedyHUNX**

## 🙏 Acknowledgments

- ASP.NET Core Team for the excellent framework
- Entity Framework Core for seamless ORM
- The .NET community for valuable resources and support

---

**Happy Coding! 🎉**
