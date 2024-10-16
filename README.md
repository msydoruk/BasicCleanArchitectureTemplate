# BasicCleanArchitectureTemplate

This project is a template for building applications using **Clean Architecture** principles. It’s designed to provide a clear separation of concerns between the core business logic and infrastructure or web layers. The solution is built on **.NET 7** and includes a **web project** as the front-end interface.

## Solution Structure

The solution consists of four projects:

1. **BasicCleanArchitectureTemplate.Core**
   - This is the core layer where the business logic resides.
   - It contains the core models, factories, services, and strategies essential to the application's domain.
   - Key folders:
     - **Factories**: Contains factory classes like `EventOccurrenceStrategyFactory.cs`.
     - **FluentValidation**: Holds validators for request validation, e.g., `EventFilterRequestValidator.cs`.
     - **Models**: Domain models such as `EventModel.cs`, `RecurrenceType.cs`.
     - **Strategies**: Implements various occurrence strategies like `SingleOccurrenceStrategy.cs`.

2. **BasicCleanArchitectureTemplate.Infrastructure**
   - This layer deals with external dependencies like data access and repository implementations.
   - Key folders:
     - **Data**: Contains database context and related configurations.
     - **Repositories**: Implements repository patterns for data access.

3. **BasicCleanArchitectureTemplate.Tests**
   - This project contains unit tests to ensure code quality and reliability.
   - Key folders:
     - **Controllers**: Contains tests for web controllers.
     - **Factories**: Contains tests for factory classes.
     - **Services**: Includes tests for services in the core project.

4. **BasicCleanArchitectureTemplate.Web**
   - This is the front-end project built with **ASP.NET Core** to handle web requests.
   - Includes:
     - **Connected Services**: For handling external services integrations.
     - **wwwroot**: Contains static files for the web application.

## Technology Stack
- **.NET 7**: The solution is built on .NET 7, leveraging its latest features and performance improvements.
- **Clean Architecture**: The solution follows Clean Architecture principles for maintainability and scalability.
- **FluentValidation**: For request validation across the web and API layers.
