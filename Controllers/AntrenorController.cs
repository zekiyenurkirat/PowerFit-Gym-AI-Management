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
}

