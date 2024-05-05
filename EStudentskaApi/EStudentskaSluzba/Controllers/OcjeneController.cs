using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OcjeneController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public OcjeneController(MyDbContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpPost]
        public async Task<AddOcjenaResponse> addOcjena([FromBody] AddOcjenaRequest add)
        {
            var prijava = await auth.getInfo();
            if (!prijava.JeLogiran || prijava.Prijava.Korisnik.isStudent) return new AddOcjenaResponse
            { Dodana = false, Greska = "Nemate pravo pristupa!" };
            var student = await db.Studenti.Where(s => s.Obrisan != true).
                FirstOrDefaultAsync(s => s.Id == add.StudentId);
            if (student == null) return new AddOcjenaResponse
            { Dodana = false, Greska = "Ne postoji student!" };
            if (await db.Ocjene.Where(o=> o.PredmetId == add.PredmetId && o.StudentId == add.StudentId).AnyAsync())
            {
                return new AddOcjenaResponse
                { Dodana = false, Greska = "Ocjena je vec unesena!" };
            }
            if (prijava.Prijava.Korisnik.isNastavnik)
            {
                var predmeti = await db.Predmeti.Include(p => p.Nastavnik).Where(
                    p => p.NastavnikId == prijava.Prijava.KorisnikId).ToListAsync();
                if (predmeti.Find(p => p.Id == add.PredmetId) == null) return
                        new AddOcjenaResponse
                        { Dodana = false, Greska = "Nemate pravo pristupa!" };
            }
            var ocjena = new Ocjena
            {
                PredmetId = add.PredmetId,
                StudentId = add.StudentId,
                Vrijednost = add.Vrijednost,
                OpisnaOcjena = add.UpisnaOcjena,
                DatumPolaganja = add.DatumPolaganja,
                Poeni = add.Poeni
            };
            db.Ocjene.Add(ocjena);
            db.SaveChanges();
            return new AddOcjenaResponse
            { Dodana = true, Greska = "" };
        }
        [HttpPut]
        public async Task<AddOcjenaResponse> updateOcjena([FromQuery]int id, [FromBody] UpdateOcjenaRequest req)
        {
            var prijava = await auth.getInfo();
            if (!prijava.JeLogiran || prijava.Prijava.Korisnik.isStudent) return new AddOcjenaResponse
            { Dodana = false, Greska = "Nemate pravo pristupa!" };
            var ocjena = await db.Ocjene.Include(o => o.Student).Where(
                o => o.Student.Obrisan != true).FirstOrDefaultAsync(o => o.Id == id);
            if (ocjena == null) return new AddOcjenaResponse
            { Dodana = false, Greska = "Ocjena ne postoji!" };
            if (prijava.Prijava.Korisnik.isNastavnik)
            {
                var predmeti = await db.Predmeti.Include(p => p.Nastavnik).Where(
                    p => p.NastavnikId == prijava.Prijava.KorisnikId).ToListAsync();
                if (predmeti.Find(p => p.Id == ocjena.PredmetId) == null) return new AddOcjenaResponse
                { Dodana = false, Greska = "Nemate pravo pristupa!" };
            }
            ocjena.Vrijednost = req.Vrijednost;
            ocjena.OpisnaOcjena = req.UpisnaOcjena;
            ocjena.DatumPolaganja = req.DatumPolaganja;
            ocjena.Poeni = req.Poeni;
            db.SaveChanges();
            return new AddOcjenaResponse
            { Dodana = true, Greska = "" };
        }
        [HttpDelete]
        public async Task<bool> deleteOcjena([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.JeLogiran || prijava.Prijava.Korisnik.isStudent) return false;
            var ocjena = await db.Ocjene.Include(o => o.Student).Where(
                o => o.Student.Obrisan != true).FirstOrDefaultAsync(o => o.Id == id);
            if (ocjena == null) return false;
            if (prijava.Prijava.Korisnik.isNastavnik)
            {
                var predmeti = await db.Predmeti.Include(p => p.Nastavnik).Where(
                    p => p.NastavnikId == prijava.Prijava.KorisnikId).ToListAsync();
                if (predmeti.Find(p => p.Id == ocjena.PredmetId) == null) return false;
            }
            db.Ocjene.Remove(ocjena);
            db.SaveChanges();
            return true;
        }
       

    }
    public class AddOcjenaRequest
    {
        public int PredmetId { get; set; }
        public int StudentId { get; set; }
        public int Vrijednost { get; set; }
        public string UpisnaOcjena { get; set; }
        public DateTime DatumPolaganja { get; set; }
        public float Poeni { get; set; }
    }
    public class UpdateOcjenaRequest
    {
        public int Vrijednost { get; set; }
        public string UpisnaOcjena { get; set; }
        public DateTime DatumPolaganja { get; set; }
        public float Poeni { get; set; }
    }
    public class AddOcjenaResponse
    {
        public bool Dodana { get; set; }
        public string Greska { get; set; }
    }
}
