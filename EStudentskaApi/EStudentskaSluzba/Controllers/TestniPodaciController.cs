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
                Email = "",
                KorisnickoIme = "admin",
                Lozinka = "admin",
                Slika = "empty",
                IsReferent = true,
                OpstinaId = opstina4.Id,
                Obrisan = false
            };
            db.Korisnici.Add(korisnik);
            db.SaveChanges();
            var smjer = new Smjer
            {
                Opis = "Razvoj softvera"
            };
            db.Smjerovi.Add(smjer);
            db.SaveChanges();
            var student = new Student
            {
              Ime="Test",
              Prezime="Student",
              Email="",
              KorisnickoIme="test.student",
              DatumDodavanja=DateTime.Now,
              DatumRodjenja=DateTime.Now,
              BrojIndeksa="IB300000",
              SmjerId = smjer.Id,
              GodinaStudija=1,
              Slika="empty",
              IsReferent=false,
              Lozinka="test",
              Obrisan=false,
              OpstinaId=opstina4.Id
            };
            db.Studenti.Add(student);
            db.SaveChanges();
            var profesor = new Nastavnik
            {
                Ime = "Test",
                Prezime = "Profesor",
                Slika = "empty",
                DatumDodavanja = DateTime.Now,
                DatumRodjenja = DateTime.Now,
                DatumZaposlenja = DateTime.Now,
                Email = "",
                IsReferent = false,
                KorisnickoIme = "test.profesor",
                Lozinka = "test",
                Obrisan = false,
                OpstinaId = opstina4.Id,
                Zvanje = "prof.dr."
            };
            db.Nastavnici.Add(profesor);
            db.SaveChanges();
        }
    }
}
