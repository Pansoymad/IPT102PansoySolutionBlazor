using System;
using IPT102PansoyWPF.ViewModels;

namespace IPT102PansoyWPF.Commands
{
    public class UpdateMovieCommand : BaseCommand
    {
        private readonly AddMovieViewModel _viewModel;

        public UpdateMovieCommand(AddMovieViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _ = _viewModel.UpdateMovie();
        }
    }
}
