using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAppMVC.Authorization;
using WebAppMVC.Authorization.Handlers;
using WebAppMVC.Authorization.Requirements;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Filters.ExceptionFilters;
using WebAppMVC.Infrastructure.DbContexts;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.Infrastructure.Repositories.DbContexts;
using WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;
using WebAppMVC.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Iniciando el servidor web...");
    
    // inject db
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    builder.Services.AddDbContext<ExpedientesDevContext>(options => options.UseSqlServer(connectionString));
    
    // repository injections
    //.Services.AddScoped<IPersonRepository, PersonDbContext>();
    builder.Services.AddScoped<IProjectRepository, DbContextProjectRepository>();
    builder.Services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();
    
    // service injections
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<ICommissionService, CommissionService>();
    builder.Services.AddScoped<ICommissionsVmService, CommissionsVmService>();
    builder.Services.AddScoped<IProjectViewModelService, ProjectViewModelService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    builder.Services.AddScoped<ISimulationServices, HttpClientSimulation>();
    builder.Services.AddHttpClient<HttpClientSimulation>();
    
    // filter injections
    builder.Services.AddScoped<ProjectIdNotFoundAsyncFilterAttribute>();
    builder.Services.AddScoped<CanDeleteProjectAsyncFilterAttribute>();
    builder.Services.AddScoped<ProjectVmAsyncFilterAttribute>();
    builder.Services.AddScoped<ReferralCommitteesAsyncFilterAttribute>();

    builder.Services.AddAuthentication(ExpedientesAuthValues.CookieName) //crea las bases y abstracciones
        //Agrega una implementación "Scheme para Cookies" dandole el id-name
        .AddCookie(ExpedientesAuthValues.CookieName,options => 
        {
            //se hacen todas las config del esquema
            //id-nombre para identificar en el cliente/nav, en el http header
            options.Cookie.Name = ExpedientesAuthValues.CookieName;
            options.ExpireTimeSpan = TimeSpan.FromHours(5);
            options.SlidingExpiration = false;
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Legislador", policy =>
        {
            policy.RequireClaim("Legislador","true");
        });
        
        options.AddPolicy("admin", policy =>
        {
            policy.RequireClaim("admin","true");
            policy.AddRequirements(new AddProjectRequirement(10));
        });
    });
    
    //Authorization injection
    builder.Services.AddSingleton<IAuthorizationHandler, AddProjectRequirementHandler>();
    
    builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
    });
    
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
    app.UseSerilogRequestLogging();
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(); // Esto habilita la interfaz gráfica
    }


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
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "El servidor web falló inesperadamente al arrancar.");
}
finally
{
    Log.CloseAndFlush(); // Asegura que todos los logs en memoria se escriban antes de cerrar
}