using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

[Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
public class RandevuController : Controller
{
    private readonly UygulamaContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RandevuController(UygulamaContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 📄 LİSTELEME
    public async Task<IActionResult> Index()
    {
        IQueryable<Randevu> randevular = _context.Randevular
            .Include(r => r.Uye)
            .Include(r => r.Antrenor)
            .Include(r => r.Hizmet);

        // Eğer Admin DEĞİLSE, sadece kendi randevularını görsün
        if (!User.IsInRole("Admin"))
        {
            var userId = _userManager.GetUserId(User);
            randevular = randevular.Where(r => r.Uye.IdentityUserId == userId);
        }

        return View(await randevular.ToListAsync());
    }

    // ➕ EKLEME SAYFASI (GET)
    public IActionResult Create()
    {
        ViewBag.Antrenorler = _context.Antrenorler.ToList();
        ViewBag.Hizmetler = _context.Hizmetler.ToList();
        return View();
    }

    // 💾 KAYDETME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Randevu randevu)
    {
        // 1. Giriş yapan kullanıcının ID'sini bul
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 2. Bu kullanıcının 'Uyeler' tablosundaki kaydını bul
        var uye = await _context.Uyeler.FirstOrDefaultAsync(u => u.IdentityUserId == userId);

        if (uye == null)
        {
            // EĞER BURAYA DÜŞÜYORSAN: Register işleminde 'Uyeler' tablosuna kayıt eklememişiz demektir.
            ModelState.AddModelError("", "Hata: Kullanıcı profili bulunamadı. Lütfen yöneticinizle görüşün.");

            // Listeleri tekrar doldurup sayfayı geri döndür
            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(randevu);
        }

        // 3. Randevuyu bu üyeye ata
        randevu.UyeId = uye.Id;

        // Model validasyonu (Gelen veriler kurallara uyuyor mu?)
        // Not: Navigasyon propertyleri (Uye, Antrenor vb.) null gelebilir, bu yüzden ModelState.Remove yapıyoruz.
        ModelState.Remove("Uye");
        ModelState.Remove("Antrenor");
        ModelState.Remove("Hizmet");

        if (!ModelState.IsValid)
        {
            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(randevu);
        }

        // 4. ÇAKIŞMA KONTROLÜ (Aynı antrenöre aynı saatte randevu var mı?)
        bool cakismaVarMi = await _context.Randevular.AnyAsync(r =>
            r.AntrenorId == randevu.AntrenorId &&
            r.TarihSaat == randevu.TarihSaat
        );

        if (cakismaVarMi)
        {
            ModelState.AddModelError("", "Seçtiğiniz antrenör bu saatte maalesef dolu.");
            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(randevu);
        }

        // 5. Kaydet
        _context.Randevular.Add(randevu);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // RandevuController.cs içine eklenecek:

    // 🟢 Sadece Adminler Onaylayabilir
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Onayla(int id)
    {
        // Onaylanacak randevuyu bul
        var randevu = await _context.Randevular.FindAsync(id);

        if (randevu == null)
        {
            return NotFound();
        }

        // Durumu 'True' yap (Onaylandı)
        randevu.Onay = true;

        // Veritabanına kaydet
        await _context.SaveChangesAsync();

        // Listeye geri dön
        return RedirectToAction(nameof(Index));
    }

}