# Todoist TUI

A terminal user interface for Todoist with both interactive and command-line modes.

## Installation

### Using Pre-built Binary

1. Download the latest release from the [GitHub releases page](https://github.com/yourusername/todoist-tui/releases)
2. Move the binary to a directory in your PATH
3. Set up your Todoist API token:
   ```bash
   # Linux/macOS
   export TODOIST_API_TOKEN=your_token_here
   # Add to ~/.bashrc or ~/.zshrc to make it permanent
   
   # Windows (PowerShell)
   $env:TODOIST_API_TOKEN="your_token_here"
   # Add to your PowerShell profile to make it permanent
   ```
4. Run `todoist-tui` to start the interactive mode

### Building from Source

1. Clone the repository
2. Run `dotnet publish -c Release -r <RID> --self-contained true` to build for your system
   - For Linux: `dotnet publish -c Release -r linux-x64 --self-contained true`
   - For macOS: `dotnet publish -c Release -r osx-x64 --self-contained true`
   - For Windows: `dotnet publish -c Release -r win-x64 --self-contained true`
3. Move the binary from `bin/Release/net9.0/<RID>/publish/` to a directory in your PATH
4. Set up your Todoist API token as described above
5. Run `todoist-tui` to start the interactive mode

## Features

### Interactive Mode

Run `todoist-tui` without arguments to enter interactive mode, which provides:

- Add Task: Create a new task with content, due date and optional label
- Add Task with Load Balancing: Automatically add tasks up to selected number of days ahead, chosen dependent on least load
- Complete Tasks: Bulk mark today's tasks as complete
- Schedule Today: Schedule tasks for today with specific times
- Collect Under New Parent: Group tasks under a new parent task
- Postpone Today: Move selected tasks up to the selected number of days ahead, distributing among days with least load
- Reset Today's Priority: Reset priority of today's tasks

### Command Line Mode

Use the following commands for quick task management:

```bash
# Add a new task
todoist-tui -a "Buy groceries" "tomorrow"

# Add a task for today
todoist-tui --addToday "Buy groceries"
```

## Requirements

- .NET 9.0 or later
- Todoist account and API token (set as TODOIST_API_TOKEN environment variable) 