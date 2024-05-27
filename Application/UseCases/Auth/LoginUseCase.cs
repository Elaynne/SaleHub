
using AutoMapper;
using Domain.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Domain.Enums;

namespace Application.UseCases.Auth
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginUseCase(IMapper mapper,
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            SetupMock();
        }

        public async Task<string?> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request);

                return (user is not null && user.Value.Role is not null) ? 
                    GenerateJwtToken(request.Username, user.Value.Role, user.Value.Id) 
                    : null;
        }

        private async Task<(string Role, string Id)?> GetUser(LoginInput login)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var currentUser = users
                .Where(x => x.UserName == login.Username && x.Password == login.Password)
                .FirstOrDefault();
            return currentUser != null ? (currentUser.Role.ToString(), currentUser.Id.ToString()) : null;
        }

        private string GenerateJwtToken(string username, string userRole, string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",userId),
                new Claim(ClaimTypes.Role, userRole)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetupMock()
        {
            var mock = GetUsersMock();
            foreach (var user in mock)
            {
                _userRepository.AddUserAsync(user);
            }
        }
        private List<Domain.Models.User> GetUsersMock()
        {
            return new List<Domain.Models.User>()
        {
            new Domain.Models.User() {
                Id = new Guid("a17ad52f-8720-492e-af21-b08514ea3e48"),
                UserName = "user-admin",
                Email = "admin@gmail.com",
                Password = "11111111",
                Role = UserRole.Admin,
                Active = true
            },
            new Domain.Models.User() {
                Id = new Guid("a21025ba-8fe1-485d-832d-cc050778e17b"),
                UserName = "user-seller",
                Email = "seller@gmail.com",
                Password = "22222222",
                Role = UserRole.Seller,
                Active = true
            },
            new Domain.Models.User() {
                Id = new Guid("de4711ce-3fb2-4050-a483-936b364fd60f"),
                UserName = "user-client",
                Email = "client@gmail.com",
                Password = "33333333",
                Role = UserRole.Client,
                Active = true
            }
        };
        }
    }
}
