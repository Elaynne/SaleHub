using Domain.Enums;

namespace Domain.Models;
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; } // Admin, Seller, Client
}