using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class User : IValidatableObject
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "The password must be between 8 and 16 characters long.")]
    public string Password { get; set; }

    public UserRole Role { get; set; } // Admin, Seller, Client

    public bool Active { get; set; }

    public Guid? SellerId { get; set; } // Nullable because it is required only for clients

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == UserRole.Client && SellerId == null)
        {
            yield return new ValidationResult("SellerId is required for clients.", new[] { nameof(SellerId) });
        }
    }
}