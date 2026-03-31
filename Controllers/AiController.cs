using Microsoft.AspNetCore.Mvc;

public class AiController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(AiOneriRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        
        string sonuc = $@"
Boyunuz: {model.Boy} cm
Kilonuz: {model.Kilo} kg
Hedefiniz: {model.Hedef}

🔹 Haftalık {model.HaftalikGun} gün spor önerilir.

🏋️ Egzersiz Önerisi:
- 10 dk ısınma
- 3 gün ağırlık antrenmanı
- 2 gün kardiyo

🥗 Beslenme:
- Protein ağırlıklı beslenme
- Şekerli gıdalardan kaçınma
- Günde 2.5L su

⚠️ Bu plan yapay zekâ tarafından oluşturulmuştur.
";

        ViewBag.Sonuc = sonuc;
        return View();
    }
}
