CREATE TRIGGER Visitor_INSERT_2
ON Авторизация
AFTER INSERT
AS
BEGIN
DECLARE @rand int, @ind int, @emal nvarchar(max)
SET @rand = (SELECT FLOOR(100 + RAND() * (9999 - 100 + 1)))
SET @emal = (select Email from Посетитель WHERE [Код посетителя] = (SELECT [Код посетителя] FROM inserted))
SET @ind = CHARINDEX('@', @emal)
UPDATE Авторизация
SET Логин = CONCAT(@emal, @rand)
WHERE [Код авторизации] = (SELECT [Код авторизации] FROM inserted)
END