using Domain.Enums;

namespace Application.UseCases.Users.RetrieveUserById
{
    public class RetrieveUserByIdOutput
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public bool Active { get; set; }
    }
}
