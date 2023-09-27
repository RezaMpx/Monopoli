using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monopoli
{
    public class MonoDB : DbContext
    {
        public MonoDB(DbContextOptions<MonoDB> options) : base(options) { }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Place> Places { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Room>().Property("Id").ValueGeneratedNever();
            builder.Entity<Player>().Property("Id").ValueGeneratedNever();
        }
    }
}
