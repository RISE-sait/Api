-- name: CreateCourse :one
INSERT INTO courses (id, name, description, start_date, end_date)
VALUES (gen_random_uuid(), $1, $2, $3, $4)
RETURNING *;

-- name: GetCourseById :one
SELECT * FROM courses WHERE id = $1;

-- name: GetAllCourses :many
SELECT * FROM courses;

-- name: UpdateCourse :exec
UPDATE courses
SET name = $1, description = $2, start_date = $3, end_date = $4
WHERE id = $5;

-- name: DeleteCourse :exec
DELETE FROM courses WHERE id = $1;
