using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SPPR_lab13_Battleship.Client.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddCascadingAuthenticationState();

//builder.Services.AddHttpClient("client");

builder.Services.AddApiAuthorization();

var app = builder.Build();

await app.RunAsync();
