# RaceDay — Parts 1, 2 and 3

This solution brings together the RaceDay planning/database work (Part 1), REST API (Part 2), automated tests, and ASP.NET Core MVC front end (Part 3).

## Solution structure
- `docs/`: Part 1 planning document, ERD, endpoint plan and SQL Server script.
- `raceday_api/`: Part 2 ASP.NET Core Web API.
- `raceday_tests/`: API tests using EF Core InMemory.
- `raceday_mvc/`: Part 3 ASP.NET Core MVC website; all data requests go through the API client.

## Run in Visual Studio
1. Install the .NET 8 SDK and SQL Server LocalDB (or SQL Server Express).
2. Open `RaceDay.sln` in Visual Studio.
3. In `raceday_api/appsettings.json`, change `ConnectionStrings:DefaultConnection` if your SQL Server instance is not `(localdb)\MSSQLLocalDB`. For SQL Express, a common value is `Server=.\SQLEXPRESS;Database=RaceDayDb;Trusted_Connection=True;TrustServerCertificate=True;`.
4. Run `docs/RaceDayDb.sql` in SSMS only against a development database after backing up any data you need. It drops and recreates the Part 1 tables. The script ends with verification `SELECT` statements.
5. **Important database integration check:** the current Part 2 EF Core model still differs from the Part 1 SQL schema (for example, the API expects a `Roles` table and `RoleId`, while Part 1 stores `Role` as text; category and enrolment columns also differ). Do not expect the API to work against the Part 1 database until these model/schema differences have been reconciled. The Part 1 script has been preserved; it has not been silently replaced.
6. Set `raceday_api` as the startup project and run it. Swagger should open at `https://localhost:53516/swagger`. Keep it running.
7. Set `raceday_mvc` as the startup project and run it separately. The MVC site is at `https://localhost:53518`. It calls the API at `https://localhost:53516/` (configured in `raceday_mvc/appsettings.json`).
8. Register a real account through the website. Seeded sample users have placeholder hashes and cannot log in.

## Part 3 features present in the MVC project
- Public event list and event details.
- Register, login and logout via the Part 2 API.
- MVC session stores user ID, role and display name; API session cookie is forwarded for protected API calls.
- Organiser dashboard, event CRUD, category management, enrolment viewing and result entry.
- Participant event enrolment, own enrolments, cancellation and personal results.
- Profile viewing/editing and role-aware navigation.
- Responsive sports-themed Razor UI.

## Docker
The MVC Dockerfile builds the MVC project. The API must be reachable from the container. From the repository root:

```bash
docker build -f raceday_mvc/Dockerfile -t raceday-mvc .
docker run --rm -p 8080:8080 -e Api__BaseUrl=http://host.docker.internal:53517/ raceday-mvc
```

For a fully containerised setup, run API and MVC on the same Docker network and set `Api__BaseUrl` to the API service name. Configure SQL Server connectivity separately.

## Remaining submission checks
- Reconcile EF Core entities with the preserved Part 1 SQL schema and add/verify migrations.
- Implement Azure Blob Storage upload in the API and persist returned image URLs; the current MVC does not yet satisfy the image-upload requirement. This requires an Azure Storage account and connection configuration.
- Build and test on the target Windows/Visual Studio environment, verify both roles and every page, and capture genuine screenshots.
- Push at least 20 meaningful commits to the student's own GitHub repository and verify the GitHub Actions workflow passes.
- Record and submit the required unlisted YouTube demonstration.

A successful build, database connection, Azure upload, green CI run, and video submission must be verified in the student's environment; this repository does not fabricate that evidence.
