using System.ComponentModel.DataAnnotations;

namespace Howestprime.Backoffice.ViewModels.Planning;

public class MovieEventFormViewModel
{
    [Required(ErrorMessage = "Something went wrong (Date missing). Please try again or contact an administrator if the issue persists.")]
    public DateOnly? SelectedDate { get; set; }
    
    [Required(ErrorMessage = "Please select a room.")]
    public Guid? RoomId { get; set; } 
    
    [Required(ErrorMessage = "Please select a movie.")]
    public Guid? MovieId { get; set; }
    
    [Required]
    public TimeOnly? ShowTime { get; set; } // nullable to prevent rendering issues with the default value to show in the form

    public DateTime? EventDateTime
    {
        get
        {
            if (SelectedDate.HasValue && ShowTime.HasValue)
            {
                return SelectedDate.Value.ToDateTime(ShowTime.Value, DateTimeKind.Utc);
            }
            
            return null;
        }
    }
}