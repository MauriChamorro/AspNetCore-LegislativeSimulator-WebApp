using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;

namespace WebAppMVC.Contexts;

// class for create a "session" with the DB
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //initialize context
        //set configs --> options
        //inyections
    }

    //DBSet, clase de EF que representa una COLECCION de entidades en el contexto
    //EF va usar esta case para LEER Y ESCRIBIR en la BASE DE DATOS
    public DbSet<Person> Persons { get; set; }
}