using Howestprime.Backoffice.ViewModels.Register;
using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;
using Microsoft.AspNetCore.Components;

namespace Howestprime.Backoffice.Components.Pages;

public partial class Register : ComponentBase
{
    
    [Inject]
    private IMovieCatalogApiClient MovieCatalogApiClient { get; set; } = null!;
    
    private MovieViewModel FormViewModel { get; set; } = new();
    private RegisterViewModel ViewModel { get; set; }= new();
    private string _tempGenre = "";
    private string _tempActor = "";
    
    private void TryAddGenre() 
    { 
        if (!string.IsNullOrWhiteSpace(_tempGenre))
        {
            FormViewModel.Genres.Add(_tempGenre.Trim()); 
            _tempGenre = ""; 
        } 
    }

    private void TryAddActor() 
    { 
        if (!string.IsNullOrWhiteSpace(_tempActor)) 
        { 
            FormViewModel.Actors.Add(_tempActor.Trim()); 
            _tempActor = ""; 
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