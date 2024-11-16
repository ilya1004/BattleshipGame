using Microsoft.AspNetCore.Identity;

namespace Entities;

public class UserClient : IdentityUser<int>
{
    public string? AvatarPath { get; set; } = string.Empty;
}
