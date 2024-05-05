using EStudentskaSluzba.Data.Tabele;
using Microsoft.EntityFrameworkCore;

namespace EStudentskaSluzba.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) :base(options)
        {

        }
        public DbSet<Drzava> Drzave { get; set; }
        public DbSet<Opstina> Opstine { get; set; }
        public DbSet<Smjer> Smjerovi { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Nastavnik> Nastavnici { get; set; }
        public DbSet<Prijava> Prijave { get; set; }
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Ocjena> Ocjene { get; set; }
        public DbSet<Upis> Upisi { get; set; }
        public DbSet<Rata> Rate { get; set; }
    }
}
