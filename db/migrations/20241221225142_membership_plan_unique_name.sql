-- +goose Up
-- +goose StatementBegin
ALTER TABLE membership_plans
ADD CONSTRAINT unique_membership_id_name UNIQUE (membership_id, name);
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
ALTER TABLE membership_plans
DROP CONSTRAINT unique_membership_id_name;
-- +goose StatementEnd