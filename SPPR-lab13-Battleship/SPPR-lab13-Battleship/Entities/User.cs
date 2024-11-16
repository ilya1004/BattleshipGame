using Microsoft.AspNetCore.Identity;

namespace Entities;

public class User : IdentityUser<int>
{
    public string? AvatarPath { get; set; } = string.Empty;
}
