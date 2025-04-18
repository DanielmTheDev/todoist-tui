# Determine installation directory
$InstallDir = "$env:LOCALAPPDATA\Programs\todoist-tui"
$BinDir = "$env:USERPROFILE\AppData\Local\Microsoft\WindowsApps"

# Create installation directory if it doesn't exist
New-Item -ItemType Directory -Force -Path $InstallDir | Out-Null

# Copy binary to installation directory
Copy-Item -Path "todoist-tui.exe" -Destination $InstallDir -Force

# Create a link in WindowsApps directory (which is in PATH by default)
New-Item -ItemType SymbolicLink -Path "$BinDir\todoist-tui.exe" -Target "$InstallDir\todoist-tui.exe" -Force

Write-Host "todoist-tui has been installed to $InstallDir"
Write-Host "You can now run it by typing 'todoist-tui' in your terminal" 