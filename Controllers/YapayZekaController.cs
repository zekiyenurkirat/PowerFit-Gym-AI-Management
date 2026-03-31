using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class YapayZekaController : Controller
    {
        // Senin çalışan API Anahtarın
        private readonly string _apiKey = "AIzaSyBHCAkMTwehDi8uyIaJtTQtUBcbibP9IDQ"; // DİKKAT: Buraya kendi AIza.. ile başlayan kodunu yapıştır!

        private readonly HttpClient _httpClient;

        public YapayZekaController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new YapayZekaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> OneriAl(YapayZekaViewModel model)
        {
            // 1. İSTEK METNİ HAZIRLAMA
            string prompt = $"Ben {model.Yas} yaşında, {model.Boy} cm boyunda, {model.Kilo} kg ağırlığında bir {model.Cinsiyet} bireyim. " +
                            $"Hedefim: {model.Hedef}. " +
                            $"Bana spor salonunda yapabileceğim 3 maddelik kısa bir egzersiz listesi ve 1 günlük örnek diyet listesi hazırla. " +
                            $"Cevabı HTML formatında (ul, li, b etiketleri kullanarak) ver.";

            
            // Listende "gemini-1.5-flash" yok ama "gemini-flash-latest" var. Bunu kullanıyoruz.
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[] {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                // 3. GOOGLE'A GÖNDERME
                var response = await _httpClient.PostAsync(url, jsonContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using (JsonDocument doc = JsonDocument.Parse(responseString))
                    {
                        // Cevabı ayıklıyoruz
                        var text = doc.RootElement
                            .GetProperty("candidates")[0]
                            .GetProperty("content")
                            .GetProperty("parts")[0]
                            .GetProperty("text")
                            .GetString();

                        model.OneriSonuc = text?.Replace("```html", "").Replace("```", "");
                    }
                }
                else
                {
                    model.OneriSonuc = $"Hata: {response.StatusCode} - {responseString}";
                }
            }
            catch (Exception ex)
            {
                model.OneriSonuc = "Bağlantı Hatası: " + ex.Message;
            }

            // 4. RESİM 
            if (model.Cinsiyet == "Erkek")
                model.OlusanResimUrl = "https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?auto=format&fit=crop&w=500&q=80";
            else
                model.OlusanResimUrl = "https://images.unsplash.com/photo-1518611012118-696072aa579a?auto=format&fit=crop&w=500&q=80";

            return View("Index", model);
        }
    }
}