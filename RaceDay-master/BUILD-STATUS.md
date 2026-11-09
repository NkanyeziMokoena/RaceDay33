# Build and integration status

## Source corrections applied in this revision
- Corrected the API connection-string database typo (`RaceDaybbd` -> `RaceDayDb`).
- Aligned API launch URLs to `https://localhost:53516` and `http://localhost:53517`, matching the ports previously shown in the student's Visual Studio output.
- Aligned MVC `Api:BaseUrl` to the API HTTPS URL and assigned separate MVC ports (`53518` / `53519`).
- Renamed the API session cookie and updated the MVC API client to forward that exact cookie name.
- Tightened the MVC role filter so a session must contain both a user ID and role.
- Limited the development certificate bypass to Development only.
- Added Part 1 SQL verification SELECT statements and an enrolment/result join query.
- Expanded README instructions and documented remaining rubric items.

## Not yet verified / must be completed
This environment has no `dotnet` executable, so `dotnet restore`, `dotnet build` and `dotnet test` could not be run here. Do not represent this source package as a verified successful build.

The most significant functional blocker is the EF Core schema mismatch. Part 1 SQL defines `Users.Role` as text and does not define a `Roles` table, while the current API expects `Roles` and `Users.RoleId`. The API also expects additional/different fields for categories, enrolments and results. The original Part 1 SQL has been preserved; a deliberate schema/model reconciliation is still required before connecting this API to that database.

Azure Blob Storage is not implemented in this revision. It requires an Azure Storage account, API-side upload handling, URL persistence and MVC forms that send images to the API. The rubric also requires actual local Docker verification, 20 meaningful commits in the student's own repository, a green GitHub Actions run and an unlisted YouTube demo.

## Local validation commands
Run from the repository root in a Developer PowerShell / terminal with .NET 8 installed:

```powershell
dotnet restore .\RaceDay.sln
dotnet build .\RaceDay.sln --configuration Release
dotnet test .\raceday_tests\raceday_tests.csproj --configuration Release
```

Then run the API and MVC separately using the Visual Studio startup project selector. API Swagger: `https://localhost:53516/swagger`. MVC: `https://localhost:53518`.
