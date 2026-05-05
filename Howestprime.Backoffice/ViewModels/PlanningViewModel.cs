using Howestprime.Backoffice.Extensions;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.ViewModels;

public class PlanningViewModel
{
    public NavigatorViewModel NavigatorViewModel { get; private init; } = new();
    public LegendViewModel LegendViewModel { get; private init; } = new();
    
    public Dictionary<int, DayCellViewModel> DayCellViewModels { get; private set; } = new();
    
    public void UpdateMovieEvents(IReadOnlyList<MovieEvent> movieEvents)
    {
        DayCellViewModels.Clear();

        ILookup<int, MovieEventViewModel> movieLookup = movieEvents.Select(me => me.ToViewModel()).ToLookup(me => me.DateTime.Day);
        
        int daysInMonth = DateTime.DaysInMonth(NavigatorViewModel.SelectedYear, NavigatorViewModel.SelectedMonth);

        DayCellViewModels = Enumerable.Range(1, daysInMonth)
            .ToDictionary(day => day, day => new DayCellViewModel
            {
                MovieEvents =  movieLookup[day].ToList()
            });
    }
}
