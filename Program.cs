using InmobiliariaApp.Data;
using InmobiliariaApp.Repositories;

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
