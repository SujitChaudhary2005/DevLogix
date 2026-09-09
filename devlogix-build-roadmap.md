# DevLogix — Build Roadmap & Go-Live Checklist

*Companion to the v2.0 Architecture Doc and PRD. Living document — check items off as we go. Currently on: Phase 1 (Data Foundations). For the technical reference (what's actually built, data model, request flow), see `devlogix-architecture.md`.*

## Right Now

- [ ] Add the two `ApplicationDbContext` files (`Data/ApplicationDbContext.cs`, `Data/ApplicationDbContext.Seed.cs`)
- [ ] Wire up `Program.cs` (DbContext registration) and `appsettings.json` (connection string)
- [ ] Add the `Microsoft.EntityFrameworkCore.Design` package
- [ ] Run `dotnet ef migrations add InitialCreate` and `dotnet ef database update`
- [ ] Confirm the app still runs, commit the checkpoint
- [ ] *(No rush)* Real team/club/freelance workflow this will serve? Say so and I'll weight optional features accordingly

Everything below this point is reference material, not immediate action items.

## Syllabus Audit — Open Items

Cross-checked against your pasted syllabus text, unit by unit.

- [ ] **`base` keyword** (Unit 1) — not currently demonstrated. Fix: `NotificationBase.LogAttempt()` as a shared `protected virtual` method that each sealed subclass calls via `base.LogAttempt(...)` before its own logic.
- [ ] **Method hiding vs. overriding** (Unit 1) — only overriding is shown. Fix: one small, clearly-commented demo contrasting a `new`-hidden method against an `override`-n one (base-typed reference to a derived object, showing the two resolve differently).
- [ ] **Arrays** (Unit 1) — currently implicit via `List<T>`. Fix: deliberate array uses, e.g. `string[] AllowedAttachmentExtensions`, `int[] PageSizeOptions`.
- [ ] **Explicit custom delegate** (Unit 1) — events currently ride on built-in `EventHandler<T>`. Fix: declare `public delegate void TicketStatusChangedHandler(...)` explicitly, use it for the event.

Everything else in Units 1–8 has a real, non-contrived home in the existing design (confirmed against the Traceability Matrix line by line against the syllabus text).

## Full Syllabus Coverage Map

Legend: ✅ covered · 🔧 fix already planned (matches the Open Items above) · 📘 conceptual/viva knowledge, not a code deliverable

**Unit 1 — Language Preliminaries**

| Topic | Where It Lives | Status |
|---|---|---|
| Intro to .NET framework | Viva knowledge | 📘 |
| Compilation & execution | `dotnet build`/`run` used hands-on | ✅ |
| Basic language constructs | Throughout every class | ✅ |
| Constructor | Every entity/service; constructor injection | ✅ |
| Properties | Every entity/ViewModel | ✅ |
| Arrays and String | `AllowedAttachmentExtensions[]`, `PageSizeOptions[]`; string formatting throughout | 🔧 |
| Indexers | `TicketBoard` — `board[TicketStatus.InProgress]` | ✅ |
| Inheritance | `NotificationBase` → 3 subclasses; `ApplicationUser : IdentityUser` | ✅ |
| `base` keyword | `NotificationBase.LogAttempt()` via `base.LogAttempt(...)` | 🔧 |
| Method hiding & overriding | Overriding: `Send()`. Hiding: dedicated contrast demo | 🔧 |
| Polymorphism | `NotificationDispatcher` | ✅ |
| Structs & enums | `TicketMetrics`; `TicketStatus`/`Priority` | ✅ |
| Abstract & sealed classes | `NotificationBase` / 3 subclasses | ✅ |
| Interface | `IRepository<T>`, `INotificationDispatcher` | ✅ |
| Delegates & Events | Explicit `TicketStatusChangedHandler` delegate + events | 🔧 |
| Partial class | `ApplicationDbContext.cs` / `.Seed.cs` | ✅ |
| Collections | Repos, LINQ, `TicketBoard` | ✅ |
| Generics | `IRepository<T>` | ✅ |
| File IO | Attachments; execution-time log | ✅ |
| LINQ & lambdas | Ticket search/filter service | ✅ |
| Try/catch & exceptions | 3 custom exceptions + global middleware | ✅ |
| Attribute classes/params/targets/multiple | `[LogExecutionTime(...)]` | ✅ |
| Async/await | Every controller & EF Core call | ✅ |

**Unit 2 — Introduction to ASP.NET**

| Topic | Where It Lives | Status |
|---|---|---|
| Framework comparisons, architecture principles | Viva knowledge | 📘 |
| CLI, MSIL, CLR | CLI hands-on every phase; MSIL/CLR conceptual | 📘✅ |
| `dotnet` build/run/test/deploy | Phase 0 (done), Phase 11 (test), Phase 12 (deploy) | ✅ |

**Unit 3 — HTTP and ASP.NET Core**

| Topic | Where It Lives | Status |
|---|---|---|
| HTTP request/response format | Observable via Postman/DevTools once the API exists | 📘 |
| Web architectures, MVC pattern | The whole app | ✅ |
| Architecture, projects, conventions | Directory blueprint, `Program.cs` pipeline | ✅ |

**Unit 4 — Creating ASP.NET Core MVC Applications**

| Topic | Where It Lives | Status |
|---|---|---|
| Environment setup | Phase 0 | ✅ |
| Controllers & Actions | Projects/Tickets/Comments/Attachments/Account | ✅ |
| Action Result types | View/Partial/File/Redirect/Json Result | ✅ |
| Razor & Tag Helpers | All views | ✅ |
| Model binding & validation | ViewModels + data annotations | ✅ |
| URL routing & features | Conventional + attribute routing + typed constraints | ✅ |
| Web API, JSON | `/api/tickets`, `/api/projects`, `/api/tickets/{id}/comments` | ✅ |
| DI & IoC containers | `Program.cs` service registration | ✅ |

**Unit 5 — Working with Database**

| Topic | Where It Lives | Status |
|---|---|---|
| ADO.NET Connection/Command/Reader | Raw dashboard-metrics queries | ✅ |
| Adapter class | `SqlDataAdapter` → `DataTable` → CSV export | ✅ |
| EF Core, ORM | `ApplicationDbContext` | ✅ |
| Choosing DB provider | `UseSqlServer(...)` in `Program.cs` | ✅ |
| Data models & data context | Entities + `ApplicationDbContext` | ✅ |
| CRUD | Full ticket/project/comment CRUD | ✅ |

**Unit 6 — State Management**

| Topic | Where It Lives | Status |
|---|---|---|
| State on stateless HTTP | Conceptual framing for everything below | 📘 |
| Session State | "Create Project" wizard | ✅ |
| TempData | Flash banners | ✅ |
| HttpContext | Underlies Session/Cache/User access | ✅ |
| Cache | Team-member list | ✅ |
| Cookies | Theme preference | ✅ |
| Query Strings | Filter/list views | ✅ |
| Hidden Fields | Wizard step state | ✅ |

**Unit 7 — Client-Side Development**

| Topic | Where It Lives | Status |
|---|---|---|
| Client-side web tech | Bootstrap 5, FontAwesome | ✅ |
| jQuery | Inline comments, status dropdown | ✅ |
| Forms & validation | Data-annotation-driven client validation | ✅ |
| SPA frameworks | React + Vite Kanban board | ✅ |

**Unit 8 — Securing ASP.NET Core**

| Topic | Where It Lives | Status |
|---|---|---|
| ASP.NET Core Identity | Cookie auth, Roles | ✅ |
| Identity service configuration | `AddIdentity<>()` setup in `Program.cs` | ✅ |
| Roles | `[Authorize(Roles = "PM")]` etc. | ✅ |
| Claims & Policies | Custom `Department` claim; `CanManageProject` policy | ✅ |
| Securing controllers/actions | `[Authorize]` throughout | ✅ |
| Cross-site Scripting (XSS) | Razor's automatic HTML encoding | ✅ |
| SQL Injection | Parameterized EF Core/ADO.NET queries | ✅ |
| CSRF | Anti-forgery tokens | ✅ |
| Open Redirect | `Url.IsLocalUrl` check on login `returnUrl` | ✅ |

## Build Order

Deliberately not the same order as the syllabus units — some "later unit" material has to exist early because everything downstream depends on it.

**Phase 0 — Environment & Scaffolding** (Units 2–4 setup)
Install tooling, create the solution, confirm `dotnet build` / `dotnet run` work end to end, first git commit.

**Phase 1 — Data Foundations** (Units 1, 5)
Entities, `ApplicationDbContext` as partial classes, first migration, seed data, `IRepository<T>` / `Repository<T>`.

**Phase 2 — Identity & Access, pulled forward** (Unit 8 — authentication half)
ASP.NET Core Identity, Roles, register/login, `returnUrl` validation. Everything after this needs "who's the current user," so it can't wait until the end.

**Phase 3 — Core MVC CRUD** (Units 1, 4)
Projects/Tickets/Comments controllers + views, ViewModels + validation, custom exceptions + global middleware, `[LogExecutionTime]`, `TicketBoard` indexer, `TicketMetrics` struct.

**Phase 4 — State Management** (Unit 6)
Theme cookie, session-based project wizard (+ hidden field for step state), TempData flash banners, team-list cache, query-string-driven filters.

**Phase 5 — Data Access Depth** (Unit 5)
Raw ADO.NET dashboard queries, `SqlDataAdapter` CSV export.

**Phase 6 — Notifications** (Unit 1 — delegates/events/abstract/sealed)
`NotificationBase` + sealed subclasses, explicit delegate + events, `NotificationDispatcher`. The `base`-keyword and method-hiding fixes from the audit land here.

**Phase 7 — File IO** (Unit 1)
Ticket attachments (access-checked upload/download), rolling execution-time log file.

**Phase 8 — Security Hardening** (Unit 8 — remainder)
Claims + `CanManageProject` policy (needs real `ProjectMember` data, hence this late), CSRF/XSS/SQLi review pass.

**Phase 9 — Client-Side** (Unit 7)
jQuery/AJAX comments + status dropdown, client validation pass, React/Angular Kanban SPA wired to the Web API.

**Phase 10 — UI/UX Pass**
Design system, responsive check, empty/loading/error states, accessibility pass, dark/light theme.

**Phase 11 — Production Hardening**
See checklist below.

**Phase 12 — Deploy**
See Go-Live checklist below.

## UI/UX Quality Bar

- [ ] Real color/type/spacing system — priority and status genuinely color-coded, not default Bootstrap blue everywhere
- [ ] Kanban usable on a phone — status-dropdown fallback stays a first-class path (also your accessibility answer for keyboard/screen-reader users, not just a mobile convenience)
- [ ] Deliberately designed empty states (no projects yet, no tickets yet), loading states, error states
- [ ] Accessible forms — labeled inputs, sufficient contrast, full keyboard navigation
- [ ] Light/dark theme (built on the theme cookie already planned)
- [ ] Consistent icon set and notification/error microcopy

## Production-Readiness Checklist (beyond "it runs")

- [ ] Centralized structured logging (Serilog or similar), not scattered `Console.WriteLine`
- [ ] Global exception middleware with a genuine catch-all, not just the named custom exceptions
- [ ] Config via `appsettings` + environment overrides + local user-secrets — no real secrets ever committed
- [ ] `/health` endpoint for uptime checks
- [ ] HTTPS redirection + HSTS enforced
- [ ] Both client- *and* server-side validation on every form
- [ ] Real EF Core migration/deploy strategy (not `EnsureCreated()`)
- [ ] Basic rate limiting on login (brute-force protection)
- [ ] At least service-layer test coverage (ties to your optional xUnit stretch idea)

## Go-Live Checklist — What We'll Need

Nothing here needs action yet — this is so you know what to start budgeting time and money for.

- [ ] **App hosting** — Azure App Service (most direct from Visual Studio), a Linux VM behind Nginx/Kestrel, or a Docker container on Render/Railway/Fly.io
- [ ] **Production database** — LocalDB won't run outside your dev machine; needs Azure SQL, a managed Postgres (provider swap), or a real SQL Server instance
- [ ] **Persistent attachment storage** — local disk on most PaaS is ephemeral; plan for Azure Blob Storage or S3-compatible storage
- [ ] **Domain name + DNS**
- [ ] **TLS certificate** — usually free/automatic (Let's Encrypt, or built into most PaaS)
- [ ] **Production secrets management** — Azure Key Vault or the host's environment-variable settings, never source control
- [ ] **CI/CD pipeline** — GitHub Actions to build, test, run the SPA's `npm run build`, and deploy on push
- [ ] **Monitoring/alerting** — Application Insights (pairs natively with Azure) or a simpler uptime checker
- [ ] **Backups** — scheduled DB backups plus an actually-tested restore
- [ ] **Realistic budget** — even "cheap" hosting + DB + storage + domain has a real monthly cost; we'll price real options once we're there

## Confirmed Tooling (verified July 2026)

- .NET 10, LTS, supported through Nov 2028 — correct target
- .NET 8 / .NET 9 both end support Nov 10, 2026 — confirmed accurate
- Visual Studio 2026 — real, GA since Nov 2025, **Windows-only**
- Mac/Linux → VS Code + C# Dev Kit, or JetBrains Rider

## Open Decisions — Resolved

- [x] OS: Windows
- [x] Current setup status: Tooling fully installed
- [x] SPA framework: **React** — lighter footprint for one embedded component vs. Angular's full-app structure; Vite + `@hello-pangea/dnd` for drag-and-drop
