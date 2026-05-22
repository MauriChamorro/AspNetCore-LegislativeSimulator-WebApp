using Microsoft.EntityFrameworkCore;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Infrastructure.DbContexts;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.Infrastructure.Repositories.DbContexts;
using WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;
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

    // inject db
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    builder.Services.AddDbContext<ExpedientesDevContext>(options => options.UseSqlServer(connectionString));
    
    // repository injections
    //.Services.AddScoped<IPersonRepository, PersonDbContext>();
    builder.Services.AddScoped<IProjectRepository, DbContextProjectRepository>();
    builder.Services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();
    
    // service injections
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<ICommissionService, CommissionService>();
    builder.Services.AddScoped<ICommissionsVmService, CommissionsVmService>();
    builder.Services.AddScoped<IProjectViewModelService, ProjectViewModelService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    
    // filter injections
    builder.Services.AddScoped<ProjectIdNotFoundAsyncFilterAttribute>();
    builder.Services.AddScoped<CanDeleteProjectAsyncFilterAttribute>();
    builder.Services.AddScoped<ProjectVmAsyncFilterAttribute>();
    
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
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(); // Esto habilita la interfaz gráfica
    }


    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error"); //testear en prod
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
    logger.LogCritical(ex, "La aplicación falló al arrancar.");
    throw; // para que el servidor sepa que falló
}