using Microsoft.AspNetCore.Identity;

namespace Entities;

internal class User : IdentityUser<int>
{
    public string? AvatarPath { get; set; } = string.Empty;
}
