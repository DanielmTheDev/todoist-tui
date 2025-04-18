#!/bin/bash

# Set installation directory
INSTALL_DIR="/usr/local/bin"

# Create directory if it doesn't exist
sudo mkdir -p "$INSTALL_DIR"

# Copy binary to installation directory
sudo cp todoist-tui "$INSTALL_DIR/"

# Make it executable
sudo chmod +x "$INSTALL_DIR/todoist-tui"

echo "todoist-tui has been installed to $INSTALL_DIR/todoist-tui"
echo "You can now run it by typing 'todoist-tui' in your terminal" 