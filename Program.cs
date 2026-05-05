using Microsoft.EntityFrameworkCore;
using WebAppMVC.Contexts;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;
using WebAppMVC.Infrastructure.Repositories.DbContexts;
using WebAppMVC.Infrastructure.Services;
using WebAppMVC.Services;

var builder = WebApplication.CreateBuilder(args);
//System.IO.File.WriteAllText("identificador_arranque.txt", "builder");

// Crear un logger manual usando la configuración del builder
using var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

ILogger logger = loggerFactory.CreateLogger("Startup");
try
{
    logger.LogInformation("Configurando servicios...");
    logger.LogInformation("GetConnectionString");
//system.IO.File.WriteAllText("identificador_arranque.txt", "GetConnectionString");
// Add services to the container.

    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
    builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IProjectViewModelService, ProjectViewModelService>();
    builder.Services.AddScoped<IPersonRepository, PersonDbContext>();
    builder.Services.AddControllersWithViews();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(); // Esto habilita la interfaz gráfica
    }


// Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAllOrigins");
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    // ESTO ES LO QUE NECESITAS EN MONSTERASP
    logger.LogCritical(ex, "La aplicación falló al arrancar.");
    throw; // Re-lanzar para que el servidor sepa que falló
}