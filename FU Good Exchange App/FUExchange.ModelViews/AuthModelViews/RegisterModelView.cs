namespace FUExchange.ModelViews.AuthModelViews
{
    public class RegisterModelView
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
