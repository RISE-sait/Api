-- name: CreateSchedule :one
INSERT INTO schedules (id, begin_datetime, end_datetime, course_id, facility_id)
VALUES (gen_random_uuid(), $1, $2, $3, $4)
    RETURNING *;

-- name: GetScheduleById :one
SELECT * FROM schedules WHERE id = $1;

-- name: GetAllSchedules :many
SELECT * FROM schedules;

-- name: UpdateSchedule :exec
UPDATE schedules
SET begin_datetime = $1, end_datetime = $2, course_id = $3, facility_id = $4, day = $5
WHERE id = $6;

-- name: DeleteSchedule :execrows
DELETE FROM schedules WHERE id = $1
RETURNING id;

-- name: CountOverlappingSchedules :one
SELECT COUNT(*) FROM schedules
WHERE facility_id = $1
  AND day = $2
  AND begin_datetime < $3
  AND end_datetime > $4;