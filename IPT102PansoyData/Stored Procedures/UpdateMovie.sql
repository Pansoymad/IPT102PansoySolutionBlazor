CREATE PROCEDURE [dbo].[UpdateMovie]
    @Id INT,
    @Title NVARCHAR(200),
    @ReleaseYear INT = NULL,
    @RuntimeMinutes INT = NULL,
    @Rating DECIMAL(3,1) = NULL
AS
    UPDATE [dbo].[Movie]
    SET
        Title = @Title,
        ReleaseYear = @ReleaseYear,
        RuntimeMinutes = @RuntimeMinutes,
        Rating = @Rating
    WHERE MovieId = @Id;
