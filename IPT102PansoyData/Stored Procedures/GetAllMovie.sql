CREATE PROCEDURE [dbo].[GetAllMovie]
AS
BEGIN
    SELECT * 
    FROM [dbo].[Movie];
END;
GO