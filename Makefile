# Use cmd.exe shell for Windows
SHELL := cmd.exe

# Default migration directory
MIGRATIONS_DIR=./db/migrations

# Goose executable
GOOSE=goose

DB_CONNECTION_STRING=postgresql://postgres:root@localhost:5432/mydatabase?sslmode=disable

# Create a new migration script
create-migration:
	$env:GOOSE_DRIVER="postgres"; $env:GOOSE_DBSTRING="postgresql://postgres:root@localhost:5432/mydatabase?sslmode=disable"; goose create create_schedule_table sql -dir ./db/migrations

db-up:
	@echo "Running database migrations..."
	$env:GOOSE_DRIVER="postgres"; $env:GOOSE_DBSTRING="postgresql://postgres:root@localhost:5432/mydatabase?sslmode=disable"; goose -dir ./dir/migrations up

# Rollback last migration (run goose down)
db-down:
	@echo "Rolling back last migration..."
	$(GOOSE) -dir $(MIGRATIONS_DIR) down 1 -env DB_CONNECTION_STRING=$(DB_CONNECTION_STRING)

# Reset the database (run goose reset)
db-reset:
	@echo "Resetting database migrations..."
	$(GOOSE) -dir $(MIGRATIONS_DIR) reset -env DB_CONNECTION_STRING=$(DB_CONNECTION_STRING)