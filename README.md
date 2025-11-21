# ISU Tasks

A minimalist **Task Management** full-stack web application built with **Angular 20** (client) and **ASP.NET Core 9** (server), backed by a **PostgreSQL** database.

The project is fully containerized with Docker for easy local development and deployment.

## 📦 Running the App (via Docker)

From the project root:

```bash
$ docker compose up --build
```

Services exposed:

* **Client** → http://localhost:8080
* **API** → http://localhost:9090
* **Database** → localhost:25432

## 🛠 Running Locally (without Docker)

### Client (Angular)

```bash
$ cd Client
$ pnpm install
$ ng serve
```

Runs at: http://localhost:4200

### Server (.NET API)

```bash
$ cd Server
$ dotnet restore
$ dotnet run --project src/IsuTasks.Api
```

Runs at: http://localhost:9090

### Database (PostgreSQL)

You need a local PostgreSQL instance running.
Update the API connection string accordingly in `appsettings.json`.

## 📌 Pending

* Write client/server integration and unit tests.
* Filter tasks by name/description, due-date and status (isCompleted).
* Make client-application responsive and use Angular Material for styling it.
* Treat refresh-tokens as HTTP-only cookies to avoid Cross-Site Scripting (XSS) attacks.
* Improve server's CORS configuration to increase security.
* Hold API's url as a environment variable in the client-application.

## 📄 License

MIT
