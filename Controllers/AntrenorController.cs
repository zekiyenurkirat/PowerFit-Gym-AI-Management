using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

[Authorize(Roles = "Admin")]
public class AntrenorController : Controller
{
    private readonly UygulamaContext _context;

    public AntrenorController(UygulamaContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Antrenorler.ToList());
    }

    public IActionResult Create()
    {
        ViewBag.SporSalonlari = _context.SporSalonları.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Antrenor antrenor)
    {
        ModelState.Remove("SporSalonu"); //sorun
        ModelState.Remove("AntrenorHizmetler");
        ModelState.Remove("Randevular");
        if (!ModelState.IsValid)
        {
            ViewBag.SporSalonlari = _context.SporSalonları.ToList();
            return View(antrenor);
        }

        _context.Antrenorler.Add(antrenor);
        _context.SaveChanges(); // bunla ekliyoruz derste öğrendin database
        return RedirectToAction(nameof(Index));
    }

    // 1. AŞAMA: Silme onay sayfasını getiren metod  
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var antrenor = _context.Antrenorler.FirstOrDefault(m => m.Id == id);
        if (antrenor == null)
        {
            return NotFound();
        }

        return View(antrenor);
    }

    // AntrenorController.cs dosyasının içindeki DeleteConfirmed metodunu bul ve bununla değiştir:

    // POST: Antrenor/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Silinecek antrenörü ve ona bağlı randevuları getiriyoruz
        var antrenor = await _context.Antrenorler
            .Include(a => a.Randevular) // 👈 Önemli: Randevuları da dahil et
            .FirstOrDefaultAsync(x => x.Id == id);

        if (antrenor != null)
        {
            // 1. Önce bu antrenöre ait gelecekteki randevuları siliyoruz
            if (antrenor.Randevular != null && antrenor.Randevular.Any())
            {
                _context.Randevular.RemoveRange(antrenor.Randevular);
            }

            // 2. Sonra antrenörün kendisini siliyoruz
            _context.Antrenorler.Remove(antrenor);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }


    // GÜNCELLEME SAYFASINI AÇAN METOD (GET)
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var antrenor = _context.Antrenorler.Find(id);
        if (antrenor == null)
        {
            return NotFound();
        }

        // Dropdown (Açılır kutu) için spor salonlarını tekrar yüklüyoruz
        ViewBag.SporSalonlari = _context.SporSalonları.ToList();
        return View(antrenor);
    }

    // GÜNCELLEME İŞLEMİNİ YAPAN METOD (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Antrenor antrenor)
    {
        if (id != antrenor.Id)
        {
            return NotFound();
        }

        // İlişkisel alan hatalarını yine yoksayıyoruz
        ModelState.Remove("SporSalonu");
        ModelState.Remove("AntrenorHizmetler");
        ModelState.Remove("Randevular");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(antrenor);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Antrenorler.Any(e => e.Id == antrenor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Hata varsa sayfayı tekrar doldurup göster
        ViewBag.SporSalonlari = _context.SporSalonları.ToList();
        return View(antrenor);
    }


}

