using System.ComponentModel.DataAnnotations;

namespace Howestprime.Backoffice.ViewModels;

public class MovieEventFormViewModel
{
    [Required(ErrorMessage = "Something went wrong (Date missing). Please try again or contact an administrator if the issue persists.")]
    public DateOnly? SelectedDate { get; set; }
    
    [Required(ErrorMessage = "Please select a room.")]
    public Guid? RoomId { get; set; } 
    
    [Required(ErrorMessage = "Please select a movie.")]
    public Guid? MovieId { get; set; }
    
    [Required]
    public TimeOnly ShowTime { get; set; }
    
    public DateTime? EventDateTime => SelectedDate?.ToDateTime(ShowTime) ?? null;
}