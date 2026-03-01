using System.Net.Http.Headers;
using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme =
    options.DefaultScheme = 
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // Check if ServiceTypes exist, if not seed them
    if (!context.ServiceTypes.Any())
    {
        Console.WriteLine("Seeding ServiceTypes...");
        context.ServiceTypes.AddRange(
            new DOTNETCORE_DEV.Models.serviceTypes { serviceTypesName = "MS Office" },
            new DOTNETCORE_DEV.Models.serviceTypes { serviceTypesName = "PC/Mobile Device/Notebook/Printer" },
            new DOTNETCORE_DEV.Models.serviceTypes { serviceTypesName = "ขอติดตั้ง Software" },
            new DOTNETCORE_DEV.Models.serviceTypes { serviceTypesName = "ขอบริการเกี่ยวกับบัญชีผู้ใช้งาน" },
            new DOTNETCORE_DEV.Models.serviceTypes { serviceTypesName = "ระบบงานภายนอก" },
            new DOTNETCORE_DEV.Models.serviceTypes { serviceTypesName = "ระบบงานภายใน" }
        );
    }
    
    // Check if Employees exist, if not seed them
    if (!context.Employees.Any())
    {
        Console.WriteLine("Seeding Employees...");
        context.Employees.AddRange(
            new DOTNETCORE_DEV.Models.Employee { Name = "สมชาย ใจดี", SupportLevel = 1 },
            new DOTNETCORE_DEV.Models.Employee { Name = "มานี รักดี", SupportLevel = 1 }
        );
    }
    
    context.SaveChanges();
    Console.WriteLine("Database seeding completed.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
