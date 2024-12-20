-- name: CreateMembership :one
INSERT INTO memberships (id, name, description, start_date, end_date)
VALUES (gen_random_uuid(), $1, $2, $3, $4)
RETURNING *;

-- name: GetMembershipById :one
SELECT * FROM memberships WHERE id = $1;

-- name: GetAllMemberships :many
SELECT * FROM memberships;

-- name: UpdateMembership :exec
UPDATE memberships
SET name = $1, description = $2, start_date = $3, end_date = $4
WHERE id = $5;

-- name: DeleteMembership :exec
DELETE FROM memberships WHERE id = $1;