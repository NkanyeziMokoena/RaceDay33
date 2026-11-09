# RaceDay – Part 2: RESTful API

ASP.NET Core Web API (C#) with Entity Framework Core (Code-First) on SQL Server LocalDB.

## Project structure
- `raceday_api/Controllers` – the request methods (GET, POST, PUT, DELETE)
- `raceday_api/database` – the classes that store, get, update and delete the data (+ `AppDbContext`)
- `raceday_api/Models` – the tables (Roles, Users, Events, Categories, Enrolments, Results)
- `raceday_api/Auth` – session and role checking
- `raceday_tests` – unit tests (xUnit)

## Setup and run
1. Install Visual Studio with the ASP.NET workload (includes SQL Server LocalDB) and the .NET 8 SDK.
2. Make sure the LocalDB instance exists: `sqllocaldb create api_web` then `sqllocaldb start api_web`.
3. Connection string (`raceday_api/appsettings.json`): `Server=(localdb)\api_web;Database=raceday;...`
4. In Package Manager Console (Default project: raceday_api): `Add-Migration InitialCreate` then `Update-Database`.
5. Run `raceday_api` and open `https://localhost:7080/swagger`.
6. Run tests: `dotnet test part2/RaceDay.sln`.

## Roles
- **Organiser** – creates, updates and deletes their own events and categories, views enrolments, and captures/edits results.
- **Participant** – browses events and categories, enrols, manages their own enrolments, and views leaderboards and their own results.

## Authentication
Session-based. `POST /api/auth/login` stores the user id and role in the server session (cookie). Passwords are hashed with BCrypt.

## CI/CD
![Green build](docs/ci-green-build.png)  <!-- add your screenshot here -->

## Demo video
YouTube (unlisted): <paste link>
