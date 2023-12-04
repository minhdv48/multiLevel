using Microsoft.EntityFrameworkCore;
using Multi_LevelModels;
using Multi_LevelModels.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(864000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//Connection
var ConnectionString = "";
var services = builder.Services;
ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<MultiLevelContext>(options => options.UseSqlServer(ConnectionString));
services.AddScoped<Ibase, Iplbase>();
services.AddOptions();
services.AddHttpContextAccessor();
services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseSession();

app.MapRazorPages();

app.Run();
