using HardwareHelper.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/BrakDostepu";
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var MenadzerRol = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();

    string[] NazwyRol = { "Admin", "Serwisant", "Klient" };

    foreach (var NazwaRoli in NazwyRol)
    {  
        var RolaIstnieje = await MenadzerRol.RoleExistsAsync(NazwaRoli);
        if (!RolaIstnieje)
        {
            await MenadzerRol.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(NazwaRoli));
        }
    }
    var MenadzerUzytkownikow = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string emailAdmina = "admin@hardware.pl";
    string hasloAdmina = "Hardware123!";

    var uzytkownikAdmin = await MenadzerUzytkownikow.FindByEmailAsync(emailAdmina);

    if (uzytkownikAdmin == null)
    {
        var nowyAdmin = new IdentityUser
        {
            UserName = emailAdmina,
            Email = emailAdmina,
            EmailConfirmed = true
        };

        var wynikTworzenia = await MenadzerUzytkownikow.CreateAsync(nowyAdmin, hasloAdmina);

        if (wynikTworzenia.Succeeded)
        {
            await MenadzerUzytkownikow.AddToRoleAsync(nowyAdmin, "Admin");
            await MenadzerUzytkownikow.AddToRoleAsync(nowyAdmin, "Serwisant");
        }
    }
}
app.Run();
