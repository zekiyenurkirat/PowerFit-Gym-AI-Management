namespace WebApplication1.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int SureDakika { get; set; }
        public decimal Ucret { get; set; }

        // bu antrenör hangi salona ait (unutma 1 antrenör yalnız 1 salona ait olabilir)
        public int SporSalonuId { get; set; }
        public SporSalonu SporSalonu { get; set; } // sporsalonu türünden sporsalonu değilkeni aldık yani buaradan çalıştığı sporsalonu adını alıcaz galiba
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }
        public ICollection<Randevu> Randevular { get; set; }



    }
}
