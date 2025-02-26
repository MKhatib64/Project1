# Multi-Threaded Banking System and IPC Simulation

## Overview
This repository contains two separate projects:
1. **Multi-Threaded Banking System**: Simulates concurrent transactions between accounts.
2. **IPC Simulation**: Demonstrates communication between a producer and consumer using pipes.

Both projects were implemented in **C#** using the **.NET SDK** and tested on a **Linux-based environment** (Ubuntu on WSL).

---

## Multi-Threaded Banking System

### Features
- **Thread Creation**: Multiple threads perform concurrent transactions.
- **Synchronization**: The `lock` keyword is used to protect shared resources (account balances).
- **Deadlock Prevention**: Lock ordering ensures that deadlocks are avoided during transfers.
- **Logging**: All transactions are logged to a file (`transactions.log`) for auditing.

### How to Run
1. Navigate to the `BankingSystem` folder:
   ```bash
   cd BankingSystem
   ```
2. Build and run the program:
   ```bash
   dotnet build
   dotnet run
   ```

---

## IPC Simulation

### Features
- **Pipe Communication**: Demonstrates communication between a producer and consumer using pipes.
- **Data Flow**: The producer sends messages to the consumer via a pipe.
- **Graceful Exit**: The producer sends an exit signal to stop the consumer.

### How to Run
1. Navigate to the `IPC` folder:
   ```bash
   cd IPC
   ```
2. Build and run the program:
   ```bash
   dotnet build
   dotnet run
   ```

---

## Dependencies and Installation Instructions

### Dependencies
- **.NET SDK**:
  - Required to compile and run the project.
  - Download and install the .NET SDK from [here](https://dotnet.microsoft.com/download).

- **Linux-based Environment**:
  - Recommended for development and testing.
  - If you're on Windows, you can use Windows Subsystem for Linux (WSL):
    - Install WSL by following the [official guide](https://learn.microsoft.com/en-us/windows/wsl/install).
    - Install a Linux distribution (e.g., Ubuntu) from the Microsoft Store.

### Installation Instructions

1. Install **.NET SDK**:
   - Follow the [official installation guide](https://dotnet.microsoft.com/en-us/download) for your operating system.

2. Set Up **Linux Environment**:
   - If you're using WSL, open a terminal and install Ubuntu or another Linux distribution.
   - Update the package list and install necessary tools:
     ```bash
     sudo apt update
     sudo apt install git dotnet-sdk-6.0
     ```

3. Clone and Run the Project:
   - Clone the repository:
     ```bash
     git clone https://github.com/MKhatib64/Project1.git
     cd Project1
     ```
   - Follow the instructions above to run each program.

