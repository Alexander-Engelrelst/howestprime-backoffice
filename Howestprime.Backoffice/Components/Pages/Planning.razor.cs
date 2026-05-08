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

        await FetchMovieEvents();
    }
    private async Task OnNavigateMonthAsync(int direction)
    {
        ViewModel.NavigatorViewModel.Navigate(direction);
        await FetchMovieEvents();
    }
    
    private async Task FetchMovieEvents()
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

    private async Task OnMovieEventClicked(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.UtcNow)) return;
        
        ViewModel.SchedulerOverlayViewModel.Form.SelectedDate = date;
        SetOverLayVisibility(true);
        
        if (ViewModel.SchedulerOverlayViewModel.MustRefreshMovies)
        {
            ViewModel.SchedulerOverlayViewModel.IsLoading = true;
            await FetchMovies();
            ViewModel.SchedulerOverlayViewModel.IsLoading = false;
        }
    }

    private void SetOverLayVisibility(bool visibility)
    {
        ViewModel.SchedulerOverlayViewModel.PrepareForNewEntry();
        ViewModel.SchedulerOverlayViewModel.IsOpen = visibility;
    }
    
    private void CloseSchedulingOverlay()
    {
        SetOverLayVisibility(false);
    }

    private async Task SaveMovieEventAsync(MovieEventFormViewModel formViewModel)
    {
        ScheduleMovieEventRequest request = new()
        {
            // if any of these are null the formViewModel will catch this
            MovieId = (Guid)formViewModel.MovieId!,
            RoomId = (Guid)formViewModel.RoomId!,
            Showtime = formViewModel.EventDateTime!.Value
        };
        
        ApiResult<Created> result = await MovieEventsApiClient.ScheduleMovieEventAsync(request);

        if (result.IsFailure)
        {
            ViewModel.SchedulerOverlayViewModel.ErrorMessage
                = result.Error?.Detail ?? "Something went wrong";
        }
        else
        {
            // there is no route to get a single movieEvent nor does the route return the newly created event to just add it
           await FetchMovieEvents();
           SetOverLayVisibility(false);
        }
        
    }
}