# Library Management System API

## Assessment Overview

This project was created as a solution to the following problem statement for a senior developer assessment.

**Problem Statement:** Model a basic library management system API.

1.  **Build C# models / entities:**
    *   The `Domain/Entities` folder contains the core models: `Library`, `Book`, `Member`, and `BorrowRecord`.
2.  **Build database design based on above entities:**
    *   The database schema is defined using Entity Framework Core configurations in the `Infrastructure/Configurations` folder. These configurations establish primary keys, relationships (one-to-many, etc.), and constraints.
3.  **What are the endpoints required for this use case?**
    *   The API provides full CRUD functionality for all major resources. See the API Endpoints Overview section for a detailed list.
4.  **Implement the solution based on Object Oriented Programming (OOP) concept:**
    *   The solution is built using Clean Architecture, which heavily relies on OOP principles like abstraction (repository interfaces), encapsulation (entities protecting their state), and polymorphism. The separation into layers (Domain, Application, Infrastructure, API) demonstrates a strong adherence to SOLID principles.

---

## OOP and SOLID Principles in Practice

This project is a practical application of Object-Oriented Programming (OOP) and SOLID principles, which are foundational to its Clean Architecture.

### Object-Oriented Programming (OOP)

*   **Abstraction:** The `Domain` layer defines repository interfaces (e.g., `IBookRepository`, `IMemberRepository`) that abstract away the data persistence details. The `Application` layer depends on these abstractions, not on concrete implementations, allowing for flexibility and decoupling.
*   **Encapsulation:** Each layer encapsulates its responsibilities. For example, the `Infrastructure` layer encapsulates all data access logic, while the `Application` layer encapsulates the business rules and use cases (commands/queries).
*   **Inheritance:** Custom exceptions like `BookAlreadyBorrowedException` inherit from a base `DomainException`, allowing for centralized and specific error handling in the `GlobalExceptionHandlerMiddleware`.
*   **Polymorphism:** The MediatR pipeline behaviors (`IPipelineBehavior`) demonstrate polymorphism. The `ValidationBehaviour` can process any request that flows through the pipeline, regardless of its specific type.

### SOLID Principles

*   **S - Single Responsibility Principle (SRP):** Each class has a single, well-defined purpose.
    *   **Controllers** handle HTTP requests.
    *   **Command/Query Handlers** execute a single use case.
    *   **Repositories** manage data for a single entity.
    *   **Validators** validate a single command.

*   **O - Open/Closed Principle (OCP):** The system is open for extension but closed for modification.
    *   New features, like a logging behavior, can be added to the MediatR pipeline without modifying existing handlers.

*   **L - Liskov Substitution Principle (LSP):** Any implementation of a repository interface (e.g., for EF Core or a mock in a test) can be substituted without breaking the application.

*   **I - Interface Segregation Principle (ISP):** Interfaces are small and focused. For example, `IBookRepository` only contains methods relevant to books, preventing classes from depending on methods they don't use.

*   **D - Dependency Inversion Principle (DIP):** This is the core of Clean Architecture. High-level modules (`Application`) do not depend on low-level modules (`Infrastructure`). Both depend on abstractions (`Domain` interfaces). This is why all dependency arrows point inwards toward the `Domain` layer.

---

## Data Model and Relationships

The domain model is designed to capture the core relationships within a library system.

*   **Library to Book/Member (One-to-Many):** A `Library` can have many `Book`s and many `Member`s. This is a straightforward one-to-many relationship.

*   **Book and Member (Many-to-Many):** A `Book` can be borrowed by many `Member`s over time, and a `Member` can borrow many `Book`s. This creates a many-to-many relationship.

### The `BorrowRecord` Entity

To manage the many-to-many relationship between `Book` and `Member`, a dedicated linking entity, `BorrowRecord`, is used. This is a key design decision for several reasons:

1.  **Rich Relationship Data:** A simple linking table would only store `BookId` and `MemberId`. The `BorrowRecord` entity enriches this relationship by storing crucial contextual information, such as `BorrowedAt` and `ReturnedAt`. This turns the "act of borrowing" into a first-class citizen in our domain.
2.  **Historical Tracking:** It provides a full history of every borrowing transaction, which is essential for features like viewing a member's borrow history or a book's circulation history.
3.  **Clearer Domain Logic:** It allows us to model business rules directly related to the act of borrowing, such as checking if a book is currently available or calculating overdue fines.

This project is a RESTful API for a Library Management System, built with .NET 9 and C#, following the principles of Clean Architecture. It provides a well-structured and decoupled foundation for managing libraries, books, members, and borrowing records.

## Architecture

The solution is structured using **Clean Architecture**, which emphasizes a separation of concerns and makes the system more maintainable, testable, and independent of external frameworks or databases.

