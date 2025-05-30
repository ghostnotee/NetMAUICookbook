using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using c5_AuthenticationService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebAPI API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Name = "Bearer",
        Scheme = "bearer",
        BearerFormat = "Bearer",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultSignInScheme = IdentityConstants.BearerScheme;
        options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
        options.DefaultScheme = IdentityConstants.BearerScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddBearerToken(IdentityConstants.BearerScheme);

builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(@"Data Source=mydatabase.db"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
    .AddPolicy("UserPolicy", policy => policy.RequireRole("User"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapIdentityApi<User>();

app.MapGet("/users", async (UserManager<User> userManager) =>
{
    var users = await userManager.Users
        .Select(u => new
        {
            u.Email,
            u.BirthDate
        })
        .ToListAsync();

    return Results.Ok(users);
}).RequireAuthorization();

app.MapDelete("/users/delete/{email}", async (
    string email,
    UserManager<User> userManager,
    HttpContext httpContext) =>
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null) return Results.NotFound();
    var result = await userManager.DeleteAsync(user);
    if (result.Succeeded) return Results.Ok();
    return Results.Problem("Failed to delete the user.");
}).RequireAuthorization("AdminPolicy");

app.MapGet("/users/candelete", async (
    HttpContext httpContext,
    IAuthorizationService authorizationService) =>
{
    var authResult = await authorizationService.AuthorizeAsync(httpContext.User, "AdminPolicy");
    return Results.Ok(authResult.Succeeded);
});

app.MapGet("/me", async (ClaimsPrincipal principal, UserManager<User> userManager) =>
{
    var userEmail = principal.Claims.First(c => c.Type == ClaimTypes.Email).Value;
    var currentUser = await userManager.FindByEmailAsync(userEmail);
    return Results.Ok(new
    {
        currentUser.Email,
        currentUser.BirthDate
    });
}).RequireAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.EnsureCreated();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await userManager.CreateUserWithRoleAsync(
        roleManager,
        "admin@cookbook.com",
        "123Password123!",
        new DateOnly(1991, 4, 20),
        "Admin");

    for (var i = 1; i < 10; i++)
        await userManager.CreateUserWithRoleAsync(
            roleManager,
            $"user{i}@cookbook.com",
            "123Password123!",
            new DateOnly(2000, 1, i), "User");
}

app.MapGet("/mauth/google", (HttpContext httpContext) =>
{
    var props = new AuthenticationProperties { RedirectUri = "mauth/google/callback" };
    return Results.Challenge(props, new List<string> { GoogleDefaults.AuthenticationScheme });
});

app.MapGet("/mauth/google/callback", async (
    HttpContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) =>
{
    var authResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    if (!authResult.Succeeded) return Results.Redirect("myapp://");
    var email = authResult.Principal.FindFirstValue(ClaimTypes.Email);
    await userManager.CreateUserWithRoleAsync(roleManager, email, null, new DateOnly(2000, 1, 1), "User");

    using var responseBody = new MemoryStream();
    context.Response.Body = responseBody;
    await context.SignInAsync(IdentityConstants.BearerScheme, new ClaimsPrincipal(authResult.Principal.Identity));
    context.Response.Body.Seek(0, SeekOrigin.Begin);
    var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
    var tokenData = JsonSerializer.Deserialize<JsonNode>(responseText);

    var token = tokenData["accessToken"].GetValue<string>();
    var refreshToken = tokenData["refreshToken"].GetValue<string>();
    var expiresIn = tokenData["expiresIn"].GetValue<int>();

    var redirectUrl = $"myapp://?access_token={token}&refresh_token={refreshToken}&expires_in={expiresIn}";
    return Results.Redirect(redirectUrl);
});

app.Run();