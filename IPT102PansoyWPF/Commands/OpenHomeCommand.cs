using System;
using IPT102PansoyWPF.Services;

namespace IPT102PansoyWPF.Commands
{
    public class OpenHomeCommand : BaseCommand
    {
        private readonly INavigationService _navigationService;

        public OpenHomeCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
