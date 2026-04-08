using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IPT102PansoyFramework
{
    public class DatabaseAutoSetup
    {
        private readonly IConfiguration _configuration;

        public DatabaseAutoSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnsureDatabaseSetupAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            var masterConnectionString = connectionString.Replace($"Initial Catalog={databaseName}", "Initial Catalog=master");

            await CreateDatabaseIfNotExistsAsync(masterConnectionString, databaseName);
            await CreateTableIfNotExistsAsync(connectionString);
            await CreateStoredProceduresAsync(connectionString);
        }

        private async Task CreateDatabaseIfNotExistsAsync(string masterConnectionString, string databaseName)
        {
            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();

            using var checkCmd = new SqlCommand($"SELECT database_id FROM sys.databases WHERE name = '{databaseName}'", connection);
            var result = await checkCmd.ExecuteScalarAsync();

            if (result == null)
            {
                using var createCmd = new SqlCommand($"CREATE DATABASE [{databaseName}]", connection);
                await createCmd.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateTableIfNotExistsAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movie')
                BEGIN
                    CREATE TABLE [dbo].[Movie]
                    (
                        [MovieId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [Title] NVARCHAR(200) NOT NULL,
                        [ReleaseYear] INT NULL,
                        [RuntimeMinutes] INT NULL,
                        [Rating] DECIMAL(3,1) NULL
                    );
                END";

            using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        private async Task CreateStoredProceduresAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var procedures = new[]
            {
                "IF OBJECT_ID('[dbo].[CreateMovie]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[CreateMovie];",
                @"CREATE PROCEDURE [dbo].[CreateMovie]
                    @Title NVARCHAR(200), @ReleaseYear INT = NULL, @RuntimeMinutes INT = NULL, @Rating DECIMAL(3,1) = NULL
                AS BEGIN
                    INSERT INTO [dbo].[Movie] (Title, ReleaseYear, RuntimeMinutes, Rating)
                    VALUES (@Title, @ReleaseYear, @RuntimeMinutes, @Rating);
                END",

                "IF OBJECT_ID('[dbo].[UpdateMovie]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[UpdateMovie];",
                @"CREATE PROCEDURE [dbo].[UpdateMovie]
                    @Id INT, @Title NVARCHAR(200), @ReleaseYear INT = NULL, @RuntimeMinutes INT = NULL, @Rating DECIMAL(3,1) = NULL
                AS BEGIN
                    UPDATE [dbo].[Movie] SET Title=@Title, ReleaseYear=@ReleaseYear, RuntimeMinutes=@RuntimeMinutes, Rating=@Rating
                    WHERE MovieId=@Id;
                END",

                "IF OBJECT_ID('[dbo].[DeleteMovie]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[DeleteMovie];",
                @"CREATE PROCEDURE [dbo].[DeleteMovie]
                    @Id INT
                AS BEGIN
                    DELETE FROM [dbo].[Movie] WHERE MovieId=@Id;
                END",

                "IF OBJECT_ID('[dbo].[GetAllMovie]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[GetAllMovie];",
                @"CREATE PROCEDURE [dbo].[GetAllMovie]
                AS BEGIN
                    SELECT * FROM [dbo].[Movie] ORDER BY MovieId;
                END",

                "IF OBJECT_ID('[dbo].[ReadMovieById]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[ReadMovieById];",
                @"CREATE PROCEDURE [dbo].[ReadMovieById]
                    @Id INT
                AS BEGIN
                    SELECT * FROM [dbo].[Movie] WHERE MovieId=@Id;
                END"
            };

            foreach (var sql in procedures)
            {
                using var command = new SqlCommand(sql, connection);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
