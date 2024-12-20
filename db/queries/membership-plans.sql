-- name: CreateMembershipPlan :one
INSERT INTO membership_plans (id, membership_id, name, price, payment_frequency, amt_periods)
VALUES (gen_random_uuid(), $1, $2, $3, $4, $5)
RETURNING *;

-- name: GetMembershipPlanById :one
SELECT * FROM membership_plans WHERE membership_id = $1 AND id = $2;

-- name: GetAllMembershipPlans :many
SELECT * FROM membership_plans;

-- name: UpdateMembershipPlan :exec
UPDATE membership_plans
SET name = $1, price = $2, payment_frequency = $3, amt_periods = $4
WHERE membership_id = $5 AND id = $6;

-- name: DeleteMembershipPlan :exec
DELETE FROM membership_plans WHERE membership_id = $1 AND id = $2;