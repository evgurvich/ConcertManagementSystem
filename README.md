# Concert Management System

A simple web-based ticketing system built with ASP.NET Core Web API. The system allows users to create and manage concert events, define ticket types, and handle ticket reservations and purchases.

## Overview

This API supports:

- Event creation, editing, and listing  
- Ticket type management (price, quantity)  
- Ticket reservation with expiration  
- Ticket purchase with simulated payment  
- Reservation cancellation  
- Real-time availability reporting  

All data is stored in memory for the scope of this assignment.

## Assumptions

- Tickets are for general admission (non-seat-assigned).  
- Each reservation request is for a single ticket only.  
- Reservations expire after **15 minutes**.  
- Ticket reservation and purchase are available only if the event exists and capacity allows.  
- Expired or cancelled tickets are returned to the available pool.  
- Payment is simulated via a mock `IPaymentProcessingService` implementation.  
- There is no authentication, authorization, or rate limiting.  
- Validation for event and ticket creation is enforced via data annotations.  

## Default Constraints

### CreateEventRequest

- `Name`: required, max 100 characters  
- `Venue`: optional, max 500 characters  
- `Description`: optional, max 1000 characters  
- `TotalCapacity`: 1 to 100,000  

### CreateTicketTypeRequest

- `Name`: required, max 100 characters  
- `Price`: between 0.01 and 100,000  
- `TotalQuantity`: 1 to 100,000  

## Technologies

- .NET 8 Web API  
- Swagger (Swashbuckle)  
- xUnit + Moq for testing  
- No external database (in-memory only)  

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Run the API

```bash
dotnet run --project ConcertManagementSystem.Api
```

Visit Swagger UI: [http://localhost:5067/swagger](http://localhost:5067/swagger)

### Run Tests

```bash
dotnet test --logger "console;verbosity=detailed"
```
## Design Decisions

- **Separation of Concerns:**  
  Business logic for ticketing is encapsulated in `TicketService`, making it easy to test and reuse.  
  Event logic is currently in `EventController`, but could be moved to a dedicated service for better follow separation of concerns and improve testability.

- **Repository Abstraction:**  
  In-memory repositories are injected using interfaces, allowing easy swap-in of a real database in future.

- **Validation:**  
  Data annotations are used for validating API requests, enforcing business rules with minimal code.

- **API Documentation:**  
  Swagger UI is enabled for testing and exploring endpoints.


## Future Improvements

This project was developed under time constraints and currently implements the core features required by the assignment. However, there are several areas that could be enhanced in the future:

### Testing

- Expand unit test coverage across controllers and services.  
- Add tests for edge cases (e.g., reservation overbooking, expired tickets).  
- Introduce integration tests for validating complete workflows.  

### Architecture and Maintainability

- Move business logic out of `EventController` into a dedicated `EventService` to better follow separation of concerns and improve testability.  
- Add middleware for centralized logging and error handling. 
- Add a `TicketFactory` to encapsulate ticket creation logic. 

### Infrastructure and Security

- Replace in-memory storage with a real database.
- Add authentication and authorization.
  - Ensure that **only the user who reserved a ticket can purchase it**.
- Implement rate limiting to prevent abuse.

### Functional Enhancements

- Improve event capacity managment:
  - Add API for updating ticket types, including TotalQuantity, with validation to prevent reducing it below reserved or sold tickets. 
  - Derive event capacity entirely from ticket types to eliminate mismatch.
  - Add PATCH method support to allow partial updates for better flexibility. This is especially useful when only one property needs to be changed without resending the entire payload.
- Add support for bulk ticket reservations (e.g., reserve multiple tickets in one request).  
- Implement seat selection logic for events with assigned seating.  
- Add administrative endpoints for event reporting and management.  