# Leave Management System

The Leave Management System is a web application built using ASP.NET Core MVC, to automate the process of managing employee leave requests and approvals. This project was developed as part of my learning journey in ASP.NET Core MVC.

## Technologies Used

The Leave Management System utilizes the following technologies and frameworks:

- **ASP.NET Core MVC:** The web application framework used for building the user interface and handling HTTP requests.

- **ASP.NET Core Identity:** Used for managing users, roles, and authentication in ASP.NET Core applications. 

- **C#:** The primary programming language.

- **Entity Framework Core:** The Object-Relational Mapping (ORM) framework used for database operations, providing a convenient way to interact with the database using C#.

- **SQL Server:** The relational database management system used for storing application data.

- **HTML/CSS:** The standard markup and styling languages used for structuring and styling the user interface.

- **TailwindCSS:** A utility-first CSS framework used for rapidly building custom user interfaces. It offers a wide range of pre-designed components and utility classes for styling.

- **xUnit:** The unit testing framework used for writing and executing unit tests in C# projects. xUnit provides a simple and extensible approach to unit testing, allowing developers to verify the correctness of their code.

- **AutoFixture:** A library used for generating test data in unit tests, helping to simplify the setup of test scenarios.

- **Moq:** A mocking framework used for creating mock objects in unit tests, allowing for isolation of the code under test.

- **Fluent Assertions:** A fluent assertion library used for writing more readable and expressive unit test assertions.

- **Node.js and npm (optional):** Node.js is used as a JavaScript runtime environment, and npm is the package manager for installing and managing JavaScript packages. These tools are optional and are only required if using TailwindCSS for styling.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Testing](#testing)

## Features

### Admin
- Login
- Register new users
- View users
- Add new leave types
- View leave types
- Add leave requests
- View leave requests
- Approve or reject leave requests
- Delete leave requests

### Employee
- Register
- Login
- Apply for leave
- View leave history

## Architecture

The project follows clean architecture principles with the following layers:
- Core: Contains business logic and domain entities.
- Infrastructure: Implements data access using Entity Framework Core.
- Presentation: Contains ASP.NET Core MVC controllers, views.

## Testing

Unit tests are written for services using AutoFixture, Moq, and Fluent Assertions.

