/* RaceDay database - Part 1, Section C
   SQL Server / SSMS. Passwords are never seeded or stored in plaintext.
   Authentication is implemented in the API; seeded accounts use placeholder hashes. */
IF DB_ID(N'RaceDayDb') IS NULL
    CREATE DATABASE RaceDayDb;
GO
USE RaceDayDb;
GO

DROP TABLE IF EXISTS dbo.AuditLogs;
DROP TABLE IF EXISTS dbo.Results;
DROP TABLE IF EXISTS dbo.Enrolments;
DROP TABLE IF EXISTS dbo.Categories;
DROP TABLE IF EXISTS dbo.Events;
DROP TABLE IF EXISTS dbo.Users;
GO

CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    FirstName NVARCHAR(60) NOT NULL,
    LastName NVARCHAR(60) NOT NULL,
    Email NVARCHAR(254) NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Users_Role CHECK (Role IN ('Organiser','Participant'))
);
CREATE TABLE dbo.Events (
    EventId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Events PRIMARY KEY,
    OrganiserId INT NOT NULL,
    Name NVARCHAR(120) NOT NULL,
    Description NVARCHAR(1000) NOT NULL,
    EventDate DATETIME2 NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    DistanceKm DECIMAL(6,2) NOT NULL,
    EventType NVARCHAR(10) NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Events_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Events_Users FOREIGN KEY (OrganiserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT CK_Events_Distance CHECK (DistanceKm > 0),
    CONSTRAINT CK_Events_Type CHECK (EventType IN ('Run','Walk','Cycle'))
);
CREATE TABLE dbo.Categories (
    CategoryId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
    EventId INT NOT NULL,
    Name NVARCHAR(80) NOT NULL,
    MinAge INT NULL,
    MaxAge INT NULL,
    DistanceKm DECIMAL(6,2) NULL,
    CONSTRAINT FK_Categories_Events FOREIGN KEY (EventId) REFERENCES dbo.Events(EventId) ON DELETE CASCADE,
    CONSTRAINT CK_Categories_Age CHECK (MinAge IS NULL OR MinAge >= 0),
    CONSTRAINT CK_Categories_AgeRange CHECK (MinAge IS NULL OR MaxAge IS NULL OR MaxAge >= MinAge),
    CONSTRAINT CK_Categories_Distance CHECK (DistanceKm IS NULL OR DistanceKm > 0),
    CONSTRAINT UQ_Categories_Event_Name UNIQUE (EventId, Name)
);
CREATE TABLE dbo.Enrolments (
    EnrolmentId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Enrolments PRIMARY KEY,
    ParticipantId INT NOT NULL,
    EventId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolledAt DATETIME2 NOT NULL CONSTRAINT DF_Enrolments_EnrolledAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Enrolments_Participant FOREIGN KEY (ParticipantId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Enrolments_Event FOREIGN KEY (EventId) REFERENCES dbo.Events(EventId),
    CONSTRAINT FK_Enrolments_Category FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(CategoryId),
    CONSTRAINT UQ_Enrolments_Participant_Event UNIQUE (ParticipantId, EventId)
);
CREATE TABLE dbo.AuditLogs (
    AuditLogId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY,
    UserId INT NULL,
    Action NVARCHAR(100) NOT NULL,
    EntityName NVARCHAR(80) NOT NULL,
    EntityId INT NULL,
    Details NVARCHAR(1000) NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId) ON DELETE SET NULL
);
CREATE TABLE dbo.Results (
    ResultId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Results PRIMARY KEY,
    EnrolmentId INT NOT NULL CONSTRAINT UQ_Results_Enrolment UNIQUE,
    FinishTime TIME(3) NOT NULL,
    FinishPosition INT NOT NULL,
    RecordedAt DATETIME2 NOT NULL CONSTRAINT DF_Results_RecordedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentId) REFERENCES dbo.Enrolments(EnrolmentId),
    CONSTRAINT CK_Results_Position CHECK (FinishPosition > 0)
);
GO

