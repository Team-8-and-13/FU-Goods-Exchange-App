using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.UserModelViews
{
    public class UpdateUserModel
    {
        public required string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email sai định dạng")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com và không chứa dấu tiếng Việt")]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]*$", ErrorMessage = "Họ và tên không được chứa số hoặc ký tự đặc biệt")]
        public required string FullName { get; set; }
    }
}
