-- +goose Up
CREATE TABLE schedules
(
    id             SERIAL PRIMARY KEY,
    begin_datetime TIMESTAMPTZ NOT NULL,
    end_datetime   TIMESTAMPTZ NOT NULL,
    course_id      UUID,
    facility_id    UUID         NOT NULL,
    created_at     TIMESTAMPTZ DEFAULT NOW(),
    updated_at     TIMESTAMPTZ DEFAULT NOW(),
    day           INT         NOT NULL,

    CONSTRAINT fk_course
        FOREIGN KEY (course_id) 
        REFERENCES courses (id),
    CONSTRAINT fk_facility
        FOREIGN KEY (facility_id) 
        REFERENCES facilities (id)
);

-- +goose Down
DROP TABLE IF EXISTS schedules;