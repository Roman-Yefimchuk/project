INSERT INTO users(role, name, password)
OUTPUT INSERTED.id 
VALUES ('user', '{0}', '{1}');