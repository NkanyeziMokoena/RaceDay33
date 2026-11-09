# RaceDay Part 3 — ASP.NET Core MVC

This MVC application is the browser-based front end for the RaceDay API. It uses `RaceDayApiClient` for all data operations; it does not connect directly to SQL Server.

## Projects
- `raceday_api`: Part 2 REST API (SQL Server and server-side session authentication).
- `raceday_mvc`: Part 3 MVC website (Razor views, session-based navigation, API client).
- `raceday_tests`: API test project.

## Run locally
1. Open `RaceDay.sln` in Visual Studio.
2. Confirm the API connection string in `raceday_api/appsettings.json` points to your SQL Server instance and the RaceDay database.
3. Start the API project first. Its HTTPS profile is `https://localhost:7080` and Swagger is at `/swagger`.
4. Start `raceday_mvc`. The MVC site uses `https://localhost:7180` and calls the API URL in `raceday_mvc/appsettings.json`.
5. Register a real account and sign in. Seeded placeholder password hashes cannot be used to log in.

If your API uses another port, update `Api:BaseUrl` in `raceday_mvc/appsettings.json` to match the API launch profile.

## Features in the MVC project
- Public event browsing and event details.
- Register, login and logout through the API.
- MVC session stores the authenticated user's ID, display name and role; the API session cookie is forwarded for protected API requests.
- Organiser-only dashboard, event creation/editing/deletion, category management, enrolment viewing and result entry.
- Participant-only enrolment, own enrolments, cancellation and personal results.
- Profile editing through the API.
- Responsive sporting theme and role-aware navigation.

## Docker
Build from the repository root:

```bash
docker build -f raceday_mvc/Dockerfile -t raceday-mvc .
```

Run the MVC container:

```bash
docker run --rm -p 8080:8080 -e Api__BaseUrl=http://host.docker.internal:5080/ raceday-mvc
```

The API must be running and reachable from the container. For a fully containerised deployment, put the API and MVC services on the same Docker network and set `Api__BaseUrl` to the API service name.

## Items requiring completion/verification before submission
- The Part 2 API currently has no Azure Blob Storage upload endpoint or storage configuration. The MVC UI therefore does not claim to upload event banners/profile images. Implement the upload in the API first, then add MVC forms that post files to that API endpoint.
- Confirm the API schema matches the Part 1 SQL Server script. The API's current EF Core model uses a Role table and different event/category/enrolment fields from the Part 1 script; this needs reconciliation before database integration can be considered complete.
- Build and run both projects on the student's computer, test every role flow, and capture genuine screenshots/video evidence. Do not submit a claimed green CI build until GitHub Actions passes.
