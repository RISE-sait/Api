-- +goose Up
ALTER TABLE schedules
ADD COLUMN day INT NOT NULL;

-- +goose Down
ALTER TABLE schedules
DROP COLUMN day;