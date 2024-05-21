
using System.ComponentModel;

namespace Domain.Enums
{
    public enum Roles
    {
        [Description("Admin")]
        Admin = 0,
        [Description("Seller")]
        Seller = 1,
        [Description("Client")]
        Client = 2
    }
}
