namespace Howestprime.Movies.ApiClient.Requests;

public sealed class FindMovieByIdRequest
{
    public string UserRole { get; set; } = "Manager";
    public required Guid MovieId { get; set; } 
}