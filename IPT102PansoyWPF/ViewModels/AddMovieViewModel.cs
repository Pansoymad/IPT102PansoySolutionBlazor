using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Domain.Commands;
using Domain.Models;
using Domain.Queries;
using IPT102PansoyWPF.Commands;
using IPT102PansoyWPF.Services;

namespace IPT102PansoyWPF.ViewModels
{
    public class AddMovieViewModel : BaseViewModel
    {
        private readonly ICreateCommand _createMovie;
        private readonly IGetAllMovie _getAllMovie;
        private readonly IUpdateCommand _updateMovie;
        private readonly IDeleteCommand _deleteMovie;

        private string _title = string.Empty;
        private string _releaseYear = string.Empty;
        private string _runtimeMinutes = string.Empty;
        private string _rating = string.Empty;
        private string _searchText = string.Empty;
        private bool _isEditMode = false;
        private int _currentMovieId;
        private readonly ObservableCollection<Movie> _allMovies = new();

        public AddMovieViewModel(
            ICreateCommand createMovie,
            IGetAllMovie getAllMovie,
            IUpdateCommand updateMovie,
            IDeleteCommand deleteMovie,
            INavigationService homeNavigationService)
        {
            _createMovie = createMovie;
            _getAllMovie = getAllMovie;
            _updateMovie = updateMovie;
            _deleteMovie = deleteMovie;

            Movies = new ObservableCollection<Movie>();

            SaveCommand = new AddMovie(this);
            UpdateCommand = new UpdateMovieCommand(this);
            DeleteCommand = new DeleteCommand(this);
            EditCommand = new EditMovieCommand(this);
            CancelCommand = new OpenHomeCommand(homeNavigationService);

            _ = LoadMoviesAsync();
        }

        public ObservableCollection<Movie> Movies { get; }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string ReleaseYear
        {
            get => _releaseYear;
            set { _releaseYear = value; OnPropertyChanged(nameof(ReleaseYear)); }
        }

        public string RuntimeMinutes
        {
            get => _runtimeMinutes;
            set { _runtimeMinutes = value; OnPropertyChanged(nameof(RuntimeMinutes)); }
        }

        public string Rating
        {
            get => _rating;
            set { _rating = value; OnPropertyChanged(nameof(Rating)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterMovies(); }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(IsAddMode));
                OnPropertyChanged(nameof(SaveButtonText));
            }
        }

        public bool IsAddMode => !IsEditMode;
        public string SaveButtonText => IsEditMode ? "Update" : "Save";

        public ICommand SaveCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CancelCommand { get; }

        public async void SaveMovie()
        {
            if (IsEditMode) { await UpdateMovie(); return; }

            if (string.IsNullOrWhiteSpace(Title))
            {
                System.Windows.MessageBox.Show("Title is required.", "Validation",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (decimal.TryParse(Rating, out var ratingVal) && (ratingVal < 0 || ratingVal > 10))
            {
                System.Windows.MessageBox.Show("Rating must be between 0.0 and 10.0.", "Validation",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                var movie = new Movie
                {
                    Title = Title,
                    ReleaseYear = int.TryParse(ReleaseYear, out var ry) ? ry : null,
                    RuntimeMinutes = int.TryParse(RuntimeMinutes, out var rm) ? rm : null,
                    Rating = decimal.TryParse(Rating, out var r) ? r : null
                };

                await _createMovie.ExecuteAsync(movie);
                ClearForm();
                await LoadMoviesAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error saving: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async Task UpdateMovie()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                System.Windows.MessageBox.Show("Title is required.", "Validation",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (decimal.TryParse(Rating, out var ratingVal) && (ratingVal < 0 || ratingVal > 10))
            {
                System.Windows.MessageBox.Show("Rating must be between 0.0 and 10.0.", "Validation",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                var movie = new Movie
                {
                    MovieId = _currentMovieId,
                    Title = Title,
                    ReleaseYear = int.TryParse(ReleaseYear, out var ry) ? ry : null,
                    RuntimeMinutes = int.TryParse(RuntimeMinutes, out var rm) ? rm : null,
                    Rating = decimal.TryParse(Rating, out var r) ? r : null
                };

                await _updateMovie.ExecuteAsync(movie);
                ClearForm();
                IsEditMode = false;
                await LoadMoviesAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error updating: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async void DeleteMovie(Movie movie)
        {
            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to delete \"{movie.Title}\"?",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result != System.Windows.MessageBoxResult.Yes) return;

            try
            {
                await _deleteMovie.ExecuteAsync(movie);
                await LoadMoviesAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error deleting: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void LoadMovieForEdit(Movie movie)
        {
            _currentMovieId = movie.MovieId;
            Title = movie.Title;
            ReleaseYear = movie.ReleaseYear?.ToString() ?? string.Empty;
            RuntimeMinutes = movie.RuntimeMinutes?.ToString() ?? string.Empty;
            Rating = movie.Rating?.ToString() ?? string.Empty;
            IsEditMode = true;
        }

        private void ClearForm()
        {
            Title = string.Empty;
            ReleaseYear = string.Empty;
            RuntimeMinutes = string.Empty;
            Rating = string.Empty;
            IsEditMode = false;
            _currentMovieId = 0;
        }

        private async Task LoadMoviesAsync()
        {
            var movies = await _getAllMovie.ExecuteAsync();

            _allMovies.Clear();
            Movies.Clear();

            if (movies != null)
            {
                foreach (var m in movies)
                {
                    _allMovies.Add(m);
                    Movies.Add(m);
                }
            }
        }

        private void FilterMovies()
        {
            Movies.Clear();

            var source = string.IsNullOrWhiteSpace(SearchText)
                ? _allMovies
                : _allMovies.Where(m =>
                    m.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    (m.ReleaseYear?.ToString().Contains(SearchText) ?? false) ||
                    (m.Rating?.ToString().Contains(SearchText) ?? false));

            foreach (var m in source)
                Movies.Add(m);
        }
    }
}
