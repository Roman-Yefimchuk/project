CREATE TABLE users(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                   role TEXT,
                   name TEXT,
                   password TEXT);

CREATE TABLE entityTypes(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                         type TEXT,
                         name NVARCHAR(MAX));

CREATE TABLE entityModels(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                          model TEXT,
                          entityTypes TEXT);

CREATE TABLE problems(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                      description NVARCHAR(MAX),
                      entityTypes TEXT);

CREATE TABLE clusters(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                      entityTypeId INTEGER,
                      entityModelId INTEGER,
                      problemId INTEGER,
                      solution NVARCHAR(MAX));



INSERT INTO users(role, name, password)
VALUES('master', 'Master', 'qwerty');

INSERT INTO users(role, name, password)
VALUES('user', 'User', 'qwerty');



INSERT INTO entityTypes(type, name)
VALUES('laptop', N'Ноутбук');

INSERT INTO entityTypes(type, name)
VALUES('printer', N'Прінтер');

INSERT INTO entityTypes(type, name)
VALUES('computer', N'Комп''ютер');

INSERT INTO entityTypes(type, name)
VALUES('monitor', N'Монітор');



INSERT INTO entityModels(model, entityTypes)
VALUES('Apple', 'laptop,computer,monitor');

INSERT INTO entityModels(model, entityTypes)
VALUES('Samsung', 'computer,monitor');

INSERT INTO entityModels(model, entityTypes)
VALUES('DELL', 'laptop,monitor');

INSERT INTO entityModels(model, entityTypes)
VALUES('HP', 'printer');

INSERT INTO entityModels(model, entityTypes)
VALUES('Canon', 'printer');

INSERT INTO entityModels(model, entityTypes)
VALUES('Lenovo', 'laptop,computer');



INSERT INTO problems(description, entityTypes)
VALUES(N'Не вмикається', '#any');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не вимикається', '#any');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не друкує', 'printer');

INSERT INTO problems(description, entityTypes)
VALUES(N'Некоректна передача кольору', 'monitor,printer');

INSERT INTO problems(description, entityTypes)
VALUES(N'Некоректний розмір зображення', 'monitor');

INSERT INTO problems(description, entityTypes)
VALUES(N'Зображення розтягнуте', 'monitor');

INSERT INTO problems(description, entityTypes)
VALUES(N'Зображення стисле', 'monitor');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не працює кнопка живлення', '#any');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не працює дисковід', 'laptop,computer');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не працює жорстки диск', 'laptop,computer');

INSERT INTO problems(description, entityTypes)
VALUES(N'Часте вимкнення', '#any');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не працює катридж', 'printer');

INSERT INTO problems(description, entityTypes)
VALUES(N'Не друкуються листки', 'printer');



INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Увімкнути в розетку');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Увімкнути в розетку');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Увімкнути в розетку');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Стукнути тапком');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Стукнути тапком');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Попити пива');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Попити пива');

INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Потанцювати з бубном');