using Howestprime.Backoffice.ViewModels;
using Howestprime.Backoffice.ViewModels.Planning;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.Extensions;

internal static class MovieEventExtensions
{
   internal static MovieEventViewModel ToViewModel(this MovieEvent movieEvent)
   {
      return new MovieEventViewModel
      {
         ShowTime = movieEvent.Showtime.DateTime, // for simplicity, we will assume everything gets done in UTC
         Room = new RoomViewModel
         {
            RoomName = movieEvent.Room.Name,
            RoomId = movieEvent.Room.Id
         },
         MovieName = movieEvent.Movie.Title,
      };
   }
}