using System.ComponentModel.DataAnnotations;

namespace MinimalAPIValidationDemo.Models
{
    public class OrderDTO :IValidatableObject
    {
        [Required]
        [Display(Name = "Customer Name")]
        public required string CustomerName { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Delivery Date")]
        [BusinessDay]
        public DateTime DeliveryDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Example rule: total must not exceed $10,000
            if (UnitPrice * Quantity > 10_000m)
            {
                yield return new ValidationResult(
                    "Order total cannot exceed $10,000.",
                    new[] { nameof(UnitPrice), nameof(Quantity) }
                );
            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class BusinessDayAttribute : ValidationAttribute
        {
            public BusinessDayAttribute()
                => ErrorMessage = "The {0} must fall on a business day (Monday–Friday).";

            public override bool IsValid(object? value)
            {
                // Let [Required] handle nulls if you need them.
                if (value is DateTime dt)
                {
                    return dt.DayOfWeek is not DayOfWeek.Saturday
                        && dt.DayOfWeek is not DayOfWeek.Sunday;
                }
                return true;
            }
        }

    }
}
