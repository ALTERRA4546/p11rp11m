CREATE TRIGGER Visitor_INSERT
ON �����������
AFTER INSERT
AS
BEGIN
DECLARE @a int, @ind int, @emal nvarchar(max)
SET @a = (select [��� ����������] from ���������� WHERE [��� ����������] = (SELECT [��� ����������] FROM inserted))
SET @emal = (select Email from ���������� WHERE [��� ����������] = (SELECT [��� ����������] FROM inserted))
SET @ind = CHARINDEX('@', @emal)
UPDATE �����������
SET ����� = CONCAT (LEFT(@emal, @ind-1), @a)
WHERE [��� �����������] = (SELECT [��� �����������] FROM inserted)
END