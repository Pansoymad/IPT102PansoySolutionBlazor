namespace Domain.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? ReleaseYear { get; set; }
        public int? RuntimeMinutes { get; set; }
        public decimal? Rating { get; set; }
    }
}
