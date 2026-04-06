#!/usr/bin/env bash
# Launcher script for HR Automation System
# Linux/NixOS compatible

echo ""
echo "╔════════════════════════════════════════════════════════╗"
echo "║     HR Automation System - Auto Start (Linux)         ║"
echo "╚════════════════════════════════════════════════════════╝"
echo ""

# Get script directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Check if bash is available
if ! command -v bash &> /dev/null; then
    echo "[ERROR] Bash not found"
    exit 1
fi

# Make setup script executable
chmod +x "$SCRIPT_DIR/setup-database.sh"

# Run setup script
bash "$SCRIPT_DIR/setup-database.sh"

if [ $? -ne 0 ]; then
    echo ""
    echo "[ERROR] An error occurred during startup"
    read -p "Press Enter to exit..."
    exit 1
fi
