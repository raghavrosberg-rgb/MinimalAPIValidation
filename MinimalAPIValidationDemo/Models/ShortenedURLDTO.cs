using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MinimalAPIValidationDemo.Models
{
    public class ShortenedURLDTO : IValidatableObject
    {
        [Required]
        [Display(Name = "URL")]
        public required string Url { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // To check if a string contains or is a URL with standard protocols (http, https, or ftp)
            string pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";
            if (!Regex.IsMatch(Url, pattern, RegexOptions.IgnoreCase))
            {
                yield return new ValidationResult(
                    "Invalid URL",
                    new[] { nameof(Url) }
                );
            }
        }
    }
}
