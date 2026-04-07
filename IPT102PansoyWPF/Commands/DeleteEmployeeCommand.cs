using System;
using Domain.Models;
using IPT102PansoyWPF.ViewModels;

namespace IPT102PansoyWPF.Commands
{
    public class DeleteCommand : BaseCommand
    {
        private readonly AddMovieViewModel _viewModel;

        public DeleteCommand(AddMovieViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is Movie Movie)
            {
                _viewModel.DeleteMovie(Movie);
            }
        }
    }
}
