using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusteriBasvuruService
{
    public class User
    {
        [Key]
        public Guid MusteriBasvuru_UID { get; set; }

        public string UserNo { get; set; }
        public string Username { get; set; }

        public string TCKimlikNO { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DogumTarihi { get; set; }


    }
}