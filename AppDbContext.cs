using Microsoft.EntityFrameworkCore;
using MusteriBasvuruService;
using Muster�BasvuruService;

namespace Muster�BasvuruService;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MusteriBasvuru> MusteriBasvuru { get; set; }
    public DbSet<User> User { get; set; }

}
