using System;
using IPT102PansoyWPF.ViewModels;

namespace IPT102PansoyWPF.Commands
{
    public class AddMovie : BaseCommand
    {
        private readonly AddMovieViewModel _viewModel;

        public AddMovie(AddMovieViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.SaveMovie();
        }
    }
}
