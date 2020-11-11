using CoinApp.Domain.Extensions;
using CoinApp.Domain.Users;
using CoinApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Administration.DomainServices
{
    public class Authentication : IAuthentication
    {
        private readonly IRepository<User> _iRepository;
        public Authentication(IRepository<User> iRepository)
        {
            this._iRepository = iRepository;
        }

        public User Authorize(string username, string password)
        {
            try
            {
                var user = _iRepository.GetSingleByCriteria(x => x.Username.ToLower() == username.ToLower());

                // check if username exists
                if (user == null)
                    return null;

                byte[] passwordHash, passwordSalt;
                HashHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                // check if password is correct
                if (!HashHelper.VerifyPasswordHash(password, user.Password, user.Salt))
                    return null;

                // authentication successful
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}