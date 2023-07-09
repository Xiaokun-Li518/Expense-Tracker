using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection")));



// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews().AddJsonOptions(x => 
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);



// Register Syncfusion licensing
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense
("Ngo9BigBOggjHTQxAR8/V1NGaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWXhecXZURmheVkZ/Wko=");


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // add UseAuthentication before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();

