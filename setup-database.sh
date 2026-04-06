#!/usr/bin/env bash
# Bash script for database setup and application launch
# HR Automation System - Linux/NixOS compatible

set -e

echo "=== HR Automation System - Setup ==="
echo ""

# PostgreSQL connection parameters
PG_HOST="localhost"
PG_PORT="5432"
PG_USER="postgres"
PG_PASSWORD="0206"
DB_NAME="hr_automation"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
WHITE='\033[1;37m'
NC='\033[0m' # No Color

# Check PostgreSQL
echo -e "${YELLOW}Checking PostgreSQL...${NC}"
if command -v psql &> /dev/null; then
    export PGPASSWORD=$PG_PASSWORD
    if psql -h $PG_HOST -p $PG_PORT -U $PG_USER -d postgres -c "SELECT version();" &> /dev/null; then
        echo -e "${GREEN}[OK] PostgreSQL found${NC}"
    else
        echo -e "${RED}[ERROR] PostgreSQL is not running${NC}"
        echo -e "${YELLOW}Start PostgreSQL service:${NC}"
        echo "  sudo systemctl start postgresql"
        echo "  or: sudo service postgresql start"
        exit 1
    fi
else
    echo -e "${RED}[ERROR] PostgreSQL is not installed${NC}"
    echo -e "${YELLOW}Install PostgreSQL:${NC}"
    echo "  NixOS: nix-env -iA nixos.postgresql"
    echo "  Ubuntu/Debian: sudo apt install postgresql"
    exit 1
fi

# Check .NET SDK
echo -e "${YELLOW}Checking .NET SDK...${NC}"
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    echo -e "${GREEN}[OK] .NET SDK found (version $DOTNET_VERSION)${NC}"
else
    echo -e "${RED}[ERROR] .NET SDK is not installed${NC}"
    echo -e "${YELLOW}Install .NET 8 SDK:${NC}"
    echo "  NixOS: nix-env -iA nixos.dotnet-sdk_8"
    echo "  Ubuntu/Debian: https://dotnet.microsoft.com/download"
    exit 1
fi

# Create database if not exists
echo ""
echo -e "${YELLOW}Creating database...${NC}"
export PGPASSWORD=$PG_PASSWORD
CHECK_DB=$(psql -h $PG_HOST -p $PG_PORT -U $PG_USER -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname='$DB_NAME'" 2>&1)

if [ "$CHECK_DB" = "1" ]; then
    echo -e "${GREEN}[OK] Database '$DB_NAME' already exists${NC}"
else
    if psql -h $PG_HOST -p $PG_PORT -U $PG_USER -d postgres -c "CREATE DATABASE $DB_NAME;" &> /dev/null; then
        echo -e "${GREEN}[OK] Database '$DB_NAME' created${NC}"
    else
        echo -e "${RED}[ERROR] Failed to create database${NC}"
        exit 1
    fi
fi

# Apply migrations
echo ""
echo -e "${YELLOW}Applying migrations...${NC}"
cd Automation
if dotnet ef database update 2>&1; then
    echo -e "${GREEN}[OK] Migrations applied successfully${NC}"
else
    echo -e "${RED}[ERROR] Failed to apply migrations${NC}"
    echo -e "${YELLOW}Try manually: cd Automation && dotnet ef database update${NC}"
    cd ..
    exit 1
fi

# Build application
echo ""
echo -e "${YELLOW}Building application...${NC}"
if dotnet build -c Release > /dev/null 2>&1; then
    echo -e "${GREEN}[OK] Application built${NC}"
else
    echo -e "${RED}[ERROR] Failed to build application${NC}"
    cd ..
    exit 1
fi

# Launch application
echo ""
echo -e "${CYAN}=== Launching Application ===${NC}"
echo ""
echo -e "${YELLOW}Login credentials:${NC}"
echo -e "${WHITE}  Administrator: admin / admin123${NC}"
echo -e "${WHITE}  HR Manager:    hr / hr123${NC}"
echo ""

dotnet run -c Release
cd ..
