using System;
using Microsoft.EntityFrameworkCore;
using MMTDigital.Model;

namespace MMTDigital.Context
{
    public class ConnectionDB: DbContext
    {
        public ConnectionDB(DbContextOptions<ConnectionDB> options):base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
