using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class PredmetiController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public PredmetiController(MyDbContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpGet]
        public async Task<List<Predmet>> getPredmeti ()
        {
            var prijava = await auth.getInfo();
            if(prijava.Prijava.Korisnik.isStudent) 
                return new List<Predmet>();
            if(prijava.Prijava.Korisnik.isNastavnik)
            {
                return await 
                    db.Predmeti.Where(p => p.NastavnikId == prijava.Prijava.KorisnikId)
                    .ToListAsync();
            }
            return await db.Predmeti.Include(p=> p.Nastavnik).ToListAsync();
        }
        [HttpPost]
        public async Task<bool> addPredmet([FromBody] PredmetRequest predmet)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) return false;
                
            var novi = new Predmet
            {
                Naziv = predmet.Naziv,
                NastavnikId = predmet.NastavnikId,
                Ects = predmet.Ects,
                Semestar = predmet.Semestar
            };
            db.Predmeti.Add(novi);
            db.SaveChanges();
            return true;
        }
        [HttpDelete]
        public async Task<bool> deletePredmet([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) return false;
            var predmet = await db.Predmeti.FindAsync(id);
            if(predmet == null)
            {
                return false;
            }
            db.Predmeti.Remove(predmet);
            db.SaveChanges();
            return true;
        }
        
    }
    public class PredmetRequest
    {
        public string Naziv { get; set; }
        public int NastavnikId { get; set; }
        public int Ects { get; set; }
        public int Semestar { get; set; }
    }
}
