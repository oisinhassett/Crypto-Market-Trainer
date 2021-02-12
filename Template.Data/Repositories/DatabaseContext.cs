using System;
using Microsoft.EntityFrameworkCore;

// import the Models (representing structure of tables in database)
using Template.Core.Models; 

namespace Template.Data.Repositories
{
    // The Context is How EntityFramework communicates with the database
    // We define DbSet properties for each table in the database
    public class DatabaseContext :DbContext
    {
         // authentication store
        public DbSet<User> Users { get; set; }

        // Configure the context to use Specified database. We are using 
        // Sqlite database as it does not require any additional installations.
        // Could use SqlServer using connection below if installed
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder                  
                .UseSqlite("Filename=api.db")
                .LogTo(Console.WriteLine);
                //.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=SMS; Trusted_Connection=True;ConnectRetryCount=0");
        }

        // Convenience method to recreate the database thus ensuring  the new database takes 
        // account of any changes to the Models or DatabaseContext
        public void Initialise()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

    }
}
