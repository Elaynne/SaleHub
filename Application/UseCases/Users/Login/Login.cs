using Domain.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Application.UseCases.Users.Login
{
    public class Login : ILogin
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        private readonly IBookRepository _bookRepository;
        private readonly IMemoryCache _memoryCache;

        public Login(
            IUserRepository userRepository,
            IConfiguration configuration,
            IBookRepository bookRepository,
            IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _configuration = configuration;

            //mock purpose
            _bookRepository = bookRepository;
            _memoryCache = memoryCache;
            _memoryCache.TryGetValue("mock_loaded", out bool mockLoaded);
            if (!mockLoaded)
                SetupMock();
        }

        public async Task<string?> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request);

            return user is not null && user.Value.Role is not null ?
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
            var userMock = GetUsersMock();
            foreach (var user in userMock)
            {
                _userRepository.AddUserAsync(user);
            }

            var bookMock = GetBooksMock();
            foreach (var book in bookMock)
            {
                _bookRepository.AddBookAsync(book);
            }

            _memoryCache.Set("mock_loaded", true);
        }
        private List<User> GetUsersMock()
        {
            return new List<User>()
            {
                new User() {
                    Id = new Guid("a17ad52f-8720-492e-af21-b08514ea3e48"),
                    UserName = "user-admin",
                    Email = "admin@gmail.com",
                    Password = "11111111",
                    Role = UserRole.Admin,
                    Active = true
                },
                new User() {
                    Id = new Guid("a21025ba-8fe1-485d-832d-cc050778e17b"),
                    UserName = "user-seller",
                    Email = "seller@gmail.com",
                    Password = "22222222",
                    Role = UserRole.Seller,
                    Active = true
                },
                new User() {
                    Id = new Guid("de4711ce-3fb2-4050-a483-936b364fd60f"),
                    UserName = "user-client",
                    Email = "client@gmail.com",
                    Password = "33333333",
                    Role = UserRole.Client,
                    Active = true
                }
            };
        }

        private List<Book> GetBooksMock()
        {
            return new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0547928227",
                    Description = "A fantasy novel and children's book by J.R.R. Tolkien, follows the quest of home-loving Bilbo Baggins.",
                    Stock = 120,
                    Price = 15.99m,
                    CostPrice = 8.00m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Lord of the Rings: The Fellowship of the Ring",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0618574940",
                    Description = "The first volume in the epic fantasy series The Lord of the Rings.",
                    Stock = 100,
                    Price = 19.99m,
                    CostPrice = 10.00m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Lord of the Rings: The Two Towers",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0618574957",
                    Description = "The second volume in the epic fantasy series The Lord of the Rings.",
                    Stock = 95,
                    Price = 19.99m,
                    CostPrice = 10.00m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Lord of the Rings: The Return of the King",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0618574971",
                    Description = "The third volume in the epic fantasy series The Lord of the Rings.",
                    Stock = 90,
                    Price = 19.99m,
                    CostPrice = 10.00m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Silmarillion",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0618391110",
                    Description = "A collection of mythopoeic stories by J.R.R. Tolkien.",
                    Stock = 80,
                    Price = 17.99m,
                    CostPrice = 9.00m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Unfinished Tales of Númenor and Middle-earth",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0618136513",
                    Description = "A collection of narratives ranging in time from the Elder Days of Middle-earth to the end of the War of the Ring.",
                    Stock = 70,
                    Price = 18.99m,
                    CostPrice = 9.50m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Children of Húrin",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0547086057",
                    Description = "An epic fantasy novel which forms part of the story of the First Age of Middle-earth.",
                    Stock = 60,
                    Price = 16.99m,
                    CostPrice = 8.50m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Beren and Lúthien",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-1328791825",
                    Description = "A fantasy story edited by Christopher Tolkien from the works of his father, J.R.R. Tolkien.",
                    Stock = 65,
                    Price = 17.99m,
                    CostPrice = 8.75m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Fall of Gondolin",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-1328613042",
                    Description = "An epic fantasy novel by J.R.R. Tolkien, edited by his son Christopher Tolkien.",
                    Stock = 60,
                    Price = 17.99m,
                    CostPrice = 8.75m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The History of Middle-earth: The Book of Lost Tales",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0261102224",
                    Description = "The first two volumes of The History of Middle-earth, a series of books edited and compiled by Christopher Tolkien.",
                    Stock = 50,
                    Price = 18.99m,
                    CostPrice = 9.25m
                }
            };
        }
    }
}
