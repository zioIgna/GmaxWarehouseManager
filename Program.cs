using Gmax.Data;
using Gmax.Extensions;
using Gmax.Models.Services.ArticoloCK;
using Gmax.Models.Services.Assegnazione;
using Gmax.Models.Services.Giacenza;
using Gmax.Models.Services.Magazzino;
using Gmax.Models.Services.OrdineCK;
using Gmax.Models.Services.OrdineProdCompCK;
using Gmax.Models.Services.Validazione;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GmaxDbContext>(options =>
    //options.UseOracle(@"User Id=gmax_user;Password=gMaxDevelopment2024;Data Source=pdborcl;")
    //options.UseOracle(@"User Id=gmax_user;Password=gMaxDevelopment2024;Data Source=localhost:1521/FREEPDB1;")
    //options.UseOracle(@"User Id=gmax_user;Password=gMaxDevelopment2024;Data Source=oracleDb:1521/FREEPDB1;")
    //options.UseNpgsql(builder.Configuration.GetConnectionString(conn))
    options.UseNpgsql("User ID=gmax_user;Password=gMaxDevelopment2024;Server=postgresDb;Port=5432;Database=postgresDb;Pooling=true;Include Error Detail=true;")
    //options.UseOracle(@"SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=FREEPDB1)));uid=gmax_user ;pwd=gMaxDevelopment2024;")
    );

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IOrdineProduzioneCKService,  OrdineProduzioneCKService>();
builder.Services.AddScoped<IArticoloCKService, ArticoloCKService>();
builder.Services.AddScoped<IOrdineProdCompCKService, OrdineProdCompCKService>();
builder.Services.AddScoped<IGiacenzaService, GiacenzaService>();
builder.Services.AddScoped<IMagazzinoService, MagazzinoService>();
builder.Services.AddScoped<IAssegnazioneService,  AssegnazioneService>();
builder.Services.AddScoped<IAssegnazioneValidator, AssegnazioneValidator>();

var app = builder.Build();

app.ApplyMigrations();

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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Articoloes}/{action=Index}/{id?}");

app.Run();
