# EventSourcingDotnet
This project demonstrates an Event Sourcing implementation using .NET Core Web API, focusing on capturing state changes as events and maintaining read-optimized views through projections. It adheres to Clean Architecture principles for enhanced maintainability and separation of concerns.

## Overview
Event Sourcing captures every state change in the system as an immutable event, which is then stored in an event store. 
This approach preserves a detailed history of entity transformations over time and enables the reconstruction of past states when needed.

In this project, Event Sourcing is integrated with the following key components:
- **Event Store**: Acts as a central repository where all events are stored, preserving a chronological history of entity state changes.
- **Projections**: Read-optimized views of the current entity state derived from events stored in the event store. These projections are typically stored in a separate database optimized for query efficiency, ensuring fast and responsive query operations. The 'product collection' in MongoDB likely contains these read-optimized views (projections) of the current state of product entities.

## Development Prerequisites
Before diving into development with this project, ensure you have the following prerequisites:

- **Development Environment**: Either Visual Studio 2022 (IDE) or Visual Studio Code (Source-code editor)
- **.NET 8**: Required framework version for building and running the project
- **Docker Desktop**: Required for running MongoDB Server and Mongo Express (web-based administrative interface for MongoDB)

## Getting Started
To run the API locally, follow these steps:

1. Clone this repository to your local machine.
2. Ensure you have Docker installed and running.
3. Open a shell and navigate to the tools folder within the cloned repository.
4. Run the following command to start MongoDB and Mongo Express containers in detached mode: 
   ```bash
   docker compose up -d
5. Once the containers are running, you can connect to Mongo Express by visiting http://localhost:8081 in your web browser. Please note that the default basicAuth credentials in Mongo Express are "admin:pass". It is highly recommended to change these credentials to ensure the security of your application.
6. Build and run the project using the appropriate commands or methods for your development environment.

## Testing
Unit tests are included to validate the correctness of the application logic. Run these tests to ensure everything functions as expected.

## Screenshots

### Swagger Documentation
![Swagger Documentation](https://github.com/gsherwin360/EventSourcingDotnet/assets/17651320/a586016d-d435-410a-9f06-14d1bdf40bd8)

---
### MongoExpress (with Example Events)
![MongoExpress Screenshot 1](https://github.com/gsherwin360/EventSourcingDotnet/assets/17651320/46030ceb-7efd-4e91-876e-194e23de8a31)
![MongoExpress Screenshot 2](https://github.com/gsherwin360/EventSourcingDotnet/assets/17651320/100a6f96-4ecf-499a-9154-8e435c46b277)
![MongoExpress Screenshot 3](https://github.com/gsherwin360/EventSourcingDotnet/assets/17651320/c9ff4f3e-e517-4045-ace1-fcbd25564c37)

---
### Docker
![Docker Screenshot](https://github.com/gsherwin360/EventSourcingDotnet/assets/17651320/068e4c57-e214-45cf-b25e-a393fc2bb42b)
