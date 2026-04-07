
    using Domain.Models;

    namespace Domain.Commands
    {
        public interface ICreateCommand
        {
            Task ExecuteAsync(Movie model);

        }
    }
