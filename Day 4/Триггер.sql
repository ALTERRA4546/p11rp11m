CREATE TRIGGER Visitor_INSERT
ON Авторизация
AFTER INSERT
AS
BEGIN
DECLARE @a int, @ind int, @emal nvarchar(max)
SET @a = (select [Код посетителя] from Посетитель WHERE [Код посетителя] = (SELECT [Код посетителя] FROM inserted))
SET @emal = (select Email from Посетитель WHERE [Код посетителя] = (SELECT [Код посетителя] FROM inserted))
SET @ind = CHARINDEX('@', @emal)
UPDATE Авторизация
SET Логин = CONCAT (LEFT(@emal, @ind-1), @a)
WHERE [Код авторизации] = (SELECT [Код авторизации] FROM inserted)
END