namespace Howestprime.Backoffice.ViewModels;

public class NavigatorViewModel
{
    private const int YearLimit = 5;
    public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public int SelectedYear => SelectedDate.Year;
    public int SelectedMonth => SelectedDate.Month;
    
    public bool CanNavigateForward => SelectedDate.AddMonths(1) <= MaxDate;
    public bool CanNavigateBack => SelectedDate.AddMonths(-1) >= MinDate;
    private DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.UtcNow);
    private DateOnly MaxDate => CurrentDate.AddMonths(YearLimit * 12);
    private DateOnly MinDate => CurrentDate.AddMonths(-YearLimit * 12);
    
    public void Navigate(int direction)
    {
        SelectedDate = SelectedDate.AddMonths(direction);
    }
}