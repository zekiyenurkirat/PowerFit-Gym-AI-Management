namespace WebApplication1.Models
{
    public class Antrenor
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string UzmanlikAlani { get; set; } // Kas geliştirme, Yoga, Kilo verme vb.

        // Müsaitlik Saatleri
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }

        // BAĞLANTI (Many-to-One)
        // Bir salonda çok antrenör vardır ama bir antrenör sadece bir salona bağlıdır
        public int SporSalonuId { get; set; }
        public SporSalonu SporSalonu { get; set; }

        // Antrenörün verebildiği hizmet türleri (Yoga, Fitness, Pilates)
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }
        public ICollection<Randevu> Randevular { get; set; }

    }

}
