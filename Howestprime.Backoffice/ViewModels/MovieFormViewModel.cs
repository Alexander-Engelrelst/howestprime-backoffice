namespace Howestprime.Backoffice.ViewModels;

public class MovieFormViewModel
{
    public string ErrorMessage { get; set; } = string.Empty;
    public bool? SuccessFullySaved { get; set; }
    
    public string CriticalErrorMessage { get; set; } = string.Empty;
    public MovieFormDataViewModel FormDataViewModel { get; set; } = new();
}