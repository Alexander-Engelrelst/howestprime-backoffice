using Howestprime.Backoffice.ViewModels;
using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;
using Microsoft.AspNetCore.Components;

namespace Howestprime.Backoffice.Components.Pages;

public partial class Planning : ComponentBase
{
    [Inject] 
    private IMovieEventsApiClient MovieEventsApiClient { get; set; }
    
    private PlanningViewModel ViewModel { get; init; } = new();
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        FindMovieEventsForMonthRequest request = new()
        {
            Year = ViewModel.NavigatorViewModel.SelectedYear,
            Month = ViewModel.NavigatorViewModel.SelectedMonth
        };
        
        ApiResult<IReadOnlyList<MovieEvent>> movieEvents = 
            await MovieEventsApiClient.FindMovieEventsForMonthAsync(request);

        if (movieEvents.IsFailure)
        {
            // TODO this must show an error using an overlay
            return;
        }
        
        ViewModel.UpdateMovieEvents(movieEvents.Value!);
    }
}