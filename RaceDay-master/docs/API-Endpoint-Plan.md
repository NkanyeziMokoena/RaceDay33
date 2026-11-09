# RaceDay API Endpoint Plan (Part 1, Section B)

Roles: **Public** = no authentication; **Any logged-in user** = authenticated Organiser or Participant. Organiser self-registration is intentionally not permitted so a public caller cannot grant themselves organiser privileges. An existing organiser can provision organiser accounts through a controlled administrative process.

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/auth/register | Create an account; public registration allows Participant only. Organiser accounts are provisioned by an existing organiser/admin process. | None (public) | { firstName, lastName, email, password } | 201 Created; 400 validation; 409 email exists |
| POST | /api/auth/login | Validate credentials and create a server-side session and HttpOnly session cookie. | None (public) | { email, password } | 200 OK with role/user details and session cookie; 401 invalid credentials |
| GET | /api/auth/session | Report whether a valid session exists and return the current user ID and role. | Any logged-in user | None | 200 OK; 401 no session |
| POST | /api/auth/logout | End the current authenticated session. | Any logged-in user | None | 200 OK; 401 unauthenticated |
| GET | /api/users/me | Return the current user's profile. | Any logged-in user | None | 200 OK; 401 unauthenticated |
| PUT | /api/users/me | Update the current user's profile; role and password are not changed here. | Any logged-in user | { firstName, lastName, email } | 200 OK; 400 invalid data; 409 duplicate email |
| GET | /api/events | List events; both roles can browse. | Any logged-in user | None | 200 OK; 401 unauthenticated |
| GET | /api/events/{id} | Get one event and its public category list. | Any logged-in user | None | 200 OK; 404 not found |
| POST | /api/events | Create an event owned by the current organiser. | Organiser | { name, description, eventDate, location, distanceKm, eventType } | 201 Created; 403 wrong role; 400 invalid data |
| PUT | /api/events/{id} | Update an event owned by the organiser. | Organiser | Event fields | 200 OK; 403 wrong role/not owner; 404 not found |
| DELETE | /api/events/{id} | Delete an event and its categories; enrolment constraints may prevent deletion if history exists. | Organiser | None | 204 No Content; 403 wrong role; 404 not found; 409 linked records |
| GET | /api/events/{eventId}/categories | List categories for an event. | Any logged-in user | None | 200 OK; 404 event not found |
| POST | /api/events/{eventId}/categories | Add an age/distance category to an event. | Organiser | { name, minAge?, maxAge?, distanceKm? } | 201 Created; 403 wrong role; 404 event not found |
| PUT | /api/categories/{id} | Update a category. | Organiser | { name, minAge?, maxAge?, distanceKm? } | 200 OK; 403 wrong role; 404 not found |
| DELETE | /api/categories/{id} | Delete a category if it has no enrolments. | Organiser | None | 204 No Content; 409 category in use |
| POST | /api/events/{eventId}/enrolments | Enrol the current participant in a category for the event. | Participant | { categoryId } | 201 Created; 403 wrong role; 404 event/category; 409 duplicate enrolment |
| GET | /api/events/{eventId}/enrolments | View all enrolments for an event managed by the current organiser. | Organiser | None | 200 OK; 403 not owner; 404 event not found |
| GET | /api/users/me/enrolments | List the current participant's enrolments. | Participant | None | 200 OK; 403 wrong role |
| POST | /api/enrolments/{id}/result | Record a participant's result for an event enrolment. | Organiser | { finishTime, finishPosition } | 201/200 OK; 403 not owner; 404 enrolment; 409 result exists |
| GET | /api/users/me/results | List results for the current participant. | Participant | None | 200 OK; 403 wrong role |
