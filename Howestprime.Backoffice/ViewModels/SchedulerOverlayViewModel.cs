using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.ViewModels;

public class SchedulerOverlayViewModel
{
    public MovieEventFormViewModel Form { get; init; } = new();
    
    // TODO ask if this is allowed
    public bool IsOpen { get; set; } = false;
    public DateTime LastMovieListUpdate { get; set; } = DateTime.MinValue;
    
    public string ErrorMessage { get; set; } = String.Empty;
    public IDictionary<Guid, string> Movies { get; init; } = new Dictionary<Guid, string>();
    
    public IList<TimeOnly> AvailableTimes { get; init; }
        = new List<TimeOnly> { new(15,0), new(19,0) };
    
    public IDictionary<Guid, string> Rooms { get; init; } 
        = new Dictionary<Guid, string>
        {
            {new Guid("019d059e-d220-71db-8a1a-ec7569492999"), "Blue Room"},
            {new Guid("019d059e-d220-75fe-b936-0a97cd75216e"), "Yellow Room"}
        };

    public void UpdateMovies(MovieCollection movies)
    {
        Movies.Clear();

        foreach (Movie movie in movies.Data)
        {
            Movies.Add(movie.Id, movie.Title);
        }
        
        LastMovieListUpdate = DateTime.UtcNow;
    }
}