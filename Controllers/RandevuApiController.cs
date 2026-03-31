using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Data; // Context dosyan buradaysa bu namespace gerekli

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış kullanıcılar API'ye erişebilir
    public class RandevuApiController : ControllerBase
    {
        private readonly UygulamaContext _context;

        public RandevuApiController(UygulamaContext context)
        {
            _context = context;
        }

        // 🔹 ÜYE → KENDİ RANDEVULARI (JSON FORMATINDA)
        // URL: https://localhost:port/api/RandevuApi/uye-randevularim
        [HttpGet("uye-randevularim")]
        public async Task<IActionResult> GetUyeRandevularim()
        {
            // Giriş yapan kullanıcının ID'sini bul
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Üye tablosunda bu kullanıcıyı bul
            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.IdentityUserId == userId);

            if (uye == null)
                return Unauthorized("Üye profili bulunamadı.");

            //  LINQ FİLTRELEME: Sadece bu üyeye ait randevuları getir
            var randevular = await _context.Randevular
                .Where(r => r.UyeId == uye.Id)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .OrderByDescending(r => r.TarihSaat) // En yeni randevu en üstte olsun
                .Select(r => new
                {
                    r.Id,
                    Tarih = r.TarihSaat.ToString("yyyy-MM-dd HH:mm"), // Tarihi okunaklı yap
                    Antrenor = r.Antrenor != null ? r.Antrenor.Ad + " " + r.Antrenor.Soyad : "Silinmiş Antrenör",
                    Hizmet = r.Hizmet != null ? r.Hizmet.Ad : "Silinmiş Hizmet",
                    OnayDurumu = r.Onay ? "Onaylandı" : "Bekliyor"
                })
                .ToListAsync();

            return Ok(randevular);
        }

        //  ADMIN -> TÜM RANDEVULAR
        // URL: https://localhost:port/api/RandevuApi/admin-tum-randevular
        [HttpGet("admin-tum-randevular")]
        [Authorize(Roles = "Admin")] // Sadece Admin girebilir
        public async Task<IActionResult> GetTumRandevular()
        {
            var randevular = await _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .OrderByDescending(r => r.TarihSaat)
                .Select(r => new
                {
                    r.Id,
                    Uye = r.Uye != null ? r.Uye.Ad + " " + r.Uye.Soyad : "Silinmiş Üye",
                    Antrenor = r.Antrenor != null ? r.Antrenor.Ad + " " + r.Antrenor.Soyad : "Silinmiş Antrenör",
                    Hizmet = r.Hizmet != null ? r.Hizmet.Ad : "Silinmiş Hizmet",
                    Tarih = r.TarihSaat.ToString("yyyy-MM-dd HH:mm"),
                    OnayDurumu = r.Onay ? "Onaylandı" : "Bekliyor"
                })
                .ToListAsync();

            return Ok(randevular);
        }
    }
}