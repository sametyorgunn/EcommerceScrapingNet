using BusinessLayer.ServiceExtension;
using DataAccessLayer.Contexts;
using EntityLayer.MappingProfiles;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServiceRouting();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAuthentication("CookieAuth")
       .AddCookie("CookieAuth", config =>
       {
           config.LoginPath = "/Login/SignIn"; 
       });
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(300);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddSession();
var app = builder.Build();

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
app.UseSession();


app.MapControllerRoute(
	 name: "areas",
	 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");



app.Run();
