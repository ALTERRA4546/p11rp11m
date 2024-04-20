CREATE TRIGGER Visitor_INSERT_2
ON Авторизация
--Событие после добавления записи
AFTER INSERT
AS
BEGIN
--Переменные
DECLARE @rand int, @ind int, @emal nvarchar(max)
--Генерирование случайного числа
SET @rand = (SELECT FLOOR(100 + RAND() * (9999 - 100 + 1)))
--Выборка почты посетителя
SET @emal = (select Email from Посетитель WHERE [Код посетителя] = (SELECT [Код посетителя] FROM inserted))
--Поиск позиции заданного символа
SET @ind = CHARINDEX('@', @emal)
--Обнавление записи в таблице авторизации
UPDATE Авторизация
--Генерация уникального логина
SET Логин = CONCAT(LEFT(@emal, @ind-1), @rand)
WHERE [Код авторизации] = (SELECT [Код авторизации] FROM inserted)
END