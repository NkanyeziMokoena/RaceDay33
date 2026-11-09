# RaceDay Entity Relationship Diagram (Part 1, Section A)

```mermaid
erDiagram
    USERS ||--o{ EVENTS : organises
    EVENTS ||--o{ CATEGORIES : defines
    USERS ||--o{ ENROLMENTS : participates
    EVENTS ||--o{ ENROLMENTS : receives
    CATEGORIES ||--o{ ENROLMENTS : selected_for
    ENROLMENTS ||--o| RESULTS : has
    USERS o|--o{ AUDITLOGS : triggers

    USERS {
        int UserId PK
        nvarchar FirstName
        nvarchar LastName
        nvarchar Email UK
        nvarchar PasswordHash
        nvarchar Role
        datetime2 CreatedAt
    }
    EVENTS {
        int EventId PK
        int OrganiserId FK
        nvarchar Name
        nvarchar Description
        datetime2 EventDate
        nvarchar Location
        decimal DistanceKm
        nvarchar EventType
        datetime2 CreatedAt
    }
    CATEGORIES {
        int CategoryId PK
        int EventId FK
        nvarchar Name
        int MinAge
        int MaxAge
        decimal DistanceKm
    }
    ENROLMENTS {
        int EnrolmentId PK
        int ParticipantId FK
        int EventId FK
        int CategoryId FK
        datetime2 EnrolledAt
    }
    AUDITLOGS {
        int AuditLogId PK
        int UserId FK
        nvarchar Action
        nvarchar EntityName
        int EntityId
        nvarchar Details
        datetime2 CreatedAt
    }
    RESULTS {
        int ResultId PK
        int EnrolmentId FK_UK
        time FinishTime
        int FinishPosition
        datetime2 RecordedAt
    }
```

## Relationship and design notes
- One organiser can create many events; each event has exactly one organiser.
- One event has many categories. Categories are event-specific.
- Users with the Participant role can have many enrolments; each enrolment links one participant, one event and one category.
- AuditLogs stores a record of important system actions; a log may optionally be associated with a user.
- One enrolment has zero or one result. A unique constraint on `Results.EnrolmentId` prevents duplicate results.
- `Users.Email` is unique. Roles are constrained to `Organiser` or `Participant`.
- `Enrolments` has a unique `(ParticipantId, EventId)` constraint to prevent duplicate entries in the same event.
- The SQL schema and the API model names are designed to match this diagram.
