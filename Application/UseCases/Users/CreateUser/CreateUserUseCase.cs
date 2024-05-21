using AutoMapper;
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.CreateUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository; 
        private readonly IMapper _mapper;

        public CreateUserUseCase(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> Handle(CreateUserInput request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            return await _userRepository.AddUserAsync(user);
        }
    }

}