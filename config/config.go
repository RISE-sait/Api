package config

import (
	"database/sql"
	"log"
	"os"

	"github.com/joho/godotenv"
)

type dbConfig struct {
	host     string
	port     string
	user     string
	password string
	name     string
}

type config struct {
	dbConfig      dbConfig
	HubSpotApiKey string
}

var Envs = initConfig()

func initConfig() config {
	err := godotenv.Load()
	if err != nil {
		return config{}
	}

	return config{
		dbConfig: dbConfig{
			host:     getEnv("DB_HOST", "localhost"),
			port:     getEnv("DB_PORT", "5432"),
			user:     getEnv("DB_USER", "postgres"),
			password: getEnv("DB_PASSWORD", "root"),
			name:     getEnv("DB_NAME", "mydatabase"),
		},
		HubSpotApiKey: getEnv("HUBSPOT_API_KEY", ""),
	}
}

func getEnv(key string, defaultValue string) string {
	if value, exists := os.LookupEnv(key); exists {
		return value
	}
	return defaultValue
}

func getConnectionString() string {
	return "postgresql://" + Envs.dbConfig.user + ":" + Envs.dbConfig.password + "@" + Envs.dbConfig.host + ":" + Envs.dbConfig.port + "/" + Envs.dbConfig.name + "?sslmode=disable"
}

func GetDBConnection() *sql.DB {
	connStr := getConnectionString() // Moved connection string logic to config package
	dbConn, err := sql.Open("postgres", connStr)
	if err != nil {
		log.Fatal(err)
	}
	return dbConn
}
