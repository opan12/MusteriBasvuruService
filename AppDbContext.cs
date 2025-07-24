using Microsoft.EntityFrameworkCore;
using MusterýBasvuruService;

namespace MusterýBasvuruService;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MusteriBasvuru> _MusteriBasvuru { get; set; }
}
