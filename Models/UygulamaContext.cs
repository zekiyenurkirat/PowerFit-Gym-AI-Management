using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class UygulamaContext : IdentityDbContext
    {
        public UygulamaContext(DbContextOptions<UygulamaContext> options) : base(options)
        { 
        }
        public DbSet<SporSalonu> SporSalonları {  get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }
        public DbSet<Uye> Uyeler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SporSalonu Hizmet ilişkisi 
            modelBuilder.Entity<Hizmet>()
                .HasOne(h => h.SporSalonu)
                .WithMany(s => s.Hizmetler)
                .HasForeignKey(h => h.SporSalonuId)
                .OnDelete(DeleteBehavior.Restrict);

            // Spor salonu antrenör ilişkisi
            modelBuilder.Entity<Antrenor>()
                .HasOne(a => a.SporSalonu)
                .WithMany(s => s.Antrenorler)
                .HasForeignKey(a => a.SporSalonuId)
                .OnDelete(DeleteBehavior.Restrict);


            //çok çok ilişkisi
            modelBuilder.Entity<AntrenorHizmet>()
                .HasKey(ah => new { ah.AntrenorId, ah.HizmetId });

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Antrenor)
                .WithMany(a => a.AntrenorHizmetler)
                .HasForeignKey(ah => ah.AntrenorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Hizmet)
                .WithMany(h => h.AntrenorHizmetler)
                .HasForeignKey(ah => ah.HizmetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Uye)
                .WithMany(u => u.Randevular)
                .HasForeignKey(r => r.UyeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany(a => a.Randevular)
                .HasForeignKey(r => r.AntrenorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Cascade);
                

            // veriler
            modelBuilder.Entity<SporSalonu>().HasData(
                new SporSalonu
                {
                    Id = 1,
                    Ad = "Fitlife Spor Salonu",
                    Adres = "İstanbul /Kadıköy",
                    Telefon ="530 000 00 00",
                    AcilisSaati = new TimeSpan(7,0,0),
                    KapanisSaati = new TimeSpan(22,0,0),
                    

                }

            );

            modelBuilder.Entity<Hizmet>().HasData(
                new Hizmet
                {
                    Id = 1,
                    Ad = "Fitness",
                    SureDakika = 60,
                    Ucret = 200,
                    SporSalonuId = 1,

                },
                new Hizmet
                {
                    Id = 2,
                    Ad = "Yoga",
                    SureDakika = 45,
                    Ucret = 180,
                    SporSalonuId = 1,

                }

            );

            modelBuilder.Entity<Antrenor>().HasData(
                new Antrenor
                {
                    Id = 1,
                    Ad = "Ahmet",
                    Soyad = "Yılmaz",
                    UzmanlikAlani = "Fitness",
                    BaslangicSaati = new TimeSpan(9, 0, 0),
                    BitisSaati = new TimeSpan(17, 0, 0),
                    SporSalonuId = 1,
                },
                new Antrenor
                {
                    Id = 2,
                    Ad = "Elif",
                    Soyad = "Demir",
                    UzmanlikAlani = "Yoga",
                    BaslangicSaati = new TimeSpan(9, 0, 0),
                    BitisSaati = new TimeSpan(17, 0, 0),
                    SporSalonuId = 1,
                }
            );

            modelBuilder.Entity<AntrenorHizmet>().HasData(
                new AntrenorHizmet { AntrenorId = 1, HizmetId = 1 },
                new AntrenorHizmet { AntrenorId = 2, HizmetId = 2 }
            );








        }



    }
}
