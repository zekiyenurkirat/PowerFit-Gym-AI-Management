using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    // 2. AŞAMA: Gerçekten silme işlemini yapan metod  
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var antrenor = _context.Antrenorler.Find(id);
        if (antrenor != null)
        {
            _context.Antrenorler.Remove(antrenor);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

}

