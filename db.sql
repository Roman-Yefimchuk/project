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

CREATE TABLE entityAccessories(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                             name NVARCHAR(MAX),
                             entityTypes TEXT,
                             ports NVARCHAR(MAX));

CREATE TABLE problems(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                      description NVARCHAR(MAX),
                      entityTypes TEXT);

CREATE TABLE userSolutions(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                           entityTypeId INTEGER,
                           entityModelId INTEGER,
                           problemId INTEGER,
                           solution NVARCHAR(MAX));

CREATE TABLE masterSolutions(id INTEGER NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                             entityTypeId INTEGER,
                             entityAccessoryId INTEGER,
                             voltageValues TEXT,
                             problem NVARCHAR(MAX),
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



INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Материнська плата', 'computer,laptop', N'port1,Порт 1|port2,Порт 2|port3,Порт 3|port4,Порт 4');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Дисковід', 'computer,laptop', N'port1,Порт 1|port2,Порт 2');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Жорсткий диск', 'computer,laptop', N'port1,Порт 1|port2,Порт 2|port3,Порт 3');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Системна плата', 'printer', N'port1,Порт 1|port2,Порт 2|port3,Порт 3|port4,Порт 4');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Системна плата', 'monitor', N'port1,Порт 1|port2,Порт 2|port3,Порт 3');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Wi-Fi модуль', 'laptop', N'port1,Порт 1');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Катридж', 'printer', N'port1,Порт 1|port2,Порт 2');

INSERT INTO entityAccessories(name, entityTypes, ports)
VALUES(N'Блок живлення', '#any', N'port1,Порт 1|port2,Порт 2|port3,Порт 3');



INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Увімкнути в розетку');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Увімкнути в розетку');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Увімкнути в розетку');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Стукнути тапком');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Стукнути тапком');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Попити пива');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Попити пива');

INSERT INTO userSolutions(entityTypeId, entityModelId, problemId, solution)
VALUES(1/*laptop*/, 1/*Apple*/, 1/*Не вмикається*/, N'Потанцювати з бубном');



INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=12.5,port2=10.4,port3=2.6,port4=5.7', N'Проблема 1', N'Вирішення 1');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=12,port2=11.4,port3=2.4,port4=6.0', N'Проблема 2', N'Вирішення 2');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=20.1,port2=13.4,port3=1.5,port4=3.7', N'Проблема 3', N'Вирішення 3');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=10.1,port2=12.3,port3=1.4,port4=9.7', N'Проблема 4', N'Вирішення 4');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=11.5,port2=11.4,port3=1.6,port4=1.7', N'Проблема 5', N'Вирішення 5');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=9.5,port2=7.4,port3=4.6,port4=5.7', N'Проблема 6', N'Вирішення 6');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=8.5,port2=4.4,port3=2.2,port4=5.1', N'Проблема 7', N'Вирішення 7');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=2.5,port2=6.4,port3=2.5,port4=5.9', N'Проблема 8', N'Вирішення 8');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=14.5,port2=13.4,port3=1.2,port4=1.7', N'Проблема 9', N'Вирішення 9');

INSERT INTO masterSolutions(entityTypeId, entityAccessoryId, voltageValues, problem, solution)
VALUES(1/*Ноутбук*/, 1/*Материнська плата*/, 'port1=14.8,port2=6.4,port3=5.6,port4=4.7', N'Проблема 10', N'Вирішення 10');