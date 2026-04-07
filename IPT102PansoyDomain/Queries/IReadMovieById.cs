using Domain.Models;
namespace Domain.Queries
{
    public interface IReadMovieById
    {
        Task<Movie?> ExecuteAsync(int movieId);

    }
}
