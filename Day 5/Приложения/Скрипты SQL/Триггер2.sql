CREATE TRIGGER Visitor_INSERT_2
ON �����������
--������� ����� ���������� ������
AFTER INSERT
AS
BEGIN
--����������
DECLARE @rand int, @ind int, @emal nvarchar(max)
--������������� ���������� �����
SET @rand = (SELECT FLOOR(100 + RAND() * (9999 - 100 + 1)))
--������� ����� ����������
SET @emal = (select Email from ���������� WHERE [��� ����������] = (SELECT [��� ����������] FROM inserted))
--����� ������� ��������� �������
SET @ind = CHARINDEX('@', @emal)
--���������� ������ � ������� �����������
UPDATE �����������
--��������� ����������� ������
SET ����� = CONCAT(LEFT(@emal, @ind-1), @rand)
WHERE [��� �����������] = (SELECT [��� �����������] FROM inserted)
END