namespace Howestprime.Backoffice.ViewModels.Catalog;

public class CatalogMovieViewModel
{
    public int ReleaseYear { get; set; }
    public string Title { get; set; } = String.Empty;
    public string PosterUrl { get; set; }  = String.Empty;
    public Guid Id { get; set; }
}