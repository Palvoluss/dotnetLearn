# FinShark API

This is the backend API for the FinShark project. Follow the instructions below to set up and run the API locally.

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- PostgreSQL database (see instructions below)

## Setting Up the Database

Before starting the API server, you need to have a PostgreSQL database running. You can use Docker for a simple setup:

```bash
docker run --name my_postgres_container \
  -e POSTGRES_DB=MyDatabase \
  -e POSTGRES_USER=myuser \
  -e POSTGRES_PASSWORD=mypassword \
  -p 5432:5432 \
  -d postgres
```

This command will:
- Create a PostgreSQL container named `my_postgres_container`
- Set up a database named `MyDatabase`
- Create a user `myuser` with password `mypassword`
- Map port 5432 of the container to port 5432 on your host machine

## Running the API

Once the database is up and running, you can start the API server in development mode:

```bash
# Navigate to the api directory (if not already there)
cd api

# Start the API with hot reload
dotnet watch run
```

The API will be available at:
- HTTP: http://localhost:5281
- HTTPS: https://localhost:7055

You can access the Swagger UI at `/swagger` endpoint to explore and test the API.

## Database Migrations

If you need to apply database migrations:

```bash
dotnet ef database update
```

To create a new migration:

```bash
dotnet ef migrations add MigrationName
``` 