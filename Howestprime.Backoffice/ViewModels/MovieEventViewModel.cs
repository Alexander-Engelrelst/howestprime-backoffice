namespace Howestprime.Backoffice.ViewModels;

public class MovieEventViewModel
{
    public required DateTime ShowTime { get;  set; }
    public required RoomViewModel Room { get;  set; }
    
    public required string MovieName { get;  set; }
}