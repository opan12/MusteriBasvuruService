using Microsoft.EntityFrameworkCore;
using Muster�BasvuruService;

namespace Muster�BasvuruService;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MusteriBasvuru> _MusteriBasvuru { get; set; }
}
