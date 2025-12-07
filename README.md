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

### Arch Linux (AUR)
```bash
yay -S mtc-bin
```

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
Download the binary for your OS from [Releases](https://github.com/JuansesDev/MTC/releases).

**Windows**:
1. Download `mtc-win-x64-1.0.0.tar.gz`.
2. Extract the contents (Windows 10/11 supports tar.gz natively, or use 7-Zip).
3. Add the extracted folder to your System PATH.

**Linux/macOS**:
1. Download the `.tar.gz` for your OS.
2. Extract and move the binary to `/usr/local/bin` or add to PATH.

## Usage

### List Available Templates
```bash
mtc list
```

### Create a New Project
```bash
# Console Application
mtc new ConsoleApp MyApp

# MVC Monolith
mtc new MvcMonolith MyWebApp

# Clean Architecture
mtc new CleanArch MyCleanApp

# Vertical Slice Architecture
mtc new VerticalSlice MyApiApp
```

### Add Features to Existing Projects
MTC automatically detects your project architecture and generates the appropriate files.

#### Add a Feature (CRUD)
```bash
# Generates Controller, Commands/Queries, and Models based on your architecture
mtc add feature Product --fields "Name:string Price:decimal Stock:int"
```

#### Add a Value Object
```bash
mtc add value-object Money --fields "Amount:decimal Currency:string"
mtc add value-object Email --fields "Value:string"
```

#### Add a DTO
```bash
mtc add dto UserDto --fields "Username:string Email:string Age:int"
mtc add dto ProductDto --fields "Id:guid Name:string Price:decimal"
```

### Configuration Commands
```bash
# Set a configuration value
mtc config set Author "Your Name"
mtc config set Company "Your Company"

# Get a configuration value
mtc config get Author

# List all configuration
mtc config list
```

### Debug Commands
```bash
# Show detected project context (architecture, root path, etc.)
mtc debug-context
```

### Field Types
When using `--fields`, you can use these types:
- `string`, `int`, `decimal`, `bool`, `datetime`, `guid`
- Example: `"Name:string Price:decimal IsActive:bool CreatedAt:datetime"`

## Development

### Build
```bash
dotnet build
```

### Test
```bash
dotnet test
```

### Publish Binaries
```bash
./publish.sh
```

### Publish to AUR (Maintainers only)
```bash
./publish_aur.sh
```

### Build Debian Package (Maintainers only)
```bash
./publish_deb.sh
```

## License
MIT
