namespace FUExchange.ModelViews.UserModelViews
{
    public class UserResponseModel
    {
        public string? Id { get; set; }
        public required string Email { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();

    }
}
