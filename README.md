# DevLogix 🚀

**DevLogix** is a full-stack, multi-layered Project Management and Bug Tracking application. Built to demonstrate enterprise-level architectural patterns, it combines a robust **ASP.NET Core MVC** backend with a modern **React + Vite** Kanban board, comprehensive data access strategies, and role-based security.

---

## 🛠️ Tech Stack

* **Backend**: .NET 10, C# 14, ASP.NET Core MVC, Web API
* **Database**: SQL Server Express, Entity Framework Core, ADO.NET
* **Frontend**: Razor Pages, Bootstrap 5, jQuery, React 19 (via Vite)
* **Design**: Custom CSS Design System, Dark/Light Mode support

---

## ✨ Key Features

### 1. Advanced Data Access
* **Entity Framework Core**: Used for complex relational operations, object mapping, and migrations.
* **ADO.NET (Raw SQL)**: Implemented for high-performance, read-only Dashboard queries using `SqlConnection`, `SqlCommand`, and `SqlDataReader`.
* **SqlDataAdapter**: Used to generate and export flat CSV files from database tables.

### 2. State Management
* **Cookies**: Persistent Dark/Light theme toggle mechanism.
* **Session State**: Multi-step "New Project Wizard" that persists data across requests before finalizing database insertion.
* **TempData**: One-time flash notifications (success/error/info) displayed across the layout.

### 3. Client-Side Integrations
* **React Kanban SPA**: A drag-and-drop Kanban board built with React, Vite, and `@hello-pangea/dnd`, injected directly into a Razor view and communicating with protected Web API endpoints.
* **jQuery/AJAX**: Inline ticket status updates and comment posting without page reloads.

### 4. Object-Oriented Notifications
* Demonstrates advanced OOP patterns (Abstract classes, Sealed classes, `base` keyword, method hiding vs overriding).
* Uses a **Custom Delegate** and **Event Dispatcher** to trigger polymorphic notifications (In-App, Email, SMS) upon ticket status changes.

### 5. File I/O & Security
* **Attachments**: File upload/download system with strict validation (MIME types, extensions, size limits) stored in `App_Data/Uploads`.
* **Resource-Based Authorization**: Custom Handlers ensure only Project Managers and Admins can edit or delete specific projects.
* **Identity**: Full authentication pipeline with Role-based access control (Admin, PM, Developer).

---

## 🚀 Getting Started

Follow these instructions to run the project on your local machine using Visual Studio and SQL Server Express.

### Prerequisites
* **Visual Studio 2026** (or newer)
* **.NET 10 SDK**
* **Node.js & npm**
* **SQL Server Express** and **SQL Server Management Studio (SSMS)**

### 1. Database Configuration
1. Open `DevLogix.slnx` (or `.sln`) in Visual Studio.
2. Open `appsettings.json` and ensure the `DefaultConnection` points to your SQL Server Express instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DevLogix;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

### 2. Apply Migrations
Generate the database schema using Entity Framework Core.
1. In Visual Studio, open **Tools** > **NuGet Package Manager** > **Package Manager Console**.
2. Run the following commands:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

### 3. Build the React Client
The Kanban board requires a one-time build to generate the JavaScript bundle.
1. Open a terminal (or View > Terminal in Visual Studio).
2. Navigate to the `ClientApp` directory:
   ```cmd
   cd DevLogix\ClientApp
   ```
3. Install dependencies and build:
   ```cmd
   npm install
   npm run build
   ```
   *(This places the compiled `kanban.js` file into `wwwroot/kanban` for the Razor engine to load).*

### 4. Run the Application
Press **F5** in Visual Studio to launch the application. 

> **Note on Roles:** When you register a new account, the backend seed logic automatically assigns the **Developer** role so you can immediately begin creating projects and tickets.

---

## 📂 Project Architecture

```text
DevLogix/
├── Authorization/    # Custom requirements & handlers (Resource-based Auth)
├── ClientApp/        # React + Vite Kanban SPA source code
├── Controllers/      # MVC & Web API Controllers
├── Data/             # ApplicationDbContext, Generic Repository pattern
├── Exceptions/       # Custom Exception types
├── Middleware/       # Global Exception handling & routing
├── Models/           # EF Core Entities & Enums
├── Services/         # Business logic layer (Interfaces & Implementations)
├── ViewModels/       # DTOs structured for Razor Views
├── Views/            # Razor markup, grouped by Controller
└── wwwroot/          # Static assets (CSS, JS, generated React bundles)
```

---

*Built by DevLogix Team.*