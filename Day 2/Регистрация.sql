CREATE PROCEDURE Registration (@Family VARCHAR(250), @Name NVARCHAR(250), @Otchestvo NVARCHAR(250), @Phone NVARCHAR(18), @Email NVARCHAR(250), @Organisation NVARCHAR(250), @Birthday NVARCHAR(250), @Series INT, @Number INT, @Login NVARCHAR(250), @Password NVARCHAR(250))
AS
BEGIN
	DECLARE @Kode INT;

	IF EXISTS (SELECT * FROM ����������� WHERE �������� = @Organisation)
		SELECT @Kode = [��� �����������] FROM ����������� WHERE �������� = @Organisation;
	ELSE
	BEGIN
		INSERT INTO ����������� VALUES (@Organisation)
		SELECT TOP 1 @Kode = [��� �����������] FROM ����������� ORDER BY [��� �����������] DESC;
	END
    INSERT INTO ���������� (�������, ���, ��������, [����� ��������], Email, [���� ��������], [��� �����������], [����� ��������], [����� ��������]) VALUES (@Family, @Name, @Otchestvo, @Phone, @Email, @Birthday, @Kode, @Series, @Number)
	SELECT TOP 1 @Kode = [��� ����������] FROM ���������� ORDER BY [��� �����������] DESC;
	INSERT INTO ����������� VALUES (@Kode, @Login, @Password)
END