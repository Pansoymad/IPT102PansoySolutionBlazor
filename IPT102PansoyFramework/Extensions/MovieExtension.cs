using Dapper;
using Domain.Models;

namespace IPT102PansoyFramework.Extensions
{
    public static class MovieExtensions
    {
        public static DynamicParameters ToCreateMovieDynamicParameters(this Movie movie)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Title", movie.Title);
            parameters.Add("@ReleaseYear", movie.ReleaseYear);
            parameters.Add("@RuntimeMinutes", movie.RuntimeMinutes);
            parameters.Add("@Rating", movie.Rating);
            return parameters;
        }

        public static DynamicParameters ToMovieDynamicParameters(this Movie movie)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", movie.MovieId);
            parameters.Add("@Title", movie.Title);
            parameters.Add("@ReleaseYear", movie.ReleaseYear);
            parameters.Add("@RuntimeMinutes", movie.RuntimeMinutes);
            parameters.Add("@Rating", movie.Rating);
            return parameters;
        }

        public static DynamicParameters ToDeleteMovieDynamicParameters(this Movie movie)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", movie.MovieId);
            return parameters;
        }
    }
}
