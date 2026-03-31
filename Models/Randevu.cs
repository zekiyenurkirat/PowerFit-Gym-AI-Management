using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        //  Üye (otomatik atanıyor)
        public int UyeId { get; set; }
        public Uye? Uye { get; set; }

        //  Antrenör
        
        public int AntrenorId { get; set; }
        public Antrenor? Antrenor { get; set; }

        //  Hizmet
        
        public int HizmetId { get; set; }
        public Hizmet? Hizmet { get; set; }

        //  Randevu Tarih-Saat
        
        public DateTime TarihSaat { get; set; }

        // ✔ Admin onayı
        public bool Onay { get; set; }
    }
}
