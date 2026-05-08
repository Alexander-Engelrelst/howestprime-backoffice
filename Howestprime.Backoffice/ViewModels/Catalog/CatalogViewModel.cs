using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.ViewModels.Catalog;

public class CatalogViewModel
{
    public IReadOnlyList<CatalogMovieViewModel> Movies { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public bool IsLoading { get; set; } = true;
    public void UpdateMovies(MovieCollection movies)
    {
        Movies = movies.Data.Select(m => new CatalogMovieViewModel
        {
            Id = m.Id,
            Title = m.Title,
            PosterUrl = m.PosterUrl,
            ReleaseYear = m.ReleaseYear
        }).ToList();
    }
}