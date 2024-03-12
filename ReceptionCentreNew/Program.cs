using AisReception.Data.Context.App;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Hubs;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Models;
using ReceptionCentreNew.Models.Account;
using ReceptionCentreNew.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ReceptionCentreContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.Tokens.EmailConfirmationTokenProvider = "Email";
    options.Tokens.ChangeEmailTokenProvider = "Email";
})
    .AddEntityFrameworkStores<ReceptionCentreContext>()
    .AddSignInManager<ApplicationSignInManager>()
    .AddUserManager<ApplicationUserManager>()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepository, EFRepository>();

builder.Services.AddScoped<IUserPasswordStore<ApplicationUser>, CustomMembershipProvider>();
builder.Services.AddScoped<IUserRoleStore<ApplicationUser>, CustomRoleProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
    {
        option.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
    });

builder.Services.AddSignalR();

builder.Services.AddScoped<Hub, NotificationHub>();

builder.Services.AddRazorPages();
  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/NotificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();