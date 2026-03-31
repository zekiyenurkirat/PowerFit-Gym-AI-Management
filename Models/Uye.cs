namespace WebApplication1.Models
{
    public class Uye
    {
        public int Id { get; set; }

        public string? Ad { get; set; }      // opsiyonel

        public string? Soyad { get; set; }   // opsiyonel

        public string Email { get; set; } = null!; //  zorunlu

        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>(); // ✅

        public string IdentityUserId { get; set; } = null!; 
    }
}
