using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusterıBasvuruService;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace MusterıBasvuruService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker başlatıldı: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var bekleyenBasvurular = db._MusteriBasvuru
                        .Where(x => x.BasvuruDurum == Durum.Beklemede && x.Kayit_Durum == "Aktif")
                        .ToList();

                    foreach (var basvuru in bekleyenBasvurular)
                    {
                        if (basvuru.MusteriNo.StartsWith("A"))
                        {
                            basvuru.BasvuruDurum = Durum.Onaylandi;
                            basvuru.HataAciklama = "Şartlar uygun, otomatik onaylandı";
                        }
                        else
                        {
                            basvuru.BasvuruDurum = Durum.Reddedildi;
                            basvuru.HataAciklama = "Şartlar sağlanamadı, otomatik reddedildi";
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }

                _logger.LogInformation("Kontrol tamamlandı: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // 10 saniyede bir çalışır
            }
        }
    }
}