--Регистрация посетителей
CREATE PROCEDURE Registration (@Family VARCHAR(250), @Name NVARCHAR(250), @Otchestvo NVARCHAR(250), @Phone NVARCHAR(18), @Email NVARCHAR(250), @Organisation NVARCHAR(250), @Birthday NVARCHAR(250), @Series INT, @Number INT, @Login NVARCHAR(250), @Password NVARCHAR(250))
AS
BEGIN
	--Переменая
	DECLARE @Kode INT;
	--Проверка существует ли организация
	IF EXISTS (SELECT * FROM Организация WHERE Название = @Organisation)
	--Если существует выборка кода организации
		SELECT @Kode = [Код организации] FROM Организация WHERE Название = @Organisation;
	ELSE
	--Если не существует создание и выборка кода организации
	BEGIN
		INSERT INTO Организация VALUES (@Organisation)
		SELECT TOP 1 @Kode = [Код организации] FROM Организация ORDER BY [Код организации] DESC;
	END
	--Добавление посетителя
    INSERT INTO Посетитель (Фамилия, Имя, Отчество, [Номер телефона], Email, [Дата рождения], [Код организации], [Серия паспорта], [Номер паспорта]) VALUES (@Family, @Name, @Otchestvo, @Phone, @Email, @Birthday, @Kode, @Series, @Number)
	--Выборка кода добавленного посетителя
	SELECT TOP 1 @Kode = [Код посетителя] FROM Посетитель ORDER BY [Код организации] DESC;
	--Добавление посетителя в авторизацию
	INSERT INTO Авторизация VALUES (@Kode, @Login, @Password)
END