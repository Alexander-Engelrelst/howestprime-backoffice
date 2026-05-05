namespace Howestprime.Backoffice.ViewModels;

public class NavigatorViewModel
{
    private DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    
    public string CurrentMonthFormatted => SelectedDate.Month.ToString("MMMM yyyy");
    
    public int SelectedYear => SelectedDate.Year;
    public int SelectedMonth => SelectedDate.Month;
    
    public void Navigate(int direction)
    {
        SelectedDate = SelectedDate.AddMonths(direction);
    }
}