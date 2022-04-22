using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Data
{
    public class CinemaDbContext: DbContext
    {
        //NB: that the CinemaDbContext is the one that relates with the database to perform queries ...
        //its inhering the EntityFrameWorkCore Dbcontext responsible for talking to the database 
        //so once it's registered in the Dependency Injection Container (Startup.cs) ...its active.
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

    }




}
