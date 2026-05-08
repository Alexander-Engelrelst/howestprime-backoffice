using Howestprime.Backoffice.ViewModels.Register;

namespace Howestprime.Backoffice.ViewModels;

public class RegisterViewModel
{
    public string ErrorMessage { get; set; } = string.Empty;
    public bool? SuccessFullyRegistered { get; set; }
    
    public MovieViewModel FormViewModel { get; set; } = new();
}