using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Queries;
namespace IPT102PansoyFramework.Queries
{
    public class GetAllMovie : IGetAllMovie
    {
        private readonly Repository _repository;

        public GetAllMovie(Repository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Movie>?> ExecuteAsync()
        {
            return await _repository.GetDataAsync<Movie>("DefaultConnection", "[dbo].[GetAllMovie]", null);
        }
    }
}
