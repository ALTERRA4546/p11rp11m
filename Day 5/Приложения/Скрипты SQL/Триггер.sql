CREATE TRIGGER Visitor_INSERT
ON Авторизация
--Событие после добавления записи
AFTER INSERT
AS
BEGIN
--Переменные
DECLARE @a int, @ind int, @emal nvarchar(max)
--Выборка кода посетителя
SET @a = (select [Код посетителя] from Посетитель WHERE [Код посетителя] = (SELECT [Код посетителя] FROM inserted))
--Выборка почты посетителя
SET @emal = (select Email from Посетитель WHERE [Код посетителя] = (SELECT [Код посетителя] FROM inserted))
--Поиск позиции заданного символа
SET @ind = CHARINDEX('@', @emal)
--Обнавление записи в таблице авторизации
UPDATE Авторизация
--Генерация уникального логина
SET Логин = CONCAT (LEFT(@emal, @ind-1), @a)
WHERE [Код авторизации] = (SELECT [Код авторизации] FROM inserted)
END