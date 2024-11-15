using Microsoft.AspNetCore.Identity;

namespace Entities;

internal class UserClient : IdentityUser<int>
{
    public string? AvatarPath { get; set; } = string.Empty;
}
