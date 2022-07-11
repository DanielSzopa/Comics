using Comics.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Comics.ApplicationCore.Data;

public class ComicsDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public ComicsDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}