using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection")));


// Register Syncfusion licensing
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense
("Mgo+DSMBaFt+QHJqVk1hXk5Hd0BLVGpAblJ3T2ZQdVt5ZDU7a15RRnVfRFxhS3xScURgW3xadA==;Mgo+DSMBPh8sVXJ1S0R+X1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jT39RdEdiXn5dcnFVQg==;ORg4AjUWIQA/Gnt2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhSdEdhWHhfc3ZSRWQ=;MjM5NTM3OUAzMjMxMmUzMDJlMzBSSVhNYkdxY1pERERLTTlETW1wVUpRK2l3SEViNU9XQ3EzWE12OUVYWEhJPQ==;MjM5NTM4MEAzMjMxMmUzMDJlMzBNK0NCd0ozdHEzOTFDNHBOd3hXam9OM0VDeWF3TXJ1aGYwN0JGNGVlaDRVPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5Vd0dhW35ZcXdWQ2dY;MjM5NTM4MkAzMjMxMmUzMDJlMzBjTldmSVZUemZNK056QkxSdUo5R3Z3L0dZTUswdXRuLzJWMmxRV01NRlU0PQ==;MjM5NTM4M0AzMjMxMmUzMDJlMzBpdmJKWjdqdHNDVEdxVXlodCtITjlDRVpRRXpySEduWHlRQmF2UFl5VzFrPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhSdEdhWHhfc3BdQ2Q=;MjM5NTM4NUAzMjMxMmUzMDJlMzBqTmdwck5GdFNaM25LS0FBSmgza1R4T3puaVlpSk1FTDQraUNjYllDRzJzPQ==;MjM5NTM4NkAzMjMxMmUzMDJlMzBQbTdmZXBJckRiUTRSZVdzWHlYVlJvekpBVkIyTTREb1VqUTZlYlZpWUNjPQ==;MjM5NTM4N0AzMjMxMmUzMDJlMzBjTldmSVZUemZNK056QkxSdUo5R3Z3L0dZTUswdXRuLzJWMmxRV01NRlU0PQ==");


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();

