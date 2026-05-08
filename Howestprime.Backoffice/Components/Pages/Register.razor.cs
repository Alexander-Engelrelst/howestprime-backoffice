using Howestprime.Backoffice.ViewModels;
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
    
    private MovieFormViewModel ViewModel { get; set; }= new();
    
    private EditContext? EditContext { get; set; }

    private bool SubmissionPending { get; set; } = false;
    protected override void OnInitialized()
    {
        EditContext = new EditContext(ViewModel.FormDataViewModel);
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
            Title = ViewModel.FormDataViewModel.Title,
            Description = ViewModel.FormDataViewModel.Description,
            ReleaseYear = ViewModel.FormDataViewModel.ReleaseYear,
            PosterUrl = ViewModel.FormDataViewModel.PosterUrl,
            Duration = ViewModel.FormDataViewModel.Duration,
            AgeRating = ViewModel.FormDataViewModel.AgeRating,
            Genres = ViewModel.FormDataViewModel.Genres.ToList(),
            Actors = ViewModel.FormDataViewModel.Actors.ToList()
        };
        
        ApiResult<Created> response = await MovieCatalogApiClient.RegisterMovieAsync(request);

        if (response.IsSuccess)
        {
            ViewModel.SuccessFullyRegistered = true;
            ViewModel.ErrorMessage = string.Empty;
            ViewModel.FormDataViewModel = new();
        }
        else
        {
            ViewModel.SuccessFullyRegistered = false;
            ViewModel.ErrorMessage = response.Error?.Detail ?? "An unexpected error occurred.";
        }
        
    }
}