CREATE PROCEDURE [dbo].[GetMovie]
    @Id INT = NULL
AS
    SELECT
        Id,
        Title,
        ReleaseYear,
        RuntimeMinutes,
        Rating
    FROM [dbo].[Movie]
    WHERE Id = @Id;
