using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// required to add console logging
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Template.Core.Models;

namespace Template.Data.Repositories
{
    class DataContext : DbContext
    {
        public DbSet<User>  Users { get; set; }

        // Configure the context to use sqlite database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                /** use logger to log the sql commands issued by entityframework **/
                .UseLoggerFactory(new ServiceCollection()
                     .AddLogging(builder => builder.AddConsole())
                     .BuildServiceProvider()
                     .GetService<ILoggerFactory>()
                 )   
                //.UseSqlServer("COMPLETE THIS")
                .UseSqlite("Filename=Template.db");
        }

        // Convenience method to recreate the database thus ensuring the new database takes account of any 
        // changes to the Models or DatabaseContext
        public void Initialise()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

    }
}
