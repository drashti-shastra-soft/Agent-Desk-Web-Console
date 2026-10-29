# AgentDesk Web Console

ASP.NET MVC web application for AgentDesk, an insurance agency management system. It is the user-facing site (admin UI). It does not talk to the database directly: every read and write goes through the **AgentDesk Web Service** (REST API).

## Features

- Login with role-based access (System Owner, company users and others)
- Multi-company data: normal users see only their own company, the System Owner can see all companies
- Masters: Company, User, Agency, Group, Client, Address
- Policies with nominee, plan / term / PPT and maturity date
- Bulk import of policies from an Excel file (.xlsx), one row at a time with per-row status
- Update FUP: upload an Excel file, verify the policy number, record the premium and set the next FUP date from the adjustment date
- Server-side paging, search and dropdown filters on list screens

## Tech stack

| Area | Technology |
|---|---|
| Framework | ASP.NET MVC 5 (.NET Framework 4.x) |
| UI | Razor views, Bootstrap, AdminLTE, jQuery, Font Awesome |
| Dependency injection | Unity |
| API access | HttpClient calls to the Web Service with a JWT bearer token |
| Session | `HttpContext.Session` (`HttpContextHelper.LoginUser`) |

## Project structure

```
AgentDeskWebConsole/
├── Controllers/        MVC controllers (Agency, User, UpdateFup, ...)
├── Domain/             View models and domain classes (match the API contracts)
├── Filter/             Action filters, e.g. [Authentication]
├── Helper/             PaginationHelper, EnumsHelper, HttpContextHelper
├── Infrastructure/     IService interfaces, service classes, StaticMethods
├── Views/              Razor views and partials (_List, _Create, ...)
├── custom-js/          Page scripts (list.js, create.js per module)
└── Web.config
```

## Getting started

### Requirements

- Visual Studio 2019 or later with the ASP.NET workload
- .NET Framework 4.x
- IIS Express or IIS 10
- A running instance of **AgentDesk Web Service**

### Setup

1. Clone the repository and open the `.sln` in Visual Studio.
2. Restore NuGet packages.
3. Set the API address in `Web.config` (use the key your project already has):

   ```xml
   <appSettings>
     <add key="ApiBaseUrl" value="https://your-api-host/" />
   </appSettings>
   ```

4. Build and run (F5).
5. Sign in with a user created in the database.

### Configuration notes

- Never commit real connection strings, API URLs, salts or keys. Keep secrets in `Web.config` transforms or environment-specific files that are in `.gitignore`.
- After changing login or role data, log out and log in again. The logged-in user is kept in the session.

## Roles and company scope

- `StaticMethods.IsSystemOwner()` is true for the System Owner role. Only that role sees the Company menu and the company dropdowns.
- `StaticMethods.ApplyCompany()` sets `CompanyId` from the logged-in user on every create, update and import. The company is never taken from the page.
- `StaticMethods.ResolveCompanyId()` forces list searches to the user's own company unless the user is the System Owner.
- `StaticMethods.CanAccess()` is used before loading, editing or deleting a record by id.

## Excel import (policies)

1. Choose an `.xlsx` file on the Update FUP / import screen. The file is parsed and shown in a table.
2. Click **Start Upload**. Rows are posted one by one with a short delay, and each row shows its own status (saved or the error message).
3. The server groups, clients, addresses, policy and nominee are created or reused per company. See the service README for the rules.

Expected columns: SNo, Family, Name, Mobile, Email, Address, DOB, Policy No, Ag Code, Com Date, Plan/Term/PPT, Sum Assured, Mode, FUP Date, Premium, Nominee, Nominee Age, Relation.

## Troubleshooting

| Problem | Check |
|---|---|
| Company menu or dropdown not visible | `RoleId` and `CompanyId` must be returned by the API login. Log out and in again. |
| `CompanyId` is 0 on saved data | `StaticMethods.ApplyCompany` is missing in the controller, or the API domain class has no `CompanyId` property |
| 500 from the API | Read the response body (`response.Content.ReadAsStringAsync()`) for the real error |
| Browser console 404 | A missing static file, for example `/Content/assets/img/loader.gif` |

## License

Add your license here.
