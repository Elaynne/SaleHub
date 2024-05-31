# Overview
SalesHub is a .NET 6 application to manage a products catalog (Books), designed following Clean Architecture principles to ensure a maintainable, testable, and scalable codebase. This project leverages several libraries to facilitate various functionalities such as mediation, JWT authentication, and unit testing. It also uses caching to abstract the database layer, enhancing performance and testability.

![image](https://github.com/Elaynne/SaleHub/assets/9860373/7140695d-df88-487e-9a33-49e96ed71833)

## Architecture
Clean Architecture is employed to separate concerns and organize the codebase into distinct layers:

Domain Layer: Contains the core business logic and domain models.

Application Layer: Contains use cases (Manage Users, Orders, and Books) interfaces, and DTOs.

Infrastructure Layer: Contains data access, external services, and other infrastructure-related code.

Presentation Layer: Contains the API controllers and user interface (if any).

Layers and Responsibilities
Domain: The heart of the application, containing entities and business rules.

Application: Implements the business logic orchestrating the domain entities and the interfaces for interacting with the external world.

Infrastructure: Provides implementations for the interfaces defined in the Application layer, including data repositories and external service integrations.

Presentation: Exposes the application to the users, typically through a web API.

## Library Choices

### MediatR
MediatR is used to implement the Mediator pattern, promoting loose coupling between components. It helps in handling commands and queries in a clean and decoupled manner.

### Microsoft.IdentityModel.JsonWebTokens & System.IdentityModel.Tokens.Jwt
These libraries provide support for handling JSON Web Tokens (JWT), which are used for authentication and authorization in the application.

### System.Security.Cryptography
Provides algorithms to password encription

### Microsoft.AspNetCore.Authentication.JwtBearer
This middleware is used for JWT authentication, enabling secure access to the API endpoints.

### Microsoft.Extensions.Caching
Caching is implemented using Microsoft.Extensions.Caching, which provides abstractions for in-memory caching.

### FluentAssertions
FluentAssertions is used for writing expressive and readable assertions in unit tests, making the tests more understandable.

### NSubstitute
NSubstitute is used for creating mock objects in unit tests, allowing for easy mocking of dependencies and verifying interactions.

### xUnit
xUnit is the testing framework used for writing and running unit tests. It supports parallel testing and integrates well with other .NET tools.

## Caching and Database Abstraction
The application uses caching to abstract the database, enhancing performance and testability. The cache acts as an intermediate storage layer, reducing the need for frequent database access. This approach also simplifies unit testing by allowing tests to interact with the cache instead of the actual database.

# Getting Started

Clone the repository.

Install the required packages using dotnet restore.

Configure the necessary settings in appsettings.json.

Run the application using dotnet run.

Run the tests using dotnet test.

# How to send order

This solution has a mock of 3 users:

1 Admin: name user-admin password 11111111

2 Seller: name user-seller password 22222222

3 Client: name user-client password 33333333

Mock of 10 books of JRR Tolking

1 - Login as and ADMIN. (user-admin)
```json
curl -X 'POST' \
  'https://localhost:7265/api/users/login' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "username": "user-admin",
  "password": "11111111"
}'
```

2 - Retrieve the JWToken to authenticate on swagger: "Bearer JWToken_value"

3 - Get Products will return a starter mock stored in-memory. Select one Id.
```json
curl -X 'GET' \
  'https://localhost:7265/api/products' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer JWToken_value
```
output
```json
[
  {
    "title": "The Hobbit",
    "author": "J.R.R. Tolkien",
    "isbn": "978-0547928227",
    "id": "a0fdf7bc-afc8-4f11-ab5a-350db6b04c5d",
    "description": "A fantasy novel and children's book by J.R.R. Tolkien, follows the quest of home-loving Bilbo Baggins.",
    "stock": 120,
    "price": 15.99,
    "costPrice": 8
  },
...
]
```
4 - Get users to select one UserId.
```json
curl -X 'GET' \
  'https://localhost:7265/api/users' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer JWToken_value
```
5 - Send order
Input
```json
curl -X 'POST' \
  'https://localhost:7265/api/orders' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer JWToken_value' \
  -H 'Content-Type: application/json' \
  -d '{
  "cLientId": "de4711ce-3fb2-4050-a483-936b364fd60f",
  "orderItems": [
    {
      "bookId": "a0fdf7bc-afc8-4f11-ab5a-350db6b04c5d",
      "quantity": 10
    }
  ]
}'
```
Output
```json
{
  "orderId": "2144eb2d-2268-44ac-9278-9a682550e3c1",
  "clientId": "de4711ce-3fb2-4050-a483-936b364fd60f",
  "sellerId": "a17ad52f-8720-492e-af21-b08514ea3e48",
  "orderItems": [
    {
      "bookId": "a0fdf7bc-afc8-4f11-ab5a-350db6b04c5d",
      "quantity": 10
    }
  ],
  "createdAt": "2024-05-30T23:40:35.8751444-03:00",
  "totalPrice": 159.9,
  "status": 0,
  "orderStatusDescription": "Pending",
  "updatedAt": "2024-05-30T23:40:35.8751444-03:00"
}
```

6 - Get Orders will return an empty list on the 1st time, before you populate as you wish.

7 - Delete Order to cancel order by id. You can only cancel orders with Pending status. ALl orders are created as Pending, assuming it could be processed asynchronously by another application.

OrderStatus [**Pending, Processing, Shipped, Delivered, Cancelled**]

## TO-DO
* [ ] Implement code coverage on: OrdersController and ProductsController, OrderRepository, ProductRepository, Books use cases
* [ ] Create Forgot Password endpoint and send email to recover
* [ ] Improve logs and exceptions
* [ ] Create Admin Dashboards (top sellers, monthly proffit, orders status
* [ ] Implement Worker to process orders assync with queues
* [ ] Publish
