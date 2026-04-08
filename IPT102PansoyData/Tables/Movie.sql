CREATE TABLE [dbo].[Movie]
(
    [MovieId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(200) NOT NULL,
    [ReleaseYear] INT NULL,
    [RuntimeMinutes] INT NULL,
    [Rating] DECIMAL(3,1) NULL
)
