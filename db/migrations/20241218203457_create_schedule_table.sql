-- +goose Up
CREATE TABLE schedules
(
    id             SERIAL PRIMARY KEY,
    begin_datetime TIMESTAMPTZ NOT NULL,
    end_datetime   TIMESTAMPTZ NOT NULL,
    course_id      INT         NOT NULL,
    facility_id    INT         NOT NULL,
    created_at     TIMESTAMPTZ DEFAULT NOW(),
    updated_at     TIMESTAMPTZ DEFAULT NOW()
);

-- +goose Down
DROP TABLE IF EXISTS schedules;