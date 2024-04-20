--Авторизация пользователя
CREATE PROCEDURE Autorisation (@Login VARCHAR(250), @Password NVARCHAR(250)) 
AS
BEGIN
--Выбор подходящих посетителей
    SELECT * FROM Авторизация WHERE Логин = @Login AND Пароль = @Password
END