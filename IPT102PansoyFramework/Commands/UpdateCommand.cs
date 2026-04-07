using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Commands;  
using IPT102PansoyFramework.Extensions;
namespace IPT102PansoyFramework.Commands
{
        public class UpdateCommand : IUpdateCommand
        {
            private readonly Repository _repository;

            public UpdateCommand(Repository repository)
            {
                _repository = repository;
            }

            public async Task ExecuteAsync(Movie model)
            {
                var parameters = model.ToMovieDynamicParameters();
                await _repository.SaveDataAsync("DefaultConnection", "[dbo].[UpdateMovie]", parameters);
            }
        }
}
