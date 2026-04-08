CREATE PROCEDURE [dbo].[GetMovie]
    @Id INT = NULL
AS
    SELECT
        MovieId,
        Title,
        ReleaseYear,
        RuntimeMinutes,
        Rating
    FROM [dbo].[Movie]
    WHERE MovieId = @Id;
