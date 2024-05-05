using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Route ("[controller]/[action]")]
    public class TestniPodaciController
    {
        private readonly MyDbContext db;
        public TestniPodaciController(MyDbContext _db)
        {
            db = _db;
        }
        [HttpPost()]
        public void GenerisiTestnePodatke ()
        {
            var drzava1 = new Drzava
            {
                Naziv = "Bosna i Hercegovina"
            };
            db.Drzave.Add(drzava1);
            db.SaveChanges();
            var opstina4 = new Opstina { Naziv = "Mostar", DrzavaId = drzava1.Id };
            db.Opstine.Add(opstina4);
            db.SaveChanges();
            var korisnik = new Korisnik
            {
                Ime = "Admin",
                Prezime = "Admin",
                DatumRodjenja = DateTime.Now,
                DatumDodavanja = DateTime.Now,
                Email = "adszarada55@gmail.com",
                KorisnickoIme = "admin",
                Lozinka = "admin",
                Slika = "empty",
                IsReferent = true,
                OpstinaId = opstina4.Id,
                Obrisan = false
            };
            db.Korisnici.Add(korisnik);
            db.SaveChanges();
        }
    }
}
