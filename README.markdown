# BookStore 📚

BookStore is an ASP.NET Core Razor Pages web application for managing an online bookstore. It supports user authentication, Google OAuth2, book and category management, cart and warehouse features, password reset via email, and background job processing with Hangfire.

## Features

- **User Authentication**: Secure login with username and password (hashed).
- **Google OAuth2**: Login and access with Google accounts.
- **Role-Based Authorization**: Admin, User, and Saler roles for access control.
- **Book Management**: CRUD operations for books and categories.
- **Cart**: Add, update, and remove books from user cart.
- **Warehouse**: Manage book inventory and quantities across warehouses.
- **Password Reset**: Users can reset passwords via email with secure tokens.
- **Email Notifications**: Password reset and order confirmation emails sent via SMTP.
- **Background Jobs**: Email sending handled asynchronously using Hangfire.
- **Logging**: Application events logged with Serilog.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or later
- SQL Server (for the `bookStore` database)
- Google OAuth2 credentials (Client ID/Secret)
- SMTP credentials (e.g., Gmail App Password)

### Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/HQuangDat/BookStore.git
   cd BookStore
   ```
2. **Configure the database:**
   - Create a SQL Server database named `bookStore`.
   - Update the connection string in `appsettings.json` or user secrets.
3. **Apply migrations:**
   - Open the solution in Visual Studio.
   - Run `Update-Database` in the Package Manager Console.
4. **Configure Google OAuth2:**
   - Add your Google Client ID and Secret to configuration.
5. **Configure SMTP:**
   - Update email and app password in `SendMailService.cs`.
6. **Run the application:**
   - Press F5 or run `dotnet run`.

### Hangfire Dashboard

Hangfire is used for background job processing (e.g., sending emails). You can add the Hangfire dashboard to your app for monitoring jobs.

## Project Structure

- `Controllers/` - MVC controllers for books, accounts, cart, warehouse, etc.
- `DataModels/` - Entity Framework Core models.
- `Repositories/` - Data access and business logic.
- `Service/` - Background services (e.g., email sending).
- `Views/` - Razor views for UI.
- `Program.cs` - App startup and configuration.

## Usage

- **Users**: Register, login, browse books, manage cart, checkout, reset password.
- **Admins**: Manage books, categories, users, and warehouse inventory.

## Contributing

Pull requests are welcome! Please fork the repo and submit a PR.

## License

This project is for educational/demo purposes.

## Author

- **Ho Quang Dat**
- GitHub: [HQuangDat](https://github.com/HQuangDat)
- Email: hoquangdat123@gmail.com
