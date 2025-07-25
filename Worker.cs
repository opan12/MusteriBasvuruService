using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MusterıBasvuruService;
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
                    Console.WriteLine("bekleyenbasvuru");

                    var bekleyenBasvurular = await db.MusteriBasvuru
                        .Where(x => x.BasvuruDurum == Durum.Onaylandi && x.Kayit_Durum == "Aktif")
                        .ToListAsync(stoppingToken);

                    Console.WriteLine("döngü");
                    foreach (var basvuru in bekleyenBasvurular)
                    {
                        var eskiDurum = basvuru.BasvuruDurum;

                        var user = await db.User.FirstOrDefaultAsync(u => u.MusteriBasvuru_UID == basvuru.Basvuru_UID); // örnek eşleşme

                        if (user != null)
                        {
                            var yas = DateTime.Today.Year - user.DogumTarihi.Year;
                            if (user.DogumTarihi > DateTime.Today.AddYears(-yas)) yas--;

                            if (yas >= 18)
                            {

                                basvuru.BasvuruDurum = Durum.Onaylandi;
                                basvuru.HataAciklama = "Şartlar uygun, otomatik onaylandı";
                            }


                            else if (user.TCKimlikNO.StartsWith("A"))
                            {
                                Console.WriteLine("else if");

                                basvuru.BasvuruDurum = Durum.Reddedildi;
                                basvuru.HataAciklama = "Şartlar sağlanamadı, otomatik reddedildi";
                            }
                            else
                            {
                                Console.WriteLine("if");

                                basvuru.BasvuruDurum = Durum.Reddedildi;
                                basvuru.HataAciklama = "Şartlar sağlanamadı, otomatik reddedildi";
                            }
                            _logger.LogInformation("Basvuru {uid} durumu: {once} => {sonra}", basvuru.Basvuru_UID, eskiDurum, basvuru.BasvuruDurum);
                        }

                        var changes = await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("{count} kayıt güncellendi.", changes);
                    }

                    _logger.LogInformation("Kontrol tamamlandı: {time}", DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }
    }
}