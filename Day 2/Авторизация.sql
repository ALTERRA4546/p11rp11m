CREATE PROCEDURE Autorisation (@Login VARCHAR(250), @Password NVARCHAR(250))
AS
BEGIN
    SELECT * FROM ����������� WHERE ����� = @Login AND ������ = @Password
END