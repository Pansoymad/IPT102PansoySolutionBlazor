using System;
using System.Collections.Generic;
using System.Text;
using IPT102PansoyFramework.Extensions;
using Domain.Models;
using Domain.Commands;
namespace IPT102PansoyFramework.Commands
{
    public class DeleteCommand : IDeleteCommand
    {
        private readonly Repository _repository;

        public DeleteCommand(Repository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Movie model)
        {
            var parameters = model.ToDeleteMovieDynamicParameters();
            await _repository.SaveDataAsync("DefaultConnection", "[dbo].[DeleteMovie]", parameters);
        }
    }
}
