namespace WebApplication1.Models
{
    public class SporSalonu
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }


        // çalışma saatleri
        public  TimeSpan AcilisSaati { get; set; }
        public TimeSpan KapanisSaati { get; set; }


        // salon hizmetler: fitness yoga pilates vb
        public ICollection<Hizmet> Hizmetler {  get; set; }

        // salonun antrenörleri de buraya yazılacak (1 salonda çok antrenör olabilir)
        public ICollection<Antrenor> Antrenorler { get; set; }





    }
}
