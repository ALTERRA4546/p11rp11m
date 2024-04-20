CREATE PROCEDURE Registration (@Family VARCHAR(250), @Name NVARCHAR(250), @Otchestvo NVARCHAR(250), @Phone NVARCHAR(18), @Email NVARCHAR(250), @Organisation NVARCHAR(250), @Birthday NVARCHAR(250), @Series INT, @Number INT, @Login NVARCHAR(250), @Password NVARCHAR(250))
AS
BEGIN
	DECLARE @Kode INT;

	IF EXISTS (SELECT * FROM Организация WHERE Название = @Organisation)
		SELECT @Kode = [Код организации] FROM Организация WHERE Название = @Organisation;
	ELSE
	BEGIN
		INSERT INTO Организация VALUES (@Organisation)
		SELECT TOP 1 @Kode = [Код организации] FROM Организация ORDER BY [Код организации] DESC;
	END
    INSERT INTO Посетитель (Фамилия, Имя, Отчество, [Номер телефона], Email, [Дата рождения], [Код организации], [Серия паспорта], [Номер паспорта]) VALUES (@Family, @Name, @Otchestvo, @Phone, @Email, @Birthday, @Kode, @Series, @Number)
	SELECT TOP 1 @Kode = [Код посетителя] FROM Посетитель ORDER BY [Код организации] DESC;
	INSERT INTO Авторизация VALUES (@Kode, @Login, @Password)
END