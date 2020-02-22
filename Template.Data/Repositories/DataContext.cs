using Template.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Data.Repositories
{
    class DataContext : DbContext
    {
        public DbSet<User>  Users { get; set; }

        // Configure the context to use sqlite database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
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
