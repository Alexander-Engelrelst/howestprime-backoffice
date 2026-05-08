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
    
    private MovieViewModel FormViewModel { get; set; } = new();
    private RegisterViewModel ViewModel { get; set; }= new();
    
    private EditContext? _editContext;

    private bool SubmissionPending { get; set; } = false;
    protected override void OnInitialized()
    {
        _editContext = new EditContext(FormViewModel);
    }
    
    private async Task HandleManualSubmit()
    {
        SubmissionPending = true;
        bool isValid = _editContext?.Validate() ?? false;

        if (isValid)
        {
            await HandleValidSubmit();
            SubmissionPending = false;
        }
        else
        {
            _editContext?.NotifyValidationStateChanged();
            ViewModel.SuccessFullyRegistered = false;
            ViewModel.ErrorMessage = "Please fix the errors before submitting.";
            SubmissionPending = false;
        }
    }
    private async Task HandleValidSubmit()
    {
        RegisterMovieRequest request = new()
        {
            Title = FormViewModel.Title,
            Description = FormViewModel.Description,
            ReleaseYear = FormViewModel.ReleaseYear,
            PosterUrl = FormViewModel.PosterUrl,
            Duration = FormViewModel.Duration,
            AgeRating = FormViewModel.AgeRating,
            Genres = FormViewModel.Genres.ToList(),
            Actors = FormViewModel.Actors.ToList()
        };
        
        ApiResult<Created> response = await MovieCatalogApiClient.RegisterMovieAsync(request);

        if (response.IsSuccess)
        {
            ViewModel.SuccessFullyRegistered = true;
            ViewModel.ErrorMessage = string.Empty;
            FormViewModel = new();
        }
        else
        {
            ViewModel.SuccessFullyRegistered = false;
            ViewModel.ErrorMessage = response.Error?.Detail ?? "An unexpected error occurred.";
        }
        
    }
}