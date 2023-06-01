CREATE DATABASE todo_app_db;
\c todo_app_db

BEGIN;

-- Create users table
CREATE TABLE users (
    id UUID PRIMARY KEY,
    first_name VARCHAR(30) NOT NULL,
    last_name VARCHAR(30) NOT NULL,
    email VARCHAR(40) NOT NULL UNIQUE,
    password VARCHAR(88) NOT NULL,
    salt VARCHAR(88) NOT NULL
);

-- Create refresh_tokens table
CREATE TABLE refresh_tokens (
    user_id UUID NOT NULL,
    token VARCHAR(88) NOT NULL UNIQUE,
    salt VARCHAR(88) NOT NULL,
    expiry_date TIMESTAMP NOT NULL,
    is_valid BOOLEAN NOT NULL,
    CONSTRAINT fk_user
      FOREIGN KEY(user_id)
        REFERENCES users(id)
          ON DELETE CASCADE
);
COMMIT;