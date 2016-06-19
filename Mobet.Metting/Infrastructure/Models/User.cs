using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobet.Metting.Infrastructure.Models
{
    public class User
    {
        public string NickName { get; set; }
        public int? Age { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
    }
}
