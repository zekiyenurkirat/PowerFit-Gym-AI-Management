namespace WebApplication1.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        public int UyeId { get; set; }
        public Uye Uye { get; set; }

        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        public DateTime Tarih {  get; set; }
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }

        public bool Onay {  get; set; }


        
        

    }
}
