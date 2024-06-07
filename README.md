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

- **TailwindCSS (optional):** A utility-first CSS framework used for rapidly building custom user interfaces. It offers a wide range of pre-designed components and utility classes for styling.

- **xUnit:** The unit testing framework used for writing and executing unit tests in C# projects. xUnit provides a simple and extensible approach to unit testing, allowing developers to verify the correctness of their code.

- **AutoFixture:** A library used for generating test data in unit tests, helping to simplify the setup of test scenarios.

- **Moq:** A mocking framework used for creating mock objects in unit tests, allowing for isolation of the code under test.

- **Fluent Assertions:** A fluent assertion library used for writing more readable and expressive unit test assertions.

- **Node.js and npm (optional):** Node.js is used as a JavaScript runtime environment, and npm is the package manager for installing and managing JavaScript packages. These tools are optional and are only required if using TailwindCSS for styling.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Installation](#installation)
- [Usage](#usage)
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

## Installation

Follow these steps to set up the Leave Management System on your local machine:

1. **Clone the Repository:**
    ```sh
    git clone https://github.com/salihahzakaria/Leave-Management-With-ASP-NET-Core-MVC.git
    ```

2. **Update the Connection String:**
    - Open the `appsettings.json` file located in the project root directory.
    - Locate the `"ConnectionStrings"` section.
    - Update the `DefaultConnection` string with your SQL Server connection details. 

3. **Apply Database Migrations:**
    - Open Tools > NuGet Package Manager > Package Manager Console. Select LeaveManagement.Infrastructure as default project.
    - Run the following command to apply migrations and create the database schema:
    ```sh
    Update-Database
    ```

4. **Build and Run the Application:**

5. **Access the Application:**
    - Once the application is running, open your web browser and navigate to `http://localhost:5119`.

6. **Optional: TailwindCSS Setup (if applicable):**

## Usage

To start using the Leave Management System:

1. Open your web browser and navigate to:
    ```
    http://localhost:5119
    ```
2. Register a new account and log in.
3. As an employee, you can apply for leaves and view your leave history.
4. As an admin, you can manage users, leave types, and leave requests.

## Testing

Unit tests are written for services using AutoFixture, Moq, and Fluent Assertions.

