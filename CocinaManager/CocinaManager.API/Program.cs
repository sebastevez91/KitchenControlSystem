using CocinaManager.API.Middleware;
using CocinaManager.Application.Interfaces;
using CocinaManager.Application.Services;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using CocinaManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CocinaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<IMovimientoStockRepository, MovimientoStockRepository>();
builder.Services.AddScoped<IHerramientaRepository, HerramientaRepository>();
builder.Services.AddScoped<IOrdenMantenimientoRepository, OrdenMantenimientoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
builder.Services.AddScoped<IPlanMenuRepository, PlanMenuRepository>();
builder.Services.AddScoped<IIncidenteRepository, IncidenteRepository>();
builder.Services.AddScoped<IAusenciaRepository, AusenciaRepository>();
builder.Services.AddScoped<IMensajeRepository, MensajeRepository>();

// Servicios
builder.Services.AddScoped<IPersonalService, PersonalService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<IMovimientoStockService, MovimientoStockService>();
builder.Services.AddScoped<IHerramientaService, HerramientaService>();
builder.Services.AddScoped<IOrdenMantenimientoService, OrdenMantenimientoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IRecetaService, RecetaService>();
builder.Services.AddScoped<IPlanMenuService, PlanMenuService>();
builder.Services.AddScoped<IIncidenteService, IncidenteService>();
builder.Services.AddScoped<IAusenciaService, AusenciaService>();
builder.Services.AddScoped<IMensajeService, MensajeService>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.RoutePrefix = "swagger"); // queda en /swagger
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api-docs", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapFallbackToPage("/Index");
app.MapControllers();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CocinaDbContext>();
    await DbSeeder.SeedAsync(context);
}

app.Run();