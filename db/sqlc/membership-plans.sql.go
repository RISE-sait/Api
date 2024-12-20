// Code generated by sqlc. DO NOT EDIT.
// versions:
//   sqlc v1.27.0
// source: membership-plans.sql

package db

import (
	"context"
	"database/sql"

	"github.com/google/uuid"
)

const createMembershipPlan = `-- name: CreateMembershipPlan :one
INSERT INTO membership_plans (id, membership_id, name, price, payment_frequency, amt_periods)
VALUES (gen_random_uuid(), $1, $2, $3, $4, $5)
RETURNING id, name, price, membership_id, payment_frequency, amt_periods, created_at, updated_at
`

type CreateMembershipPlanParams struct {
	MembershipID     uuid.UUID            `json:"membership_id"`
	Name             string               `json:"name"`
	Price            int64                `json:"price"`
	PaymentFrequency NullPaymentFrequency `json:"payment_frequency"`
	AmtPeriods       sql.NullInt32        `json:"amt_periods"`
}

func (q *Queries) CreateMembershipPlan(ctx context.Context, arg CreateMembershipPlanParams) (MembershipPlan, error) {
	row := q.db.QueryRowContext(ctx, createMembershipPlan,
		arg.MembershipID,
		arg.Name,
		arg.Price,
		arg.PaymentFrequency,
		arg.AmtPeriods,
	)
	var i MembershipPlan
	err := row.Scan(
		&i.ID,
		&i.Name,
		&i.Price,
		&i.MembershipID,
		&i.PaymentFrequency,
		&i.AmtPeriods,
		&i.CreatedAt,
		&i.UpdatedAt,
	)
	return i, err
}

const deleteMembershipPlan = `-- name: DeleteMembershipPlan :exec
DELETE FROM membership_plans WHERE membership_id = $1 AND id = $2
`

type DeleteMembershipPlanParams struct {
	MembershipID uuid.UUID `json:"membership_id"`
	ID           uuid.UUID `json:"id"`
}

func (q *Queries) DeleteMembershipPlan(ctx context.Context, arg DeleteMembershipPlanParams) error {
	_, err := q.db.ExecContext(ctx, deleteMembershipPlan, arg.MembershipID, arg.ID)
	return err
}

const getAllMembershipPlans = `-- name: GetAllMembershipPlans :many
SELECT id, name, price, membership_id, payment_frequency, amt_periods, created_at, updated_at FROM membership_plans
`

func (q *Queries) GetAllMembershipPlans(ctx context.Context) ([]MembershipPlan, error) {
	rows, err := q.db.QueryContext(ctx, getAllMembershipPlans)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	var items []MembershipPlan
	for rows.Next() {
		var i MembershipPlan
		if err := rows.Scan(
			&i.ID,
			&i.Name,
			&i.Price,
			&i.MembershipID,
			&i.PaymentFrequency,
			&i.AmtPeriods,
			&i.CreatedAt,
			&i.UpdatedAt,
		); err != nil {
			return nil, err
		}
		items = append(items, i)
	}
	if err := rows.Close(); err != nil {
		return nil, err
	}
	if err := rows.Err(); err != nil {
		return nil, err
	}
	return items, nil
}

const getMembershipPlanById = `-- name: GetMembershipPlanById :one
SELECT id, name, price, membership_id, payment_frequency, amt_periods, created_at, updated_at FROM membership_plans WHERE membership_id = $1 AND id = $2
`

type GetMembershipPlanByIdParams struct {
	MembershipID uuid.UUID `json:"membership_id"`
	ID           uuid.UUID `json:"id"`
}

func (q *Queries) GetMembershipPlanById(ctx context.Context, arg GetMembershipPlanByIdParams) (MembershipPlan, error) {
	row := q.db.QueryRowContext(ctx, getMembershipPlanById, arg.MembershipID, arg.ID)
	var i MembershipPlan
	err := row.Scan(
		&i.ID,
		&i.Name,
		&i.Price,
		&i.MembershipID,
		&i.PaymentFrequency,
		&i.AmtPeriods,
		&i.CreatedAt,
		&i.UpdatedAt,
	)
	return i, err
}

const updateMembershipPlan = `-- name: UpdateMembershipPlan :exec
UPDATE membership_plans
SET name = $1, price = $2, payment_frequency = $3, amt_periods = $4
WHERE membership_id = $5 AND id = $6
`

type UpdateMembershipPlanParams struct {
	Name             string               `json:"name"`
	Price            int64                `json:"price"`
	PaymentFrequency NullPaymentFrequency `json:"payment_frequency"`
	AmtPeriods       sql.NullInt32        `json:"amt_periods"`
	MembershipID     uuid.UUID            `json:"membership_id"`
	ID               uuid.UUID            `json:"id"`
}

func (q *Queries) UpdateMembershipPlan(ctx context.Context, arg UpdateMembershipPlanParams) error {
	_, err := q.db.ExecContext(ctx, updateMembershipPlan,
		arg.Name,
		arg.Price,
		arg.PaymentFrequency,
		arg.AmtPeriods,
		arg.MembershipID,
		arg.ID,
	)
	return err
}
