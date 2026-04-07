CREATE PROCEDURE [dbo].[DeleteMovie]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Movie]
    WHERE Id = @Id;
END
