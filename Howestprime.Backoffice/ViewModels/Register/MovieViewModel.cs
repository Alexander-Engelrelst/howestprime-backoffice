using System.ComponentModel.DataAnnotations;

namespace Howestprime.Backoffice.ViewModels.Register;

public class MovieViewModel
{
    [Required(ErrorMessage = "Please fill in the title")]
    [MaxLength(200, ErrorMessage = "The title cannot be longer than 200 characters")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Please fill in the description")]
    [MaxLength(1000, ErrorMessage = "The description cannot be longer than 1000 characters")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Please fill in the release year")]
    [Display(Name = "Release Year")]
    [MovieYearRange(2)]
    public int ReleaseYear { get; set; }
    
    [Required(ErrorMessage = "Please fill in the release date")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid duration in minutes (must be at least 1).")]
    public int Duration { get; set; }
    
    [MinLength(1, ErrorMessage = "Please add at least one genre")]
    public HashSet<string> Genres { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    [MinLength(1, ErrorMessage = "Please add at least one actor")]
    public HashSet<string> Actors { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    [Required]
    [Range(0, 18)]
    [Display(Name = "Age Rating")]
    public int AgeRating { get; set; }

    [Required]
    [Url]
    [MaxLength(500)]
    [Display(Name = "Poster URL")]
    public string PosterUrl { get; set; } = string.Empty;
}
