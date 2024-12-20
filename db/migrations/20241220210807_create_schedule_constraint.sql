-- +goose Up
ALTER TABLE schedules
ADD CONSTRAINT chk_end_after_begin CHECK (end_datetime > begin_datetime);

-- Add the EXCLUDE constraint to prevent overlapping schedules for the same facility
CREATE EXTENSION IF NOT EXISTS btree_gist;

ALTER TABLE schedules
ADD CONSTRAINT no_overlapping_schedules
EXCLUDE USING gist (
    facility_id WITH =,
    day WITH =,
    tstzrange(begin_datetime, end_datetime) WITH &&
);

-- +goose Down
ALTER TABLE schedules
DROP CONSTRAINT no_overlapping_schedules;

ALTER TABLE schedules
DROP CONSTRAINT chk_end_after_begin;