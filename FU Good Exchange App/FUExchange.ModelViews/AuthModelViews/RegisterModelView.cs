using System.ComponentModel.DataAnnotations;

namespace FUExchange.ModelViews.AuthModelViews
{
    public class RegisterModelView
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email sai định dạng")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com và không chứa dấu tiếng Việt")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]*$", ErrorMessage = "Họ và tên không được chứa số hoặc ký tự đặc biệt")]
        public required string FullName { get; set; }
    }
}
