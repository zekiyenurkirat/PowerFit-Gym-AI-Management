using Microsoft.AspNetCore.Identity.UI.Services;
using WebApplication1.Services;


using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<UygulamaContext>(options =>
    options.UseSqlServer(connectionString));

// ?? Identity + Roller
// Program.cs içinde bu kýsmý bul ve böyle güncelle:

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Þifre kurallarýný "sau" yazabilmek için esnetiyoruz
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3; // En az 3 karakter olsun (sau)
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<UygulamaContext>()
.AddDefaultTokenProviders();






// buraya üyeler eriþemez sayfasý geldi

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LoginPath = "/Identity/Account/Login";
});


builder.Services.AddScoped<IEmailSender, EmailSender>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Identity UI için

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Identity sayfalarý

// eklediðim yer
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    Task.Run(async () =>
    {
        string[] roles = { "Admin", "Uye" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "b231210388@sakarya.edu.tr";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var user = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "sau");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }).Wait();
}


app.Run();
