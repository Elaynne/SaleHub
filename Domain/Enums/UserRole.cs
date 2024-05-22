
using System.ComponentModel;

namespace Domain.Enums
{
    public enum UserRole
    {
        [Description("Client")]
        Client = 0,
        [Description("Seller")]
        Seller = 1,
        [Description("Administrator")]
        Admin = 2
    }
}
