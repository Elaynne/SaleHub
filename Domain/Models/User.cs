using Domain.Enums;

namespace Domain.Models;
public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; } // Admin, Seller, Client
    public bool Active { get; set; }
}