
using AutoMapper;
using Domain.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

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
        }

        public async Task<string?> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            var userRole = await GetUserRole(request);

            return !string.IsNullOrWhiteSpace(userRole) ? GenerateJwtToken(request.Username, userRole) : null;
        }

        private async Task<string> GetUserRole(LoginInput login)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var currentUser = users
                .Where(x => x.UserName == login.Username && x.Password == login.Password)
                .FirstOrDefault();
            return currentUser != null ? currentUser.Role.ToString() : null;
        }

        private string GenerateJwtToken(string username, string userRole)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
    }
}