/* Sample users use non-working placeholder hashes; create real users through API registration. */
INSERT INTO dbo.Users (FirstName, LastName, Email, PasswordHash, Role) VALUES
('Amina','Dlamini','amina.organiser@example.com','SEED_HASH_NOT_FOR_LOGIN','Organiser'),
('Thabo','Mokoena','thabo.organiser@example.com','SEED_HASH_NOT_FOR_LOGIN','Organiser'),
('Lerato','Nkosi','lerato.participant@example.com','SEED_HASH_NOT_FOR_LOGIN','Participant'),
('Sipho','Khumalo','sipho.participant@example.com','SEED_HASH_NOT_FOR_LOGIN','Participant');

INSERT INTO dbo.Events (OrganiserId, Name, Description, EventDate, Location, DistanceKm, EventType) VALUES
(1,'Johannesburg Spring Run','Community road-running event.','2027-09-12T07:00:00','Zoo Lake, Johannesburg',10,'Run'),
(2,'Golden Walk','Family-friendly charity walk.','2027-10-03T08:00:00','Emmarentia Dam, Johannesburg',5,'Walk'),
(1,'Cycle for Hope','Community cycling event.','2027-11-14T06:30:00','Kyalami, Midrand',21,'Cycle');

INSERT INTO dbo.Categories (EventId, Name, MinAge, MaxAge, DistanceKm) VALUES
(1,'Under 20',12,19,5),(1,'Senior',60,NULL,10),(1,'Open 10km',20,59,10),
(2,'Family',NULL,NULL,5),(2,'Senior Walk',60,NULL,5),(2,'Open Walk',18,59,5),
(3,'Junior Cycle',14,17,10),(3,'Open 21km',18,59,21),(3,'Veteran Cycle',60,NULL,21);

INSERT INTO dbo.Enrolments (ParticipantId, EventId, CategoryId) VALUES
(3,1,3),(4,1,1),(3,2,4),(4,3,8);

INSERT INTO dbo.Results (EnrolmentId, FinishTime, FinishPosition) VALUES
(1,'00:52:31.000',12),(2,'00:47:12.000',8);
GO

GO

/* Verification SELECT statements — run after the inserts above. */
SELECT UserId, FirstName, LastName, Email, Role, CreatedAt FROM dbo.Users ORDER BY UserId;
SELECT EventId, Name, EventDate, Location, DistanceKm, EventType FROM dbo.Events ORDER BY EventDate;
SELECT CategoryId, EventId, Name, MinAge, MaxAge, DistanceKm FROM dbo.Categories ORDER BY EventId, CategoryId;
SELECT EnrolmentId, ParticipantId, EventId, CategoryId, EnrolledAt FROM dbo.Enrolments ORDER BY EnrolmentId;
SELECT ResultId, EnrolmentId, FinishTime, FinishPosition, RecordedAt FROM dbo.Results ORDER BY FinishPosition;
SELECT a.AuditLogId, a.UserId, a.Action, a.EntityName, a.EntityId, a.CreatedAt FROM dbo.AuditLogs AS a ORDER BY a.CreatedAt DESC;

/* Joined view: participant, event, selected category and result. */
SELECT en.EnrolmentId, u.FirstName + N' ' + u.LastName AS ParticipantName,
       ev.Name AS EventName, ev.EventDate, cat.Name AS CategoryName,
       res.FinishTime, res.FinishPosition
FROM dbo.Enrolments AS en
INNER JOIN dbo.Users AS u ON u.UserId = en.ParticipantId
INNER JOIN dbo.Events AS ev ON ev.EventId = en.EventId
INNER JOIN dbo.Categories AS cat ON cat.CategoryId = en.CategoryId
LEFT JOIN dbo.Results AS res ON res.EnrolmentId = en.EnrolmentId
ORDER BY ev.EventDate, res.FinishPosition;
GO
