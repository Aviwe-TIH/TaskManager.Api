# Task Manager REST API (.NET 10)

A lightweight RESTful CRUD API built with **ASP.NET Core** and **.NET 10**. This service manages task items using a thread-safe, in-memory repository without requiring an external database setup.

---

## Architecture & Request Flow

```
[ Client / Postman ]
         │
         │  HTTP Requests (JSON)
         ▼
┌──────────────────────────────────────────────┐
│            ASP.NET Core Middleware           │
│   Routing ──► Validation ──► Swagger / Auth   │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│           TasksController (API)              │
│       - Receives HTTP Verbs (GET/POST/PUT)   │
│       - Validates Model Attributes           │
│       - Dispatches to Repository Layer       │
└──────────────────────┬───────────────────────┘
                       │  Dependency Injection (Singleton)
                       ▼
┌──────────────────────────────────────────────┐
│            ITaskRepository                   │
│         InMemoryTaskRepository               │
│       - Thread-Safe (lock mechanism)         │
│       - In-Memory List<TaskItem>             │
└──────────────────────────────────────────────┘

```

---

## Tech Stack & Prerequisites

* **Runtime / SDK:** [.NET 10 SDK](https://dotnet.microsoft.com/download?utm_source=gemini)
* **Framework:** ASP.NET Core Web API (Controllers)
* **Language:** C# 13+
* **Data Storage:** In-Memory collection (`List<TaskItem>`)
* **API Testing & Documentation:** Swagger / OpenAPI, [Postman](https://www.postman.com/downloads/?utm_source=gemini)
* **Version Control:** Git

---

## Project Structure

```text
TaskManager.Api/
├── Controllers/
│   └── TasksController.cs          # HTTP request handlers & routing
├── Models/
│   ├── TaskItem.cs                 # Task entity & DataAnnotation validations
│   └── TaskPriority.cs             # Enum: Low, Medium, High
├── Services/
│   ├── ITaskRepository.cs          # Abstraction contract for repository
│   └── InMemoryTaskRepository.cs   # Thread-safe in-memory store
├── Properties/
│   └── launchSettings.json         # Development ports and URLs
├── appsettings.json                # Runtime configuration
├── Program.cs                      # Service registration (DI) & middleware pipeline
├── TaskManager.Api.csproj          # .NET build targets & packages
└── postman/                        # Exported Postman collection and environment

```

---

## Getting Started

### 1. Clone & Verify SDK

```bash
git clone <repository-url>
cd TaskManager.Api

# Verify that .NET 10 SDK is installed
dotnet --version

```

### 2. Scaffold (If starting from scratch)

```bash
dotnet new webapi -n TaskManager.Api -controllers
cd TaskManager.Api

```

### 3. Build & Run

```bash
# Restore dependencies and build
dotnet build

# Run application
dotnet run

```

The terminal will display the active listening URLs (e.g., `https://localhost:7123` or `http://localhost:5123`).

### 4. Interactive Documentation

When running in development mode, open your browser and navigate to:

```text
https://localhost:<port>/swagger

```

---

## API Reference

Base route: `/api/tasks`

| Method | Endpoint | Description | Status Codes |
| --- | --- | --- | --- |
| `GET` | `/api/tasks` | Retrieve all tasks | `200 OK` |
| `GET` | `/api/tasks?completed=true` | Filter tasks by completion status | `200 OK` |
| `GET` | `/api/tasks/{id}` | Retrieve a task by ID | `200 OK`, `404 Not Found` |
| `POST` | `/api/tasks` | Create a new task | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/tasks/{id}` | Update an existing task | `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/tasks/{id}` | Delete a task by ID | `204 No Content`, `404 Not Found` |

---

## Data Schema & Validation

### `TaskItem`

```json
{
  "id": 1,
  "title": "Prepare demo",
  "description": "Prepare the Week 4 walkthrough",
  "isCompleted": false,
  "priority": "High",
  "dueDate": "2026-10-01T00:00:00Z",
  "createdAt": "2026-09-22T13:13:22Z"
}

```

* `id` (`int`): Auto-incremented primary key, managed by the server.
* `title` (`string`): **Required**. Min length: 3, Max length: 100 characters.
* `description` (`string`, optional): Max length: 500 characters.
* `isCompleted` (`bool`): Default `false`.
* `priority` (`string` or `int`): Options: `Low` (0), `Medium` (1), `High` (2). Default `Medium`.
* `dueDate` (`DateTime?`, optional): ISO 8601 UTC timestamp.
* `createdAt` (`DateTime`): Assigned automatically at resource creation.

---

## Postman Setup & Automated Tests

### 1. Environment Configuration

Create a Postman Environment (e.g., `Local`) with the variable:

* `baseUrl`: `https://localhost:<port>` (matches your `launchSettings.json` or console URL)

### 2. Request Payloads

#### Create Task (`POST /api/tasks`)

* **Headers:** `Content-Type: application/json`
* **Body:**

```json
{
  "title": "Build the Tasks API",
  "description": "Implement CRUD controllers and unit tests",
  "priority": "High",
  "dueDate": "2026-10-01T00:00:00Z"
}

```

* **Tests script (`Tests` tab):**

```javascript
pm.test("Status code is 201 Created", function () {
    pm.response.to.have.status(201);
});

var jsonData = pm.response.json();
pm.test("Task contains valid generated ID", function () {
    pm.expect(jsonData.id).to.be.a('number');
});

// Cache dynamic ID for subsequent PUT/DELETE requests
pm.environment.set("createdTaskId", jsonData.id);

```

#### Update Task (`PUT /api/tasks/{{createdTaskId}}`)

* **Headers:** `Content-Type: application/json`
* **Body:**

```json
{
  "title": "Build the Tasks API (Completed)",
  "description": "Implement CRUD controllers and unit tests",
  "isCompleted": true,
  "priority": "High",
  "dueDate": "2026-10-01T00:00:00Z"
}

```

* **Tests script (`Tests` tab):**

```javascript
pm.test("Status code is 204 No Content", function () {
    pm.response.to.have.status(204);
});

```

### 3. Automated Test Suite Execution

Run the entire test suite using Postman Collection Runner to ensure end-to-end reliability:

```text
1. Open Postman -> Select 'Task Manager API' Collection
2. Click 'Run collection'
3. Verify test runs cover both Happy Paths (200, 201, 204) and Error States (400, 404)

```

---

## Troubleshooting

* **`dotnet: command not found`:** Verify `.NET 10 SDK` is installed and the directory is added to your system `PATH`. Restart your terminal session after installation.
* **Port conflicts on startup:** Change the assigned ports inside `Properties/launchSettings.json` under `applicationUrl`.
* **Data lost between restarts:** By design, the repository uses a singleton in-memory list (`List<TaskItem>`). Shutting down or restarting the process flushes volatile memory.
