# BookStore 📚

BookStore is an ASP.NET MVC web application designed for managing an online bookstore. It includes features for user authentication via username and password (with passwords securely hashed), authorization with Google OAuth2 for specific features, CRUD operations for book management, email notifications for order confirmations, and password reset functionality using SMTP. The application supports role-based access, allowing admins to manage books, accounts, warehouse inventory, and categories.

## Features ✨

- **User Authentication** 🔒: Secure login using username and password, with passwords hashed using `PasswordHasher`.
- **Google OAuth2 Authorization** 🔗: Authorizes users to access specific features without storing OAuth2 data in the database.
- **CRUD Operations** 📝: Create, read, update, and delete books, accounts, and categories.
- **Order Confirmation Emails** 📧: Sends emails to users upon checkout, including a list of purchased items via SMTP.
- **Password Reset** 🔑: Allows users to reset passwords by receiving a token via email (SMTP).
- **Role-Based Access** 👥:
  - **Users**: Browse and purchase books, with access to additional features via OAuth2 authorization.
  - **Admins**: Manage books, user accounts, warehouse inventory, and book categories.
- **Database** 💾: Uses a `bookStore` database for storing application data.

## Prerequisites 🛠️

To run the BookStore project locally, ensure you have the following installed:

- **Visual Studio 2022** (with ASP.NET and web development workload)
- **.NET Framework** (version compatible with the project, typically .NET 4.x or .NET Core depending on the setup)
- **SQL Server** (for the `bookStore` database)
- **Google OAuth2 Credentials** (Client ID and Secret for authorization)
- **SMTP Server Credentials** (e.g., Gmail SMTP, configured in the `SendEmail` functions)
- A `bookStore` database set up in SQL Server (see Database Setup below)

## Getting Started 🚀

### 1. Clone the Repository

```bash
git clone https://github.com/HQuangDat/BookStore.git
cd BookStore
```

### 2. Database Setup 🗄️

1. **Create the Database**:

   - Open SQL Server Management Studio (SSMS) or your preferred SQL client.

   - Create a new database named `bookStore`.

   - Update the connection string in `Web.config` or `appsettings.json` to point to your SQL Server instance. Example:

     ```xml
     <connectionStrings>
       <add name="DefaultConnection" connectionString="Server=your_server_name;Database=bookStore;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

2. **Run Migrations** (if using Entity Framework):

   - Open the Package Manager Console in Visual Studio.

   - Run the following commands:

     ```bash
     Update-Database
     ```

   Alternatively, if the database schema is provided as a `.sql` file in the repository, execute it to set up the tables.

### 3. Configure Google OAuth2 🔗

1. Go to the Google Cloud Console.

2. Create a new project and enable the **Google+ API** (or equivalent for OAuth2).

3. Create OAuth 2.0 credentials (Client ID and Client Secret).

4. Add the credentials to your configuration file (`Web.config` or `appsettings.json`):

   ```xml
   <appSettings>
     <add key="GoogleClientId" value="your_client_id" />
     <add key="GoogleClientSecret" value="your_client_secret" />
   </appSettings>
   ```

### 4. Configure SMTP for Email 📬

The SMTP configuration is handled within the `SendEmail` functions in the `Receipt` and `Account` controllers. Ensure you have valid SMTP credentials (e.g., Gmail SMTP with an App Password if 2-Step Verification is enabled). Update the credentials directly in the `SendEmail` functions as needed.

Example configuration in the `SendEmail` function (pseudo-code, adjust based on your implementation):

```csharp
var smtpClient = new SmtpClient("smtp.gmail.com")
{
    Port = 587,
    Credentials = new NetworkCredential("your_email@gmail.com", "your_app_password"),
    EnableSsl = true,
};
```

### 5. Run the Application ▶️

1. Open the solution (`BookStore.sln`) in **Visual Studio 2022**.
2. Ensure the `bookStore` database is running and accessible.
3. Press `F5` or click **Start** in Visual Studio to run the application.
4. The application should launch in your default browser (e.g., `http://localhost:port`).

## Project Structure 🏗️

- **Controllers/**: Contains MVC controllers, including `Receipt` and `Account` controllers with `SendEmail` functions for SMTP.
- **Models/**: Defines data models and Entity Framework configurations.
- **Views/**: Razor views for the user interface.
- **App_Data/**: May contain database-related files (if applicable).
- **Web.config**: Configuration file for database connections and Google OAuth2 settings.

## Usage 🖱️

- **Users**:
  - Log in using username and password (securely hashed).
  - Authorize with Google OAuth2 to access specific features.
  - Browse books, add to cart, and checkout.
  - Receive order confirmation emails with purchased items.
  - Reset password via email if needed.
- **Admins**:
  - Log in with admin credentials.
  - Manage books (add, edit, delete).
  - Manage user accounts and categories.
  - Monitor warehouse inventory.

## Contributing 🤝

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes and commit (`git commit -m "Add your feature"`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a Pull Request.

## Contact 📩

For questions or support, contact:

- **Author**: Ho Quang Dat
- **GitHub**: HQuangDat
- **Email**: \hoquangdat123@gmail.com

---
