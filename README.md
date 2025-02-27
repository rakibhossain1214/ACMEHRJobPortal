# HR Job Portal

A comprehensive web-based HR Job Portal built with ASP.NET Core, Blazor Server, and Entity Framework Core.

![HR Job Portal](https://via.placeholder.com/800x400?text=HR+Job+Portal)

## Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Setup Instructions](#setup-instructions)
- [Database Configuration](#database-configuration)
- [Database Backup and Restoration](#database-backup-and-restoration)
- [Email Configuration](#email-configuration)
- [Running the Application](#running-the-application)
- [Key Features Guide](#key-features-guide)
- [Demo Credentials](#demo-credentials)

## Features

- **User Authentication**: Register and login system
- **Profile Management**: Create and maintain personal profiles
- **Education Management**: Add and manage multiple education records
- **Resume Management**: Upload and manage resume documents
- **Job Listings**: Browse and search for available jobs 
- **Job Applications**: Apply for jobs and track application status
- **Pagination**: Navigate through multiple pages of job listings
- **Email Notifications**: Receive email confirmations for job applications

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Frontend**: Blazor Server with Razor Components
- **Database**: SQL Server
- **ORM**: Entity Framework Core (with Code-First approach)
- **Styling**: Bootstrap 5

## Project Structure

- **Components/Pages**: Blazor pages for user interface
- **Data**: Database models, DbContext, and services
- **wwwroot**: Static files (CSS, JS, images)

## Setup Instructions

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code
- SQL Server Management Studio (SSMS) for database backups

### Installation Steps

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/HRJobPortal.git
   cd hrjobportal
   ```

2. Restore NuGet packages:
   ```
   dotnet restore
   ```

3. Install Entity Framework Core tools (if not already installed):
   ```
   dotnet tool install --global dotnet-ef
   ```

### Database Configuration

1. Update the connection string in `appsettings.json` if needed:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HRPortal;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. Apply database migrations:
   ```
   dotnet ef database update
   ```

   This will create the database and seed it with initial data including:
   - Sample users
   - Sample job listings
   - Sample applications

### Database Restoration

**Instruction:**  

Get the backup file `HRPortal.bak` from the `backups` folder of the root directory.

#### Restoring a Database from Backup

**Using SQL Server Management Studio (SSMS):**

1. Open SQL Server Management Studio
2. Connect to your database server
3. In Object Explorer, right-click on Databases
4. Select Restore Database...
5. In the Restore Database dialog:
   - Select Device and click the browse button (...)
   - Add your .bak file
   - Verify the database name in "To database:" field
   - Check the backup sets to restore
6. Click OK to restore the database
7. Update the connection string in `appsettings.json` to point to the restored database if needed

**Using T-SQL Script:**

```sql
RESTORE DATABASE HRPortal
FROM DISK = 'C:\Path\To\Backups\HRPortal.bak'
WITH REPLACE, RECOVERY;
```

**Using Command Line (sqlcmd):**

```
sqlcmd -S ServerName -E -Q "RESTORE DATABASE HRPortal FROM DISK = 'C:\Path\To\Backups\HRPortal.bak' WITH REPLACE, RECOVERY"
```

#### Verifying the Restored Database

After restoration:

1. Connect to the database in SSMS
2. Run a simple query to verify data integrity:
   ```sql
   SELECT COUNT(*) FROM Users;
   SELECT COUNT(*) FROM Jobs;
   SELECT COUNT(*) FROM JobApplications;
   ```
3. Update the connection string in your application's `appsettings.json` if necessary
4. Run the application and test functionality with the restored database

### Email Configuration

For email notifications to work, update the SMTP settings in `appsettings.json`:

```json
"SmtpSettings": {
  "Host": "smtp.example.com",
  "Port": 587,
  "Username": "your-email@example.com",
  "Password": "your-password",
  "SenderEmail": "noreply@hrjobportal.com",
  "SenderName": "HR Job Portal"
}
```

**Note**: For production, consider using a service like SendGrid, Mailgun, or Amazon SES.

## Running the Application

1. Build and run the application:
   ```
   dotnet run
   ```

2. Open your browser and navigate to:
   ```
   https://localhost:7173
   ```

## Key Features Guide

### User Registration and Login

1. Click on "Register" in the navigation menu
2. Fill in your details and create an account
3. Login with your email and password

### Profile Management

1. After logging in, click on "My Profile" in the navigation menu
2. Update your personal information
3. Add education history
4. Upload your resume

### Job Listings and Applications

1. Browse jobs by clicking on "Job Listings" in the navigation menu
2. Use pagination to navigate through multiple pages of jobs
3. Click on a job to view its details
4. Click "Apply Now" to submit an application
5. Track your applications by clicking on "My Applications"

### Withdrawing Applications

1. Go to "My Applications" in the navigation menu
2. Find the application you want to withdraw
3. Click the "Withdraw" button

## Demo Credentials

The system is seeded with the following demo accounts:

### Job Seeker
- Email: john@example.com
- Password: password123

### Alternate User
- Email: jane@example.com
- Password: password123

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributors

- A. M. Rakibul Hossain

## Acknowledgements

- Bootstrap - For the responsive design
- ASP.NET Core team - For the amazing framework
- Entity Framework Core - For the ORM
