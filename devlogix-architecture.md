# DevLogix — Architecture Reference

*Living technical reference: what exists, how it's structured, why. For tasks and checklists, see `devlogix-build-roadmap.md`. Status as of: mid Phase 1.*

## 1. What DevLogix Is

A full-stack ASP.NET Core MVC project-management and bug-tracking app — a self-hosted, lightweight alternative to Jira/Trello. Built to demonstrate real command of the full ASP.NET Core stack (every syllabus unit maps to a genuine feature, not a bolted-on checkbox) while also being something actually usable.

## 2. Confirmed Tech Stack

| Layer | Choice |
|---|---|
| Runtime | .NET 10 (LTS, supported through Nov 2028), C# 14 |
| IDE | Visual Studio 2026 (Windows) |
| Web framework | ASP.NET Core MVC + Web API — single project, monolith |
| ORM | EF Core (SQL Server provider) — all CRUD |
| Raw data access | ADO.NET (Connection/Command/Reader) — dashboard metrics; `SqlDataAdapter` — CSV export |
| Database | SQL Server Express / LocalDB (`(localdb)\mssqllocaldb`) for dev |
| Auth | ASP.NET Core Identity — cookie-based, Roles + Claims + Policies |
| Server-rendered frontend | Bootstrap 5, jQuery, FontAwesome |
| SPA | React + Vite — Kanban board only, embedded in a Razor view; `@hello-pangea/dnd` for drag-and-drop |
| Source control | git, single repo |

## 3. Layered Architecture

```
+-----------------------------------------------------------------------+
|                          Presentation Layer                            |
|   Razor Views + Tag Helpers + jQuery/AJAX   |   React (Vite) Kanban    |
|        (server-rendered pages)               |   SPA — Phase 9         |
+---------------------+-------------------------+------------------------+
                       | MVC actions             | REST/JSON
                       |                         | (same origin, same auth cookie)
+-----------------------v-------------------------v----------------------+
|                          Application Layer                             |
|          MVC Controllers, API Controllers, Action Filters              |
|          DevLogix/Controllers/                                         |
+------------------------------+------------------------------------------+
                                | Interfaces & Services (DI)
+------------------------------v------------------------------------------+
|                            Business Logic                                |
|   Services, ViewModels, LINQ, NotificationDispatcher, Repository<T>      |
|   DevLogix/Services/, DevLogix/Models/                                   |
+------------------------------+------------------------------------------+
                                | EF Core ORM & ADO.NET (+ SqlDataAdapter)
+------------------------------v------------------------------------------+
|                              Data Access                                  |
|          ApplicationDbContext, SQL Server Express                        |
|          DevLogix/Data/                                                  |
+-----------------------------------------------------------------------------+
```

## 4. Project Structure — Actual State

✅ built and populated · 🔄 exists, partially done · ⬜ folder created, empty, waiting on a later phase

```
DevLogix/                              (repo root — git, .sln)
├── DevLogix.sln
└── DevLogix/                          (project folder — .csproj here)
    ├── Controllers/                   ✅ template default (HomeController) only, so far
    ├── Data/                          🔄
    │   ├── ApplicationDbContext.cs         ✅ written
    │   └── ApplicationDbContext.Seed.cs    ✅ written
    ├── Models/                        ✅ fully populated
    │   ├── ApplicationUser.cs
    │   ├── Project.cs
    │   ├── ProjectMember.cs
    │   ├── Ticket.cs
    │   ├── Comment.cs
    │   ├── TimeLog.cs
    │   ├── Notification.cs
    │   ├── Attachment.cs
    │   └── Enums.cs
    ├── Services/                      ⬜ empty — Phases 3 & 6
    │   └── Notifications/             ⬜ empty — Phase 6
    ├── Filters/                       ⬜ empty — Phase 3 (`[LogExecutionTime]`)
    ├── Exceptions/                    ⬜ empty — Phase 3
    ├── Views/                         ✅ template defaults only
    ├── ClientApp/                     ⬜ empty — Phase 9 (React/Vite scaffold)
    ├── wwwroot/                       ✅ template defaults
    ├── App_Data/Uploads/              ⬜ empty — Phase 7
    ├── Migrations/                    🔄 pending — created by `dotnet ef migrations add`, not yet confirmed working
    ├── Program.cs                     🔄 DbContext registered; Identity not wired yet (Phase 2)
    └── appsettings.json               🔄 connection string added
```

## 5. Data Model

**Entities**

| Entity | Core Fields |
|---|---|
| `ApplicationUser` | (from `IdentityUser`: Id, UserName, Email, PasswordHash…) + `FullName`, `CreatedAt` |
| `Project` | Id, Title, Description, CreatedAt, IsActive |
| `ProjectMember` | Id, ProjectId (FK), UserId (FK), JoinedAt |
| `Ticket` | Id, Title, Description, Priority (enum), Status (enum), CreatedAt, ProjectId (FK), CreatedById (FK), AssignedToId (FK, nullable) |
| `Comment` | Id, Content, CreatedAt, TicketId (FK), UserId (FK) |
| `TimeLog` | Id, MinutesSpent, LoggedAt, Note, TicketId (FK), UserId (FK) |
| `Notification` | Id, Channel (enum), Message, IsRead, CreatedAt, RecipientUserId (FK), RelatedTicketId (FK, nullable) |
| `Attachment` | Id, FileName, StoredPath, ContentType, FileSizeBytes, UploadedAt, TicketId (FK), UploadedById (FK) |

**Enums**

