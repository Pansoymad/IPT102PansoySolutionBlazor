using Domain.Models;


namespace Domain.Commands
{
    public interface IUpdateCommand
    {
        Task ExecuteAsync(Movie model);
    }
}
