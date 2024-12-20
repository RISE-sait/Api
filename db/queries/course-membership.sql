-- name: CreateCourseMembership :one
INSERT INTO course_membership (course_id, membership_id, price_per_booking, is_eligible)
VALUES ($1, $2, $3, $4)
RETURNING *;

-- name: GetCourseMembershipById :one
SELECT * FROM course_membership
WHERE course_id = $1 AND membership_id = $2;

-- name: GetAllCourseMemberships :many
SELECT * FROM course_membership;

-- name: UpdateCourseMembership :exec
UPDATE course_membership
SET price_per_booking = $3, is_eligible = $4
WHERE course_id = $1 AND membership_id = $2;

-- name: DeleteCourseMembership :exec
DELETE FROM course_membership
WHERE course_id = $1 AND membership_id = $2;
