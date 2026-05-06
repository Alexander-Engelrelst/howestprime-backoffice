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
    private IMovieEventsApiClient MovieEventsApiClient { get; set; } = null!;
    
    [Inject]
    private IMovieCatalogApiClient MovieCatalogApiClient { get; set; } = null!;

    private PlanningViewModel ViewModel { get; init; } = new();
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await Task.WhenAll(FetchNewMovieEvents(), FetchMovies());
    }
    private async Task OnNavigateMonthAsync(int direction)
    {
        // update navigator state
        // TODO ensure movies aren't fetched if the user tries to force navigation it doesn't work
        ViewModel.NavigatorViewModel.Navigate(direction);
        await FetchNewMovieEvents();
    }
    
    private async Task FetchNewMovieEvents()
    {
        FindMovieEventsForMonthRequest request = new()
        {
            Year = ViewModel.NavigatorViewModel.SelectedYear,
            Month = ViewModel.NavigatorViewModel.SelectedMonth
        };
        
        ApiResult<IReadOnlyList<MovieEvent>> movieEvents = 
            await MovieEventsApiClient.FindMovieEventsForMonthAsync(request);

        if (movieEvents.IsFailure)
        {
            // TODO also add proper handling such that if connection to the api fails there isn't a huge stack trace in the browser
            // TODO this must show an error using an overlay
            return;
        }
        
        ViewModel.UpdateMovieEvents(movieEvents.Value!);
    }

    private async Task FetchMovies()
    {
        SearchMovieCatalogRequest request = new()
        {
            // TODO store this value somewhere else
            UserRole = "Manager"
        };
        
        ApiResult<MovieCollection> result = await MovieCatalogApiClient.SearchMovieCatalogAsync(request);

        if (result.IsFailure)
        {
            ViewModel.SchedulerOverlayViewModel.ErrorMessage = "Could not fetch available movies, contact your administrator if the issue persists.";
        }
        else
        {
            ViewModel.SchedulerOverlayViewModel.UpdateMovies(result.Value!);
        }
    }

    private void OnMovieEventClickedAsync(DateOnly date)
    {
        ViewModel.SchedulerOverlayViewModel.Form.SelectedDate = date;
        SetOverLayVisibility(true);
    }

    private void SetOverLayVisibility(bool visibility)
    {
        ViewModel.SchedulerOverlayViewModel.IsOpen = visibility;
        StateHasChanged();
    }
    
    private void CloseSchedulingOverlay()
    {
        SetOverLayVisibility(false);
    }

    private Task SaveMovieEventAsync(MovieEventFormViewModel formViewModel)
    {
        return Task.CompletedTask;
    }
}