namespace WebApplication1.Models
{
    public class YapayZekaViewModel
    {
        public int Yas { get; set; }
        public int Kilo { get; set; }
        public int Boy { get; set; }
        public string Cinsiyet { get; set; } // "Erkek" veya "Kadın"
        public string Hedef { get; set; } // "Kilo Vermek", "Kas Yapmak" vb.

        // Yapay zekadan gelen cevabı buraya yazacağız
        public string? OneriSonuc { get; set; }

        // Resim linkini buraya yazacağız
        public string? OlusanResimUrl { get; set; }
    }
}
