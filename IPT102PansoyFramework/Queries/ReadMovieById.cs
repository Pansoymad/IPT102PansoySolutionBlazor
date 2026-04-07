using Dapper;
using Domain.Models;
using Domain.Queries;
using System.Linq;
using System.Threading.Tasks;

namespace IPT102PansoyFramework.Queries
{
    public class ReadMovieById : IReadMovieById
    {
        private readonly Repository _repository;

        public ReadMovieById(Repository repository)
        {
            _repository = repository;
        }

        public async Task<Movie?> ExecuteAsync(int movieId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", movieId);

            var data = await _repository.GetDataAsync<Movie>(
                "DefaultConnection",
                "[dbo].[ReadMovieById]",
                parameters
            );

            return data?.FirstOrDefault();
        }
    }
}