namespace Howestprime.Backoffice.ViewModels.Register;

public class RegisterViewModel
{
    public string ErrorMessage { get; set; } = string.Empty;
    public bool? SuccessFullyRegistered { get; set; } = null;
    
    public MovieViewModel FormViewModel { get; set; } = new();
}