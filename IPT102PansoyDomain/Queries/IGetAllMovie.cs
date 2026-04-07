using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
namespace Domain.Queries
{
    public interface IGetAllMovie
    {
        Task<IEnumerable<Movie>?> ExecuteAsync();

    }
}
