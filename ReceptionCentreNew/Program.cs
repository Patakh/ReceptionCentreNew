using AisReception.Data.Context.App;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using SmartBreadcrumbs.Extensions;
using Microsoft.EntityFrameworkCore; 
using ReceptionCentreNew.Areas.Identity.User;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Data.Context.Identity;
using ReceptionCentreNew.Hubs;  

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ReceptionCentreContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AuthenticationContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
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
    .AddEntityFrameworkStores<AuthenticationContext>();
 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews(); 
builder.Services.AddScoped<IRepository, EFRepository>();
builder.Services.AddScoped<IHubContext, NotificationHub>();
builder.Services.AddSignalR();
 
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

app.UseAuthorization();

app.MapHub<NotificationHub>("/NotificationHub");
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.MapRazorPages();

app.Run();