using System.ComponentModel.DataAnnotations;

public class AiOneriRequest
{
    [Required]
    public int Boy { get; set; }

    [Required]
    public int Kilo { get; set; }

    [Required]
    public string Hedef { get; set; } // kilo verme, kas kazanma

    [Required]
    public int HaftalikGun { get; set; }
}

