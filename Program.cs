using Microsoft.EntityFrameworkCore;
using WebAppMVC.Contexts;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;
using WebAppMVC.Infrastructure.Repositories.DbContexts;
using WebAppMVC.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

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

    // Add services.
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    
    builder.Services.AddScoped<IPersonRepository, PersonDbContext>();
    builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
    builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
    builder.Services.AddSingleton<IReferralCommissionRepository, ReferralCommissionRepository>();
    
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<ICommissionService, CommissionService>();
    builder.Services.AddScoped<ICommissionsVmService, CommissionsVmService>();
    builder.Services.AddScoped<IProjectViewModelService, ProjectViewModelService>();
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
    app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    app.Run();
}
catch (Exception ex)
{
    // ESTO ES LO QUE NECESITAS EN MONSTERASP
    logger.LogCritical(ex, "La aplicación falló al arrancar.");
    throw; // Re-lanzar para que el servidor sepa que falló
}