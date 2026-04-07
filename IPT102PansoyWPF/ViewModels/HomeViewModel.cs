using System.Windows.Input;
using IPT102PansoyWPF.Commands;
using IPT102PansoyWPF.Services;

namespace IPT102PansoyWPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand NavigateAddMovieCommand { get; }

        public HomeViewModel(INavigationService addMovieNavigationService)
        {
            NavigateAddMovieCommand = new OpenAddMovieCommand(addMovieNavigationService);
        }
    }
}
