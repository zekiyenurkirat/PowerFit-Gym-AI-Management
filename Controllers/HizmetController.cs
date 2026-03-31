using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

[Authorize]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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

    

    // POST: Hizmet/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Silinecek hizmeti ve ona bağlı randevuları getir
        var hizmet = await _context.Hizmetler
            .Include(h => h.Randevular) //  Randevuları da dahil et
            .FirstOrDefaultAsync(x => x.Id == id);

        if (hizmet != null)
        {
            // 1. Önce bu hizmete ait randevuları sil
            if (hizmet.Randevular != null && hizmet.Randevular.Any())
            {
                _context.Randevular.RemoveRange(hizmet.Randevular);
            }

            // 2. Sonra hizmetin kendisini sil
            _context.Hizmetler.Remove(hizmet);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GÜNCELLEME SAYFASI (GET)
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var hizmet = _context.Hizmetler.Find(id);
        if (hizmet == null) return NotFound();

        ViewBag.SporSalonlari = _context.SporSalonları.ToList();
        return View(hizmet);
    }

    // GÜNCELLEME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Hizmet hizmet)
    {
        if (id != hizmet.Id) return NotFound();

        ModelState.Remove("SporSalonu");
        ModelState.Remove("AntrenorHizmetler");
        ModelState.Remove("Randevular");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hizmet);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Hizmetler.Any(e => e.Id == hizmet.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.SporSalonlari = _context.SporSalonları.ToList();
        return View(hizmet);
    }

}
