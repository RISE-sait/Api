-- name: CreateMembershipPlan :one
INSERT INTO membership_plans (id, membership_id, name, price, payment_frequency, amt_periods)
VALUES (gen_random_uuid(), $1, $2, $3, $4, $5)
RETURNING *;

-- name: GetMembershipPlans :many
SELECT * 
FROM membership_plans
WHERE 
    ($1::UUID IS NULL OR $1::UUID = '00000000-0000-0000-0000-000000000000' OR membership_id = $1) AND
    ($2::UUID IS NULL OR $2::UUID = '00000000-0000-0000-0000-000000000000' OR id = $2);

-- name: UpdateMembershipPlan :exec
UPDATE membership_plans
SET name = $1, price = $2, payment_frequency = $3, amt_periods = $4
WHERE membership_id = $5 AND id = $6;

-- name: DeleteMembershipPlan :exec
DELETE FROM membership_plans WHERE membership_id = $1 AND id = $2;