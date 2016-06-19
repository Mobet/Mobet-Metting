using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobet.Metting.Infrastructure.Models;

namespace Mobet.Metting.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public User Login(string account, string password)
        {
            if (account == "392327013" && password == "mobet")
            {
                return new User
                {
                    NickName = "穆轻寒",
                    Age = 25,
                    Avatar = "https://s1.mi-img.com/mfsv2/avatar/fdsc3/p01ipyFBOr98/GOF2ZyTtU5ejqs_320.jpg",
                };
            }
            return null;
        }
    }
}
