# Default migration directory
MIGRATIONS_DIR=./db/migrations

# Goose executable
GOOSE=goose

DB_CONNECTION_STRING=postgresql://postgres:root@localhost:5432/mydatabase?sslmode=disable

# Create a new migration script
create-migration:
	@if (-not "$(name)") { \
		Write-Host "Migration name is required. Usage: make create-migration name=<migration_name>"; \
		exit 1; \
	} else { \
		$env:GOOSE_DRIVER="postgres"; $env:GOOSE_DBSTRING="postgresql://postgres:root@localhost:5432/mydatabase?sslmode=disable"; goose -dir ./dir/migrations up
	}

db-up:
	$env:GOOSE_DRIVER="postgres"; $env:GOOSE_DBSTRING="postgresql://postgres:root@localhost:5432/mydatabase?sslmode=disable"; goose -dir ./db/migrations up

db-down:
	@Write-Host "Rolling back last migration..."; \
	$env:GOOSE_DRIVER = "postgres"; \
	$env:GOOSE_DBSTRING = "$(DB_CONNECTION_STRING)"; \
	& $(GOOSE) -dir $(MIGRATIONS_DIR) down 1;

db-reset:
	@Write-Host "Resetting database migrations..."; \
	$env:GOOSE_DRIVER = "postgres"; \
	$env:GOOSE_DBSTRING = "$(DB_CONNECTION_STRING)"; \
	& $(GOOSE) -dir $(MIGRATIONS_DIR) reset;
