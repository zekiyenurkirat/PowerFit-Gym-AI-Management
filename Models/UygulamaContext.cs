using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class UygulamaContext : DbContext
    {
        public UygulamaContext(DbContextOptions<UygulamaContext> options) : base(options)
        { 
        }
        public DbSet<SporSalonu> SporSalonları {  get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }

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


        }



    }
}
