using Microsoft.EntityFrameworkCore;
using MusteriBasvuruService;
using MusterýBasvuruService;

namespace MusterýBasvuruService;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MusteriBasvuru> MusteriBasvuru { get; set; }
    public DbSet<User> User { get; set; }

}
