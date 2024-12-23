// Code generated by sqlc. DO NOT EDIT.
// versions:
//   sqlc v1.27.0
// source: facilities.sql

package db

import (
	"context"

	"github.com/google/uuid"
)

const createFacility = `-- name: CreateFacility :one
INSERT INTO facilities (id, name, location, facility_type_id)
VALUES (gen_random_uuid(), $1, $2, $3)
    RETURNING id, name, location, facility_type_id
`

type CreateFacilityParams struct {
	Name           string    `json:"name"`
	Location       string    `json:"location"`
	FacilityTypeID uuid.UUID `json:"facility_type_id"`
}

func (q *Queries) CreateFacility(ctx context.Context, arg CreateFacilityParams) (Facility, error) {
	row := q.db.QueryRowContext(ctx, createFacility, arg.Name, arg.Location, arg.FacilityTypeID)
	var i Facility
	err := row.Scan(
		&i.ID,
		&i.Name,
		&i.Location,
		&i.FacilityTypeID,
	)
	return i, err
}

const createFacilityType = `-- name: CreateFacilityType :one
INSERT INTO facility_types (id, name) VALUES (uuid_generate_v4(), $1) RETURNING id, name
`

func (q *Queries) CreateFacilityType(ctx context.Context, name string) (FacilityType, error) {
	row := q.db.QueryRowContext(ctx, createFacilityType, name)
	var i FacilityType
	err := row.Scan(&i.ID, &i.Name)
	return i, err
}

const deleteFacility = `-- name: DeleteFacility :exec
DELETE FROM facilities WHERE id = $1
`

func (q *Queries) DeleteFacility(ctx context.Context, id uuid.UUID) error {
	_, err := q.db.ExecContext(ctx, deleteFacility, id)
	return err
}

const deleteFacilityType = `-- name: DeleteFacilityType :exec
DELETE FROM facility_types WHERE id = $1
`

func (q *Queries) DeleteFacilityType(ctx context.Context, id uuid.UUID) error {
	_, err := q.db.ExecContext(ctx, deleteFacilityType, id)
	return err
}

const getAllFacilities = `-- name: GetAllFacilities :many
SELECT id, name, location, facility_type_id FROM facilities
`

func (q *Queries) GetAllFacilities(ctx context.Context) ([]Facility, error) {
	rows, err := q.db.QueryContext(ctx, getAllFacilities)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	var items []Facility
	for rows.Next() {
		var i Facility
		if err := rows.Scan(
			&i.ID,
			&i.Name,
			&i.Location,
			&i.FacilityTypeID,
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

const getAllFacilityTypes = `-- name: GetAllFacilityTypes :many
SELECT id, name FROM facility_types
`

func (q *Queries) GetAllFacilityTypes(ctx context.Context) ([]FacilityType, error) {
	rows, err := q.db.QueryContext(ctx, getAllFacilityTypes)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	var items []FacilityType
	for rows.Next() {
		var i FacilityType
		if err := rows.Scan(&i.ID, &i.Name); err != nil {
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

const getFacilityById = `-- name: GetFacilityById :one
SELECT id, name, location, facility_type_id FROM facilities WHERE id = $1
`

func (q *Queries) GetFacilityById(ctx context.Context, id uuid.UUID) (Facility, error) {
	row := q.db.QueryRowContext(ctx, getFacilityById, id)
	var i Facility
	err := row.Scan(
		&i.ID,
		&i.Name,
		&i.Location,
		&i.FacilityTypeID,
	)
	return i, err
}

const getFacilityTypeById = `-- name: GetFacilityTypeById :one
SELECT id, name FROM facility_types WHERE id = $1
`

func (q *Queries) GetFacilityTypeById(ctx context.Context, id uuid.UUID) (FacilityType, error) {
	row := q.db.QueryRowContext(ctx, getFacilityTypeById, id)
	var i FacilityType
	err := row.Scan(&i.ID, &i.Name)
	return i, err
}

const updateFacility = `-- name: UpdateFacility :exec
UPDATE facilities
SET name = $1, location = $2, facility_type_id = $3
WHERE id = $4
`

type UpdateFacilityParams struct {
	Name           string    `json:"name"`
	Location       string    `json:"location"`
	FacilityTypeID uuid.UUID `json:"facility_type_id"`
	ID             uuid.UUID `json:"id"`
}

func (q *Queries) UpdateFacility(ctx context.Context, arg UpdateFacilityParams) error {
	_, err := q.db.ExecContext(ctx, updateFacility,
		arg.Name,
		arg.Location,
		arg.FacilityTypeID,
		arg.ID,
	)
	return err
}

const updateFacilityType = `-- name: UpdateFacilityType :exec
UPDATE facility_types SET name = $1 WHERE id = $2
`

type UpdateFacilityTypeParams struct {
	Name string    `json:"name"`
	ID   uuid.UUID `json:"id"`
}

func (q *Queries) UpdateFacilityType(ctx context.Context, arg UpdateFacilityTypeParams) error {
	_, err := q.db.ExecContext(ctx, updateFacilityType, arg.Name, arg.ID)
	return err
}
