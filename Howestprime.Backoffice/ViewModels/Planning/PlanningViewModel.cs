using Howestprime.Backoffice.Extensions;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.ViewModels.Planning;

public class PlanningViewModel
{
    public NavigatorViewModel NavigatorViewModel { get; } = new();
    public LegendViewModel LegendViewModel { get; private init; } = new();
    
    public SchedulerOverlayViewModel SchedulerOverlayViewModel { get; private init; } = new();
    
    public Dictionary<int, DayCellViewModel> DayCellViewModels { get; private set; } = new();
    
    public string ErrorMessage { get; set; } = string.Empty;

    public PlanningViewModel()
    {
        // only here to prevent issues with latency while fetching data
        InitializeEmptyDayCellViewModel();
    }
    
    public void UpdateMovieEvents(IReadOnlyList<MovieEvent> movieEvents)
    {
        DayCellViewModels.Clear();

        ILookup<int, MovieEventViewModel> movieLookup = movieEvents
            .Select(me => me.ToViewModel())
            .ToLookup(me => me.ShowTime.Day);
        
        DayCellViewModels = Enumerable.Range(1, NavigatorViewModel.DaysInCurrentMonth)
            .ToDictionary(day => day, day => new DayCellViewModel
            {
                MovieEvents =  movieLookup[day].ToList()
            });
    }
    
    private void InitializeEmptyDayCellViewModel()
    {
        DayCellViewModels = Enumerable
            .Range(1, NavigatorViewModel.DaysInCurrentMonth)
            .ToDictionary(day => day, _ => new DayCellViewModel());
    }
}
