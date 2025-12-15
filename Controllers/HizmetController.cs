using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

[Authorize(Roles = "Admin")] // admin olmayan giremez şeklinde kısıtlandı 
public class HizmetController : Controller
{
    private readonly UygulamaContext _context;

    public HizmetController(UygulamaContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Hizmetler.ToList());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Hizmet hizmet)
    {
        _context.Hizmetler.Add(hizmet);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}

