using System;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=tcp:localhost,1433;Initial Catalog=Database;User Id=sa;Password=J39eta2jk6A7f945bdfd;");
    }
}
