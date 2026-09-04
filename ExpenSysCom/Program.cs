using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ExpenSysCom.Data;
using ExpenSysCom.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ExpenSysDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Configuración de la cookie de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Errores/AccessDenied";
    options.LoginPath = "/Login/Index"; 
});


// Configuración de Identity con roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ExpenSysDBContext>()
.AddDefaultTokenProviders();

// Agregamos los servicios de controladores y vistas
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configuración del pipeline de la aplicación HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Errores/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Manejo de códigos de estado HTTP (ej: 404)
app.UseStatusCodePagesWithReExecute("/Errores/StatusCode/{0}");


// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.MapRazorPages();

// Seeder de roles y usuario administrador
async Task SeedRolesAndAdminAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    string adminEmail = "Admin@expensys.com";
    string adminPassword = "Admin456/";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = "Administrador",
            Email = adminEmail,
            EmailConfirmed = true,
            NoDocumento = "0000000000",
            PhoneNumber = "0000000000"
        };

        var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);

        if (createAdmin.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}

await SeedRolesAndAdminAsync(app.Services);



app.Run();
