using Howestprime.Backoffice.ViewModels;
using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Howestprime.Backoffice.Components.Pages;

public partial class EditMovie : ComponentBase
{
    private bool _submissionPending;

    [Parameter]
    public Guid MovieId { get; set; }
    
    [Inject]
    private IMovieCatalogApiClient MovieCatalogApiClient { get; set; } = null!;
    
    private MovieFormViewModel ViewModel { get; set; } = new();
    private EditContext? EditContext { get; set; }
    
    protected override async Task OnInitializedAsync()
    {        
        FindMovieByIdRequest request = new FindMovieByIdRequest
        {
            MovieId = MovieId,
            UserRole = "Manager"
        };
        
        ApiResult<Movie> result = await MovieCatalogApiClient.FindMovieByIdAsync(request);

        if (result.IsFailure || result.Value is null)
        {
            ViewModel.CriticalErrorMessage 
                = "An error occurred while fetching the movie details. Please try again later or contact an administrator if the issue persists.";
        }
        else
        {
            ViewModel.FormDataViewModel.PosterUrl = result.Value.PosterUrl;
            ViewModel.FormDataViewModel.Title = result.Value.Title;
            ViewModel.FormDataViewModel.Description = result.Value.Description;
            ViewModel.FormDataViewModel.ReleaseYear = result.Value.ReleaseYear;
            ViewModel.FormDataViewModel.Duration = result.Value.Duration;
            ViewModel.FormDataViewModel.AgeRating = result.Value.AgeRating;
            ViewModel.FormDataViewModel.Genres = result.Value.Genres.ToHashSet();
            ViewModel.FormDataViewModel.Actors = result.Value.Actors.ToHashSet();
        }

        EditContext = new EditContext(ViewModel.FormDataViewModel);
    }

    private async Task HandleManualSubmit(Action[] clearInputFields)
    {
        if (_submissionPending) return;
        
        _submissionPending = true;
        bool isValid = EditContext?.Validate() ?? false;

        if (isValid)
        {
            await HandleValidSubmit(clearInputFields);
        }
        else
        {
            EditContext?.NotifyValidationStateChanged();
            ViewModel.SuccessFullySaved = false;
            ViewModel.ErrorMessage = "Please fix the errors before submitting.";
        }
        
        _submissionPending = false;
    }
    private async Task HandleValidSubmit(Action[] tempFieldClears)
    {
        UpdateMovieRequest request = new()
        {
            MovieId = MovieId,
            Title = ViewModel.FormDataViewModel.Title,
            Description = ViewModel.FormDataViewModel.Description,
            ReleaseYear = ViewModel.FormDataViewModel.ReleaseYear,
            PosterUrl = ViewModel.FormDataViewModel.PosterUrl,
            Duration = ViewModel.FormDataViewModel.Duration,
            AgeRating = ViewModel.FormDataViewModel.AgeRating,
            Genres = ViewModel.FormDataViewModel.Genres.ToList(),
            Actors = ViewModel.FormDataViewModel.Actors.ToList()
        };
        
        ApiResult<Created> response = await MovieCatalogApiClient.UpdateMovieAsync(request);

        if (response.IsSuccess)
        {
            ViewModel.SuccessFullySaved = true;
            ViewModel.ErrorMessage = string.Empty;
            
            foreach (Action clearField in tempFieldClears)
            {
                clearField();
            }
        }
        else
        {
            ViewModel.SuccessFullySaved = false;
            ViewModel.ErrorMessage = response.Error?.Detail ?? "An unexpected error occurred.";
        }
        
    }
    
}