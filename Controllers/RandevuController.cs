using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

[Authorize]
public class RandevuController : Controller
{
    private readonly UygulamaContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RandevuController(UygulamaContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        IQueryable<Randevu> randevular = _context.Randevular
            .Include(r => r.Uye)
            .Include(r => r.Antrenor)
            .Include(r => r.Hizmet);

        // 👑 Admin her şeyi görür
        if (!User.IsInRole("Admin"))
        {
            var userId = _userManager.GetUserId(User);

            randevular = randevular
                .Where(r => r.Uye.IdentityUserId == userId);
        }

        return View(await randevular.ToListAsync());
    }


    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Antrenorler = _context.Antrenorler.ToList();
        ViewBag.Hizmetler = _context.Hizmetler.ToList();
        return View(); //antrenör ve hizmet bilgisi getirme lagin yapanlar için antrenör ve hizmet bilgisini getirir
    }


    // 🔴 CREATE (POST) → KAYDET
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Randevu randevu)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        // 🔵 ÜYEYİ BUL
        var uye = await _context.Uyeler
            .FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);

        // 🔵 YOKSA OTOMATİK OLUŞTUR
        if (uye == null)
        {
            uye = new Uye
            {
                Email = user.Email,
                Ad = user.Email,
                IdentityUserId = user.Id
            };

            _context.Uyeler.Add(uye);
            await _context.SaveChangesAsync();
        }

        // 🔴 ÇAKIŞMA KONTROLÜ (ÖNCE!)
        bool cakismaVarMi = await _context.Randevular.AnyAsync(r =>
            r.AntrenorId == randevu.AntrenorId &&
            r.TarihSaat == randevu.TarihSaat
        );

        if (cakismaVarMi)
        {
            ModelState.AddModelError("", "Bu antrenör bu saatte dolu.");
            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(randevu);
        }

        // 🔵 KAYDET
        randevu.UyeId = uye.Id;

        _context.Randevular.Add(randevu);
        await _context.SaveChangesAsync();

        // buraya ekledinnnn
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(randevu);
        }

        return RedirectToAction(nameof(Index));




    }






}
