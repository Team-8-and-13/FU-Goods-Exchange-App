using Microsoft.AspNetCore.Identity;
using FUExchange.Core.Utils;

namespace FUExchange.Contract.Repositories.Entity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public const string Admin = "Admin";
        public const string UserPolicy = "User";
        public const string Moderator = "Moderator";

        public ApplicationRole()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
