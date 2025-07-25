using Microsoft.EntityFrameworkCore;
using MusterýBasvuruService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(" Server=.;Database=MusteriBasvuru12345;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=False;Encrypt=False;Integrated Security=True"));

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

await app.RunAsync();
