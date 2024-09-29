# Pokemons 🐉

Pokemons is a project designed for a Telegram Web App, including an API and a bot. The project is built on .NET 8 and follows a layered architecture, handling business logic, database interaction, caching, and API for managing game events.

# About the Project 🎮

The Pokemons project provides an API to handle game interactions within a Telegram Web App and includes a bot that registers users and launches the game. The bot communicates with the API via RabbitMQ to manage in-game events.
# Requirements 📋

You will need the following to run the project:

    Docker and Docker Compose
    
# Environment Variables 🔑

You can find all required environment variables in the .env.sample file located in the Build folder.

# Getting Started 🚀

Navigate to the Build folder:

``` bash
cd src/Build
```
Create a .env file based on .env.sample and configure the necessary environment variables.

Run the project using Docker Compose:

``` bash
docker compose up --build
```

This will launch all required services (API, databases, cache) automatically.
# Project Structure 🏗️

The project is divided into several layers:

    API Layer:
        Handles HTTP requests and routes them to the appropriate business logic.
        Example endpoint can be found in the BattleController.

    Service Layer:
        Contains the core business logic of the project.
        Manages the game logic such as dealing damage and using super abilities.

    Repository Layer:
        Implements data access using Entity Framework Core.
        Interacts with PostgreSQL for persistent storage and Redis for caching.

    Data Storage Layer:
        Uses PostgreSQL for storing persistent data and Redis for caching.

# Bot 🤖

The Telegram bot in this project performs the following functions:

    User registration — the bot registers new players.
    Game launch — the bot initiates the game for registered users.

The bot communicates with the API via RabbitMQ, sending and receiving game-related events.
# Technologies 🛠️

The project is built with the following technologies:

    .NET 8 — for building the API and game logic.
    PostgreSQL — for storing user data and game events.
    Redis — for caching data.
    RabbitMQ — for message-based communication between the bot and API.
    Docker — for containerizing and orchestrating all services.
