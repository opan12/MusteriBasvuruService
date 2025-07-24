using Microsoft.EntityFrameworkCore;
using Muster�BasvuruService;
using Muster�BasvuruService;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=.;Database=MusteriBasvuruDb;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
