# Todo App

## Project Description
This is my first ASP.NET Core web API practice project - a simple Todo application which allows users to sign up, manage their accounts and perform CRUD operations on their todos.

I've kept the domain logic of the application very simple on purpose, as the main goal of creating it was to familiarize myself with best practices, patterns and libraries used to create web APIs with ASP.NET Core. Here's what I've tried to implement and use:
- **Clean Architecture folder and solution structure**:
  - The root folder of the project contains separate folders for source code, tests and documentation
  - The solution includes separate projects for each of Clean Architecture layers: Domain, Application, Infrastructure and Presentation. The web API represents the Presentation layer, other layers are represented by class libraries. Each project has it's own dependency injection configuration to keep the program.cs file neat
- **CQRS pattern**:
  - API requests are mapped to either command or query records depending on the action. I've used Mapster library for the mapping to try it out, but it obviously could be done manually in such a simple project
- **Mediator pattern**:
  - I used MediatR library to send commands and queries to appropriate handlers in the Application layer
- **FluentValidation**:
  - I've implemented a generic validation MediatR pipeline behavior to validate commands and queries with FluentValidation library before they are executed
- **Repository Pattern**:
  - I've handled data persistence with Repository pattern and handled data access with Dapper in the concrete implementations of my repositories
- **JWT-token based user authentication**:
  - I created a custom authentication system using built-in ASP.NET Core tools, including password hashing and salting. I recognize that production apps should rely on 3rd party authentication providers for security, but it was out of the scope of this project
- **Rate limiting**:
  - I've used AspNetCoreRateLimit library to implement rate limiting and configured it to allow 3 requests each 5 seconds for any client for the sake of testing
- **Healthchecks**:
  - I've created a custom healthcheck to verify that the database is functioning and added AspNetCore.Healthchecks.UI.Client to provide more information
- **Logging**:
  - I've added logging with Serilog and configured it in appsettings.json to log both to console and .txt file
- **Testing**:
  - I used xUnit and Moq to create unit tests for my user-related command/query handlers in the application layer

The project turned out massively over-engineered for what it is (as expected :) ), but that allowed me to get a grip of patterns and libraries used to create modern ASP.NET Core web APIs. While the app certainly can be improved in many aspects, I am quite happy wh

## Running and using the app

The app can be run in a Docker container along with a Postgresql database via the included docker-compose file:

1. Clone the repo
2. Edit the appsettings.json file in the TodoApi project to configure JWT-token authentication and database connection string
3. Edit Postgresql credentials in the docker-compose file
4. Build the images and run the containers with 

```bash
docker-compose up
```
5. Test the endpoints using Postman or similar tools, information on API contracts can be found in the [docs](docs). Be sure to create a user first as all endpoints require authentication.
