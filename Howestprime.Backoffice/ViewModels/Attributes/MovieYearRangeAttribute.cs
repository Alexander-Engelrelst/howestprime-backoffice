using System.ComponentModel.DataAnnotations;

namespace Howestprime.Backoffice.ViewModels.Attributes;

/// <summary>
/// Attribute to validate that a year falls between 1888 (the release year of the first movie ever made) 
/// and a dynamic future year based on the current date.
/// </summary>
/// / <remarks>
/// <para>
/// This attribute supports custom error messages via the <see cref="ValidationAttribute.ErrorMessage"/> property.
/// Use "{0}" in your custom message to automatically include the property's display name.
/// </para>
/// <para>
/// <b>Default message:</b> "{0} must be between 1888 and [MaxYear]."
/// </para>
/// </remarks>
public class MovieYearRangeAttribute : ValidationAttribute
{
    // release year of the first ever movie
    private const int MinYear = 1888;
    private readonly int _maxYearsAhead;
    
    public MovieYearRangeAttribute(int maxYearsAhead)
    {
        _maxYearsAhead = maxYearsAhead;
    }
    
    /// <summary>
    /// Validates the specified value with respect to the current validation context.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>A <see cref="ValidationResult"/> if the year is invalid; otherwise, <see cref="ValidationResult.Success"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the attribute is applied to a non-integer property.</exception>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        if (value is not int year)
            throw new InvalidOperationException(
                $"{validationContext.DisplayName} is not a valid year"
            );

        int currentYear = DateTime.Now.Year;
        if (year < MinYear || year > currentYear + _maxYearsAhead)
        {
            string errorMessage = ErrorMessage ?? $"{validationContext.DisplayName} must be between {MinYear} and {currentYear + _maxYearsAhead}.";
            return new ValidationResult(errorMessage, new[] { validationContext.MemberName! });        }

        return ValidationResult.Success;
    }
}
