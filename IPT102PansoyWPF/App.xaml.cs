using System.Windows;
using Domain.Commands;
using Domain.Queries;
using Framework;
using IPT102PansoyFramework.Commands;
using IPT102PansoyFramework.Queries;
using IPT102PansoyFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IPT102PansoyWPF.Services;
using IPT102PansoyWPF.Stores;
using IPT102PansoyWPF.ViewModels;
using IPT102PansoyWPF.Views;

namespace IPT102PansoyWPF
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider = null!;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // Configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            services.AddSingleton<IConfiguration>(config);

            // Repository & DB setup
            services.AddSingleton<IPT102PansoyFramework.Repository>();
            services.AddSingleton<DatabaseAutoSetup>();

            // Domain commands & queries
            services.AddTransient<ICreateCommand, CreateCommand>();
            services.AddTransient<IUpdateCommand, UpdateCommand>();
            services.AddTransient<IDeleteCommand, DeleteCommand>();
            services.AddTransient<IGetAllMovie, GetAllMovie>();
            services.AddTransient<IReadMovieById, ReadMovieById>();

            // Navigation
            services.AddSingleton<NavigationStore>();

            services.AddTransient<HomeViewModel>(sp => new HomeViewModel(
                new NavigationService<AddMovieViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<AddMovieViewModel>()
                )
            ));

            services.AddTransient<AddMovieViewModel>(sp => new AddMovieViewModel(
                sp.GetRequiredService<ICreateCommand>(),
                sp.GetRequiredService<IGetAllMovie>(),
                sp.GetRequiredService<IUpdateCommand>(),
                sp.GetRequiredService<IDeleteCommand>(),
                new NavigationService<HomeViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<HomeViewModel>()
                )
            ));

            services.AddSingleton<MainViewModel>(sp => new MainViewModel(
                sp.GetRequiredService<NavigationStore>()
            ));

            services.AddSingleton<MainView>(sp =>
            {
                var window = new MainView();
                window.DataContext = sp.GetRequiredService<MainViewModel>();
                return window;
            });

            _serviceProvider = services.BuildServiceProvider();

            // Show window immediately
            var navStore = _serviceProvider.GetRequiredService<NavigationStore>();
            navStore.CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();

            var mainWindow = _serviceProvider.GetRequiredService<MainView>();
            mainWindow.Show();

            // Run DB setup in background, show error if it fails
            var dbSetup = _serviceProvider.GetRequiredService<DatabaseAutoSetup>();
            _ = dbSetup.EnsureDatabaseSetupAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        System.Windows.MessageBox.Show(
                            $"Database setup failed:\n{t.Exception?.InnerException?.Message ?? t.Exception?.Message}",
                            "DB Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    });
                }
            });
        }
    }
}
