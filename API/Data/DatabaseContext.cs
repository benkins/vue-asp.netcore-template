using System;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=Application;Integrated Security=True;TrustServerCertificate=False;");
    }
}
