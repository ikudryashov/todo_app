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

-- Create todos table
CREATE TABLE todos (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(1000),
    due_date TIMESTAMP,
    is_complete BOOLEAN,
    CONSTRAINT fk_user
      FOREIGN KEY(user_id)
        REFERENCES users(id)
          ON DELETE CASCADE
);
COMMIT;