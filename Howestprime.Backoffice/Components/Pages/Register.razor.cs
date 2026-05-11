using Howestprime.Backoffice.ViewModels;
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
    private bool _submissionPending;

    [Inject]
    private IMovieCatalogApiClient MovieCatalogApiClient { get; set; } = null!;
    
    private MovieFormViewModel ViewModel { get; set; }= new();
    
    private EditContext? EditContext { get; set; }

    protected override void OnInitialized()
    {
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
            ViewModel.SuccessFullySaved = true;
            ViewModel.ErrorMessage = string.Empty;
            ViewModel.FormDataViewModel = new();
            
            foreach (var clearField in tempFieldClears)
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