| Enum | Values |
|---|---|
| `TicketStatus` | Todo, InProgress, Resolved, Closed, Reopened |
| `Priority` | Low, Medium, High, Critical |
| `NotificationChannel` | Email, InApp, Sms |

**Relationships & Cascade Behavior** — every `ApplicationUser` relationship is `Restrict`, not `Cascade` (SQL Server disallows multiple cascade paths converging on one table, and User touches almost everything):

| From | To | On Delete |
|---|---|---|
| Project | Ticket | Cascade |
| Project | ProjectMember | Cascade |
| Ticket | Comment | Cascade |
| Ticket | TimeLog | Cascade |
| Ticket | Attachment | Cascade |
| Ticket | Notification (RelatedTicket, nullable) | SetNull |
| ApplicationUser | Ticket (CreatedBy) | Restrict |
| ApplicationUser | Ticket (AssignedTo, nullable) | Restrict |
| ApplicationUser | Comment | Restrict |
| ApplicationUser | TimeLog | Restrict |
| ApplicationUser | Notification | Restrict |
| ApplicationUser | Attachment | Restrict |
| ApplicationUser | ProjectMember | Restrict |
| Project ↔ ApplicationUser | many-to-many, via ProjectMember (unique on ProjectId+UserId) | — |

Seed data: three `IdentityRole` rows (Admin, PM, Developer) via `HasData`, fixed GUIDs so every developer gets the same migration.

## 6. Request Lifecycle — Worked Example

How "view a ticket" will flow once Phase 3 exists, to show how the layers actually connect:

```
Browser
  → GET /Tickets/Details/5
  → TicketsController.Details(int id)              [Application layer]
  → ITicketService.GetByIdAsync(id, currentUser)    [Business Logic — checks ProjectMember access]
  → IRepository<Ticket>.GetByIdAsync(id)            [Business Logic]
  → ApplicationDbContext                            [Data Access]
  → SQL Server / LocalDB
  ← Ticket entity
  ← mapped to TicketDetailsViewModel
  ← Details.cshtml (Razor) renders HTML
  ← Browser
```

The Kanban SPA's path differs after the Application layer: `GET /api/tickets` → same service/repository/DbContext chain → JSON instead of a rendered view.

## 7. API Surface (Planned — Phase 3+)

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/tickets` | List, filterable via `?status=&priority=&assignee=&page=` |
| GET | `/api/tickets/{id}` | Retrieve one |
| POST | `/api/tickets` | Create |
| PUT | `/api/tickets/{id}/status` | Transition status (validated against `TicketStatus`) |
| GET | `/api/tickets/{id}/comments` | List comments |
| POST | `/api/tickets/{id}/comments` | Add comment |
| POST | `/api/tickets/{id}/attachments` | Upload (`multipart/form-data`) |
| GET | `/api/tickets/{id}/attachments/{attachmentId}` | Download, access-checked |
| GET | `/api/projects` | List (scoped to caller's membership unless Admin) |
| POST | `/api/projects` | Create (PM/Admin only) |

Auth: same cookie-based Identity session as the rest of the site — no separate JWT scheme, since the SPA is same-origin.

## 8. Security Model (Built + Planned)

- **Built:** `Restrict` delete behavior protects historical data from accidental cascade loss.
- **Phase 2:** Identity, cookie auth, Roles.
- **Phase 7:** Attachments stored under `App_Data/Uploads` (outside `wwwroot` — never directly web-reachable), download endpoint checks ticket access before streaming.
- **Phase 8:** Custom `Department` claim, `CanManageProject` resource-based policy, anti-forgery tokens, parameterized queries (default with EF Core/ADO.NET), Razor's automatic HTML encoding, `Url.IsLocalUrl` on login `returnUrl`.

## 9. Build Roadmap — Status

| Phase | What Gets Built | Status |
|---|---|---|
| 0 — Environment & Scaffolding | Solution, project, folder skeleton, git | ✅ Done |
| 1 — Data Foundations | Models, `ApplicationDbContext`, migration, `IRepository<T>` | 🔄 Models + DbContext done; migration not yet confirmed |
| 2 — Identity & Access | Identity wiring, Roles, register/login | ⬜ Not started |
| 3 — Core MVC CRUD | Controllers, Views, ViewModels, exceptions, `[LogExecutionTime]`, `TicketBoard`, `TicketMetrics` | ⬜ Not started |
| 4 — State Management | Cookies, Session wizard, TempData, Cache, query strings, hidden fields | ⬜ Not started |
| 5 — Data Access Depth | Raw ADO.NET dashboard, `SqlDataAdapter` CSV export | ⬜ Not started |
| 6 — Notifications | `NotificationBase`, sealed subclasses, delegate/events, dispatcher | ⬜ Not started |
| 7 — File IO | Attachments, execution-time log | ⬜ Not started |
| 8 — Security Hardening | Claims/Policies, CSRF/XSS/SQLi/Open Redirect review | ⬜ Not started |
| 9 — Client-Side | jQuery bits, React/Vite Kanban SPA | ⬜ Not started |
| 10 — UI/UX Pass | Design system, responsive, empty/loading/error states, theme | ⬜ Not started |
| 11 — Production Hardening | Logging, health checks, HTTPS, tests | ⬜ Not started |
| 12 — Deploy | Hosting, DB, storage, domain, CI/CD | ⬜ Not started |

## 10. Where We Are, Right Now

Models compile clean. `ApplicationDbContext` + seed file are written and wired into `Program.cs`/`appsettings.json`. The one open item: the `dotnet ef` migration hasn't been confirmed successful yet — we hit a `UseSqlServer` error a couple messages back that never got resolved with an exact error message. That's the actual blocker standing between here and "Phase 1 done."
