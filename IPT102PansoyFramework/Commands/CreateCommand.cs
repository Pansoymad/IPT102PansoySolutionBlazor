using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Commands;
using IPT102PansoyFramework.Extensions;
namespace IPT102PansoyFramework.Commands
{
    public class CreateCommand : ICreateCommand
    {
            private readonly Repository _repository;

            public CreateCommand(Repository repository)
            {
                _repository = repository;
            }

            public async Task ExecuteAsync(Movie movie)
            {
                var parameters = movie.ToCreateMovieDynamicParameters();
                await _repository.SaveDataAsync("DefaultConnection", "[dbo].[CreateMovie]", parameters);
            }
        }
}
