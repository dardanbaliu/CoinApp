using AutoMapper;
using CoinApp.API.Helper;
using CoinApp.Domain.Extensions;
using CoinApp.Domain.Users;
using CoinApp.Infrastructure.Repositories;
using CoinApp.SharedModel.Users;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.API.Users.Commands
{
    public class RegisterHandler : IRequestHandler<Register, UserDto>
    {
        private readonly IRepository<User> _iUserRepository;

        public RegisterHandler(IRepository<User> iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }
        public async Task<UserDto> Handle(Register request, CancellationToken cancellationToken)
        {
            var isEmailValid = ValidationHelper.IsValidEmail(request.Email);
            if (!isEmailValid)
                throw new Exception("Invalid Email!");
            var isUsernameAvailable = _iUserRepository.CountByCriteria(t => t.Username == request.Username) == 0 ? true : false;
            if (!isUsernameAvailable)
                throw new Exception("This username is taken, try another!");

            byte[] newPasswordHash, newPasswordSalt;
            HashHelper.CreatePasswordHash(request.Password, out newPasswordHash, out newPasswordSalt);
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IsActive = true,
                Username = request.Username,
                Password = newPasswordHash,
                Salt = newPasswordSalt,
            };
            await _iUserRepository.AddAsync(newUser);
            var mapperConfig = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<User, UserDto>().ForMember(t => t.AccessToken, a => a.Ignore()).ReverseMap();
            });
            mapperConfig.AssertConfigurationIsValid();
            var mapper = mapperConfig.CreateMapper();
            var userData = mapper.Map<UserDto>(newUser);
            return Task.FromResult(userData).Result;
        }
    }
}
