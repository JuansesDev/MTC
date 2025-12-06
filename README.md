# MTC - Modular Template CLI

MTC is a powerful, architecture-aware CLI tool for scaffolding .NET projects. It supports **Clean Architecture**, **MVC Monolith**, and **Vertical Slice** patterns, allowing you to generate projects, features, value objects, and DTOs with ease.

## Features

- **Project Scaffolding**: Create new projects using pre-defined templates (`ConsoleApp`, `MvcMonolith`, `CleanArch`, `VerticalSlice`).
- **Architecture Awareness**: Automatically detects the project structure (MVC, Clean Arch, Vertical Slice) and places generated files in the correct layers/folders.
- **Feature Generation**: Generate full feature slices (Controller, Command/Query, Models) with a single command.
- **Productivity Tools**: Quickly add Value Objects and DTOs.
- **User Configuration**: Persist global preferences (e.g., Author name).
- **Cross-Platform**: Runs on Windows, Linux, and macOS.

## Installation

### .NET Tool (Global)
```bash
dotnet tool install -g MTC
```

### Debian/Ubuntu (.deb)
Download the latest `.deb` release and run:
```bash
sudo dpkg -i mtc_1.0.0_amd64.deb
```

### Manual (Binary)
Download the binary for your OS, extract it, and add it to your PATH.

## Usage

### Create a New Project
```bash
mtc new CleanArch MyAwesomeApp
```

### Add a Feature (Vertical Slice / Clean Arch / MVC)
```bash
cd MyAwesomeApp
mtc add feature CreateOrder --fields "ProductId:guid Quantity:int"
```

### Add a Value Object
```bash
mtc add value-object Money --fields "Amount:decimal Currency:string"
```

### Add a DTO
```bash
mtc add dto UserDto --fields "Username:string Email:string"
```

### Configuration
```bash
mtc config set Author "Juanse"
```

## Development

### Build
```bash
dotnet build
```

### Test
```bash
dotnet test
```

### Release
Use the `publish.sh` script to generate cross-platform binaries:
```bash
./publish.sh
```

## License
MIT
