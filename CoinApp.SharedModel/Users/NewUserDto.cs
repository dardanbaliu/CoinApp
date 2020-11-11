using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.SharedModel.Users
{
    public class NewUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
