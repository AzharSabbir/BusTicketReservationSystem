# Bus Ticket Reservation System

This project is a simple bus ticket reservation system. The backend API was built using .NET 9 (C#) and the frontend client was built using Angular, following Clean Architecture and Domain-Driven Design principles.

## Features

* Available buses between two locations on a specific date can be searched.
* Bus seat layout showing available and booked seats can be viewed.
* Seats can be selected and booked by providing passenger details.

## Technologies Used

* **Backend:** .NET 9 (C#), ASP.NET Core Web API, Entity Framework Core 9
* **Database:** PostgreSQL
* **Frontend:** Angular (latest stable via CLI), TypeScript, CSS
* **Architecture:** Clean Architecture, Domain-Driven Design (DDD)
* **Testing:** xUnit (Backend Unit Tests)

## Prerequisites

Before setup, ensure the following are installed:

* [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [Node.js (LTS version)](https://nodejs.org/) (includes npm)
* [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
* [PostgreSQL Server](https://www.postgresql.org/download/) (and a tool like pgAdmin)
* [Git](https://git-scm.com/downloads/)
* A code editor like [Visual Studio 2022](https://visualstudio.microsoft.com/) (for backend) and [VS Code](https://code.visualstudio.com/) (for frontend).

## Setup Instructions

1.  **Repository Cloning:**
    * The repository should be cloned using Git:
        ```bash
        git clone https://github.com/AzharSabbir/BusTicketReservationSystem.git
        cd BusTicketReservationSystem
        ```

2.  **Backend Setup (.NET):**
    * **Database Configuration:**
        * The `src/WebApi/appsettings.json` file should be opened.
        * The `ConnectionStrings` section needs to be located.
        * The `Password` field for the `BusReservationDb` connection string **must be updated** to match your PostgreSQL `postgres` user password. (***Correction:*** *The name is `BusReservationDb`, not `BusTicketReservationSystemDb`*)
        * Ensure the `Host`, `Database`, and `Username` settings are correct for your environment.
    * **Database Schema Creation:**
        * A terminal or command prompt should be opened in the **root** folder of the project.
        * The following command must be run to apply Entity Framework Core migrations and create the database tables:
            ```bash
            dotnet ef database update --project src/Infrastructure --startup-project src/WebApi
            ```
    * **Database Seeding (Manual SQL):**
        * Your PostgreSQL client (e.g., pgAdmin) should be opened.
        * A connection must be made to your `BusTicketReservationSystem` database.
        * The SQL scripts located in the `/DatabaseScripts` folder **must be opened and executed in numerical order** (01_, 02_, 03_, 04_, 05_). This ensures tables are populated correctly based on dependencies.

3.  **Frontend Setup (Angular):**
    * **Navigate to ClientApp:**
        * The command prompt or terminal should be navigated to the frontend directory:
            ```bash
            cd src/ClientApp
            ```
    * **Install Dependencies:**
        * Node.js package dependencies must be installed:
            ```bash
            npm install
            ```

## Running the Application

Both the backend and frontend need to be run simultaneously.

1.  **Run the Backend:**
    * The solution file (`.sln`) should be opened in Visual Studio 2022.
    * `WebApi` must be set as the startup project.
    * Press **F5** or click the **Run** button. A console window will appear, indicating the backend is running (usually on `https://localhost:xxxx`). This must be kept running.

2.  **Run the Frontend:**
    * A **new** terminal or command prompt should be opened.
    * Navigate to the Angular project folder:
        ```bash
        cd src/ClientApp
        ```
    * The Angular development server should be started:
        ```bash
        ng serve --open
        ```
    * The frontend will be compiled, and your default web browser will automatically open to the application (usually `http://localhost:4200`).

## Running Unit Tests (Backend)

1.  The solution should be opened in Visual Studio 2022.
2.  Navigate to the **Test** menu.
3.  Select **Test Explorer**.
4.  Click the **Run All Tests** button in the Test Explorer window. All tests should pass.