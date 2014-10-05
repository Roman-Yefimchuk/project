CREATE TABLE users(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY, role TEXT, name TEXT, password TEXT);

INSERT INTO users(role, name, password) VALUES('master', 'sergiy', 'qwerty');