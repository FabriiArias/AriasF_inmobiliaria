using InmobiliariaApp.Data;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder de la db
builder.Services.AddSingleton<DatabaseConnection>();


// levantamos los repo
builder.Services.AddScoped<PropietarioRepo>();
builder.Services.AddScoped<InquilinoRepo>();
builder.Services.AddScoped<InmuebleRepo>();
builder.Services.AddScoped<ContratoRepo>();
builder.Services.AddScoped<PagoRepo>();
builder.Services.AddScoped<UsuarioRepo>();



// autenticacion de cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/Home/Logout";
        options.AccessDeniedPath = "/Home/Register";
        options.SlidingExpiration = false;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.MaxAge = null; 
    });


builder.Services.AddAuthorization();



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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
