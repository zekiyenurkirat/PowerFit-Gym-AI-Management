using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

[Authorize(Roles = "Admin")]
public class HizmetController : Controller
{
    private readonly UygulamaContext _context;

    public HizmetController(UygulamaContext context)
    {
        _context = context;
    }

    // LİSTELEME
    public IActionResult Index()
    {
        // İlişkili olduğu spor salonu bilgisini de dahil edebiliriz ama şimdilik düz listeyelim
        return View(_context.Hizmetler.ToList());
    }

    // EKLEME SAYFASI (GET)
    public IActionResult Create()
    {
        ViewBag.SporSalonlari = _context.SporSalonları.ToList();
        return View();
    }

    // EKLEME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Hizmet hizmet)
    {
        // İlişkisel alan hatalarını yoksay
        ModelState.Remove("SporSalonu");
        ModelState.Remove("AntrenorHizmetler");
        ModelState.Remove("Randevular");

        if (!ModelState.IsValid)
        {
            ViewBag.SporSalonlari = _context.SporSalonları.ToList();
            return View(hizmet);
        }

        _context.Hizmetler.Add(hizmet);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // SİLME ONAY SAYFASI (GET)
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hizmet = _context.Hizmetler.FirstOrDefault(m => m.Id == id);
        if (hizmet == null)
        {
            return NotFound();
        }

        return View(hizmet);
    }

    // SİLME İŞLEMİ (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var hizmet = _context.Hizmetler.Find(id);
        if (hizmet != null)
        {
            // 1. Bu hizmete ait RANDEVULARI temizle
            var randevular = _context.Randevular.Where(r => r.HizmetId == id).ToList();
            if (randevular.Count > 0)
            {
                _context.Randevular.RemoveRange(randevular);
            }

            // 2. Bu hizmetin Antrenörlerle olan bağını (AntrenorHizmet tablosunu) temizle
            var baglantilar = _context.AntrenorHizmetler.Where(ah => ah.HizmetId == id).ToList();
            if (baglantilar.Count > 0)
            {
                _context.AntrenorHizmetler.RemoveRange(baglantilar);
            }

            // 3. Hizmeti sil
            _context.Hizmetler.Remove(hizmet);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}
