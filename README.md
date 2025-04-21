# BookStore
BookStore 📚
A web-based bookstore management system built with ASP.NET MVC (NET8), featuring Google OAuth2 authentication, role-based access control, CRUD operations, and email notifications upon checkout.​

🚀 Features
Google OAuth2 Authentication: Secure login using Google accounts.

Role-Based Access Control: Admin and user roles with specific permissions.

CRUD Operations: Manage books, accounts, warehouses, and categories.

Email Notifications: Send confirmation emails to users after checkout, including a list of purchased items.

Responsive UI: User-friendly interface built with Bootstrap 5.​

🛠️ Technologies Used
ASP.NET MVC (NET8)

Entity Framework Core

Bootstrap 5

Google OAuth2

SMTP for email services​

⚙️ Getting Started
Prerequisites
Visual Studio 2022

.NET 8 SDK

SQL Server​

Installation
Clone the repository:

bash
Copy
Edit
git clone https://github.com/HQuangDat/BookStore.git
Navigate to the project directory:

bash
Copy
Edit
cd BookStore
Set up the database:

Create a new database named BookStore in SQL Server.

Update the connection string in appsettings.json with your database credentials.​

Apply migrations:

bash
Copy
Edit
Update-Database
Run the application:

bash
Copy
Edit
dotnet run
The application will be available at https://localhost:5001.

🔐 Configuration
Ensure you have the following in your appsettings.json for Google OAuth2 authentication:​

json
Copy
Edit
"GoogleKey": {
  "ClientId": "YOUR_GOOGLE_CLIENT_ID",
  "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
}
Note: Do not commit your appsettings.json with sensitive information. Add it to your .gitignore file to prevent accidental commits.​

📧 Email Service
Configure the SMTP settings in appsettings.json to enable email notifications:​

json
Copy
Edit
"SmtpSettings": {
  "Host": "smtp.your-email-provider.com",
  "Port": 587,
  "Username": "your-email@example.com",
  "Password": "your-email-password"
}
🧑‍💻 Author
Ho Quang Dat

GitHub: HQuangDat
