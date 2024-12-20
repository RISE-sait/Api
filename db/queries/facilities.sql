-- name: CreateFacilityType :one
INSERT INTO facility_types (id, name) VALUES (uuid_generate_v4(), $1) RETURNING *;

-- name: GetFacilityTypeById :one
SELECT * FROM facility_types WHERE id = $1;

-- name: GetAllFacilityTypes :many
SELECT * FROM facility_types;

-- name: UpdateFacilityType :exec
UPDATE facility_types SET name = $1 WHERE id = $2;

-- name: DeleteFacilityType :exec
DELETE FROM facility_types WHERE id = $1;

-- name: CreateFacility :one
INSERT INTO facilities (id, name, location, facility_type_id)
VALUES (gen_random_uuid(), $1, $2, $3)
    RETURNING *;

-- name: GetFacilityById :one
SELECT * FROM facilities WHERE id = $1;

-- name: GetAllFacilities :many
SELECT * FROM facilities;

-- name: UpdateFacility :exec
UPDATE facilities
SET name = $1, location = $2, facility_type_id = $3
WHERE id = $4;

-- name: DeleteFacility :exec
DELETE FROM facilities WHERE id = $1;