### Architectural Diagram

The dependencies flow inwards, with the core business logic (Domain) at the center, having no dependencies on any other layer.

```
+-----------------------------------------------------------------+
|                           Presentation (API)                    |
|        [Controllers, Middleware, Swagger, Program.cs]           |
|                           (ASP.NET Core)                        |
+---------------------------------v-------------------------------+
                                  |
+---------------------------------v-------------------------------+
|                         Application Layer                       |
|   [Commands, Queries, Handlers, DTOs, Validators, Exceptions]   |
|                            (MediatR)                            |
+---------------------------------v-------------------------------+
                                  |
+---------------------------------v-------------------------------+
|                           Domain Layer                          |
|              [Entities, Interfaces, Domain Exceptions]          |
|                       (Core Business Logic)                     |
+---------------------------------^-------------------------------+
                                  |
+---------------------------------^-------------------------------+
|                       Infrastructure Layer                      |
|      [DbContext, Repositories, EF Core Configurations]          |
|                       (Data Access, External)                   |
+-----------------------------------------------------------------+
```

### Layers

*   **Domain**: The core of the application. It contains all the business entities (e.g., `Book`, `Member`), domain-specific exceptions, and repository interfaces. This layer has zero dependencies on any other part of the application.

*   **Application**: This layer orchestrates the domain logic. It contains the application-specific business rules, implemented using the CQRS pattern with MediatR. It defines commands (for writing data), queries (for reading data), and their respective handlers. It depends only on the Domain layer.

*   **Infrastructure**: This layer contains the implementations for external concerns, primarily data access. It implements the repository interfaces defined in the Domain layer using Entity Framework Core. It depends on the Domain and Application layers.

*   **API (Presentation)**: The entry point to the application. This is an ASP.NET Core Web API project that contains the controllers, middleware, and dependency injection setup. It handles HTTP requests and responses, delegating all work to the Application layer.

## Getting Started

### Prerequisites

*   .NET 9 SDK

### Build and Run

1.  **Clone the repository**

2.  **Navigate to the solution directory**
    ```bash
    cd LibraryManagement
    ```

3.  **Build the solution**
    This will restore all NuGet packages and compile the projects.
    ```bash
    dotnet build
    ```

4.  **Run the API**
    Navigate to the API project folder and run the application.
    ```bash
    cd API
    dotnet run
    ```

The API will start and listen on the configured ports (e.g., `http://localhost:5115`). You can access the Swagger UI for interactive API documentation at `http://localhost:5115/swagger`.

## API Endpoints Overview

The API is organized by resources, with dedicated controllers for each.

### Libraries (`/v1/libraries`)
*   `POST /` - Creates a new library.
*   `GET /` - Gets a list of all libraries.
*   `GET /{libraryId}` - Gets a specific library by its ID.
*   `PUT /{libraryId}` - Updates an existing library.
*   `DELETE /{libraryId}` - Deletes a library.
*   `GET /{libraryId}/books` - Gets all books within a specific library.
*   `GET /{libraryId}/members` - Gets all members of a specific library.

### Books (`/v1/books`)
*   `POST /` - Adds a new book.
*   `GET /` - Gets a paginated list of all books.
*   `GET /search` - Searches for books by title.
*   `GET /{bookId}` - Gets a specific book by its ID.
*   `PUT /{bookId}` - Updates an existing book.
*   `DELETE /{bookId}` - Deletes a book.
*   `GET /{bookId}/borrow-history` - Gets the full borrowing history for a specific book.

### Members (`/v1/members`)
*   `POST /` - Registers a new member.
*   `GET /` - Gets a paginated list of all members.
*   `GET /{memberId}` - Gets a specific member by their ID.
*   `PUT /{memberId}` - Updates an existing member.
*   `DELETE /{memberId}` - Deletes a member.
*   `GET /{memberId}/borrowed-books` - Gets a list of books currently borrowed by a member.
*   `GET /{memberId}/borrow-history` - Gets the full borrowing history for a member.

### Borrowing (`/v1/borrow`)
*   `POST /` - Borrows a book for a member, creating a new borrow record.
*   `GET /{id}` - Gets a specific borrow record by its ID.
*   `PATCH /{id}/return` - Marks a borrowed book as returned.

## Testing

The solution includes a `UnitTests` project that contains both unit and integration tests.

*   **Unit Tests**: Located in `UnitTests/Application`, these tests use Moq to verify the logic of individual command and query handlers in isolation.
*   **Integration Tests**: Located in `UnitTests/API`, these tests use `WebApplicationFactory` to test the full API request pipeline, from the controller down to the in-memory database.

To run all tests, navigate to the solution root and execute:

```bash
dotnet test
```