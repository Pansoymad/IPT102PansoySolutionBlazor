CREATE PROCEDURE [dbo].[CreateMovie]
    @Title NVARCHAR(200),
    @ReleaseYear INT = NULL,
    @RuntimeMinutes INT = NULL,
    @Rating DECIMAL(3,1) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Movie]
        (Title, ReleaseYear, RuntimeMinutes, Rating)
    VALUES
        (@Title, @ReleaseYear, @RuntimeMinutes, @Rating);
END
