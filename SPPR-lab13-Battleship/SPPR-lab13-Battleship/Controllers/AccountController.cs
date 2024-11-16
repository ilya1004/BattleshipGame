using Database;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using System.Net;

namespace Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager; 
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
    {
        _userManager = userManager; 
        _signInManager = signInManager;
        _context = context;
    }

    [HttpPost]
    public async Task<IResult> Register(RegisterUserModel model)
    {
        var user = new User {
            UserName = model.UserName, 
            Email = model.Email
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Results.Created();
        }

        return Results.BadRequest(result.Errors);
    }


    [HttpGet]
    //[Authorize]
    public async Task<IResult> GetCurrentInfo()
    {
        var cookies = Request.Cookies;

        if (cookies.Count < 2)
        {
            return Results.Unauthorized();
        }

        var handler = new HttpClientHandler()
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        foreach (var cookie in cookies)
        {
            Console.WriteLine($"Cookie: {cookie.Key} = {cookie.Value}");
            handler.CookieContainer.Add(new Uri("https://localhost:7147"), new Cookie(cookie.Key, cookie.Value));
        }

        using var client = new HttpClient(handler);
        var res = await client.GetAsync("https://localhost:7147/manage/info");
        var content = await res.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<UserInfo>(content);

        if (data == null)
        {
            return Results.BadRequest();
        }

        var userData = await _context.Users.FirstOrDefaultAsync(user => user.Email == data.Email);

        return Results.Json(userData);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("https://localhost:7147/");
    }
}
