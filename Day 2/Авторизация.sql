CREATE PROCEDURE Autorisation (@Login VARCHAR(250), @Password NVARCHAR(250))
AS
BEGIN
    SELECT * FROM Авторизация WHERE Логин = @Login AND Пароль = @Password
END