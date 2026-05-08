using Howestprime.Backoffice.ViewModels.Register;
using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Howestprime.Backoffice.Components.Pages;

public partial class Register : ComponentBase
{
    
    [Inject]
    private IMovieCatalogApiClient MovieCatalogApiClient { get; set; } = null!;
    
    private RegisterViewModel ViewModel { get; set; }= new();
    
    private EditContext? EditContext { get; set; }

    private bool SubmissionPending { get; set; } = false;
    protected override void OnInitialized()
    {
        EditContext = new EditContext(ViewModel.FormViewModel);
    }
    
    private async Task HandleManualSubmit()
    {
        SubmissionPending = true;
        bool isValid = EditContext?.Validate() ?? false;

        if (isValid)
        {
            await HandleValidSubmit();
            SubmissionPending = false;
        }
        else
        {
            EditContext?.NotifyValidationStateChanged();
            ViewModel.SuccessFullyRegistered = false;
            ViewModel.ErrorMessage = "Please fix the errors before submitting.";
            SubmissionPending = false;
        }
    }
    private async Task HandleValidSubmit()
    {
        RegisterMovieRequest request = new()
        {
            Title = ViewModel.FormViewModel.Title,
            Description = ViewModel.FormViewModel.Description,
            ReleaseYear = ViewModel.FormViewModel.ReleaseYear,
            PosterUrl = ViewModel.FormViewModel.PosterUrl,
            Duration = ViewModel.FormViewModel.Duration,
            AgeRating = ViewModel.FormViewModel.AgeRating,
            Genres = ViewModel.FormViewModel.Genres.ToList(),
            Actors = ViewModel.FormViewModel.Actors.ToList()
        };
        
        ApiResult<Created> response = await MovieCatalogApiClient.RegisterMovieAsync(request);

        if (response.IsSuccess)
        {
            ViewModel.SuccessFullyRegistered = true;
            ViewModel.ErrorMessage = string.Empty;
            ViewModel.FormViewModel = new();
        }
        else
        {
            ViewModel.SuccessFullyRegistered = false;
            ViewModel.ErrorMessage = response.Error?.Detail ?? "An unexpected error occurred.";
        }
        
    }
}