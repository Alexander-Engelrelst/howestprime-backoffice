using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.ViewModels.Planning;

public class SchedulerOverlayViewModel
{
    private const int MINUTES_BETWEEN_MOVIE_LIST_UPDATES = 5;
    public MovieEventFormViewModel Form { get; init; } = new();
    
    public bool IsOpen { get; set; } = false;
   
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

    public bool MustRefreshMovies 
        => !LastMovieListUpdate.HasValue 
           || DateTime.UtcNow > LastMovieListUpdate.Value.AddMinutes(MINUTES_BETWEEN_MOVIE_LIST_UPDATES);
    public bool IsLoading { get; set; } = false;
    private DateTime? LastMovieListUpdate { get; set; }

    public void UpdateMovies(MovieCollection movies)
    {
        Movies.Clear();

        foreach (Movie movie in movies.Data)
        {
            Movies.Add(movie.Id, movie.Title);
        }
        
        LastMovieListUpdate = DateTime.UtcNow;
    }

    public void PrepareForNewEntry()
    {
        ErrorMessage = String.Empty;
        Form.MovieId = null;
    }
}