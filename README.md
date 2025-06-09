# Todoist TUI

A terminal user interface for Todoist with both interactive and command-line modes.

## Installation

1. Download the appropriate package for your system from the [releases page](https://github.com/DanielmTheDev/todoist-tui/releases):
   - Linux (x64): `todoist-tui-linux-x64.tar.gz`
   - Linux (ARM64/aarch64): `todoist-tui-linux-arm64.tar.gz`
   - Windows (x64): `todoist-tui-windows-x64.zip`
   - macOS (Intel): `todoist-tui-macos-x64.tar.gz`
   - macOS (Apple Silicon M1/M2/M3): `todoist-tui-macos-arm64.tar.gz`

2. Extract the archive:
   - Linux/macOS: `tar xzf todoist-tui-*.tar.gz`
   - Windows: Extract the zip file

3. Run the install script:
   - Linux/macOS: `./install.sh`
   - Windows: Right-click `install.ps1` and select "Run with PowerShell"

The installer will:
- Copy the binary to an appropriate location (`/usr/local/bin` on Linux/macOS, `%LOCALAPPDATA%\Programs\todoist-tui` on Windows)
- Make it available in your PATH
- Make it executable (Linux/macOS)

After installation, you can run `todoist-tui` from any terminal.

## Setup

Before using todoist-tui, you need to set up your Todoist API token:

1. Get your API token from [Todoist Settings > Integrations](https://todoist.com/app/settings/integrations)
2. Set it as an environment variable:

```bash
# Linux/macOS
export TODOIST_API_TOKEN=your_token_here
# Add to ~/.bashrc or ~/.zshrc to make it permanent

# Windows (PowerShell)
$env:TODOIST_API_TOKEN="your_token_here"
# Add to your PowerShell profile to make it permanent
```

## Features

### Interactive Mode

Run `todoist-tui` without arguments to enter interactive mode, which provides:

- Add Task: Create a new task with content, due date and optional label
- Add Task with Load Balancing: Automatically add tasks up to selected number of days ahead, chosen dependent on least load
- Complete and add reminder: Completes a task, but prompts to add a reminder for today. Especially nice for recurring tasks
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