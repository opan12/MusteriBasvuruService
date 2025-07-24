using Microsoft.EntityFrameworkCore;
using MusterýBasvuruService;
using MusterýBasvuruService;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=.;Database=MusteriBasvuruDb;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
