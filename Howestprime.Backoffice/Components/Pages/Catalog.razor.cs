using Howestprime.Backoffice.Services;
using Howestprime.Backoffice.ViewModels.Catalog;
using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;
using Microsoft.AspNetCore.Components;

namespace Howestprime.Backoffice.Components.Pages;

public partial class Catalog : ComponentBase
{
    private const int MOVIE_EAGER_LOAD_THRESHOLD = 12;
    
    [Inject]
    private IMovieCatalogApiClient MovieCatalogApiClient { get; set; } = null!;
    
    private CatalogViewModel ViewModel { get; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        SearchMovieCatalogRequest request = new()
        {
            UserRole = "Manager"
        };
        
        ApiResult<MovieCollection> result = await MovieCatalogApiClient.SearchMovieCatalogAsync(request);

        if (result.IsFailure)
        {
            ViewModel.ErrorMessage 
                = "An error occured while getting the movies catalog. Please try again later or contact an administrator if the issue persists.";
        }
        else
        {
            ViewModel.UpdateMovies(result.Value!);
        }
        
        ViewModel.IsLoading = false;
    }
}