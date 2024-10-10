using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.UserModelViews
{
    public class CreateUserModel
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } // Admin assigns roles
        public required string FullName { get; set; } = string.Empty;
    }
}
