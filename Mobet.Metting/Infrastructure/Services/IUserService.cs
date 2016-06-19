using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobet.Metting.Infrastructure.Models;

namespace Mobet.Metting.Infrastructure.Services
{
    public interface IUserService
    {
        User Login(string account, string password);
    }
}
