using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class OpstinaController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public OpstinaController(MyDbContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpGet]
        public async Task<List<Opstina>> getOpstine()
        {
            return await db.Opstine.Include(o => o.Drzava).ToListAsync();
        }
        [HttpPost]
        public async Task<bool> addOpstina([FromBody] OpstinaRequest opstinareq)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) return false;
            var nova = new Opstina
            {
                Naziv = opstinareq.Naziv,
                DrzavaId = opstinareq.DrzavaId
            };
            db.Opstine.Add(nova);
            db.SaveChanges();
            return true;
        }
        [HttpDelete]
        public async Task<bool> deleteOpstina([FromQuery] int opstinaid)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) return false;
            var opstina = await db.Opstine.FindAsync(opstinaid);
            if (opstina == null) return false;
            db.Opstine.Remove(opstina);
            db.SaveChanges();
            return true;
        }
        [HttpPost]
        public async Task<bool> addDrzava([FromBody] DrzavaRequest drzavareq)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) return false;
            var nova = new Drzava
            {
                Naziv = drzavareq.Naziv
            };
            db.Drzave.Add(nova);
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public async Task<List<Drzava>> getDrzave()
        {
            return await db.Drzave.ToListAsync();
        }
        [HttpDelete]
        public async Task<bool> deleteDrzava([FromQuery] int drzavaid)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) return false;
            var drzava = await db.Drzave.FindAsync(drzavaid);
            if (drzava == null) return false;
            db.Drzave.Remove(drzava);
            db.SaveChanges();
            return true;
        }

    }
    public class OpstinaRequest
    {
        public string Naziv { get; set; }
        public int DrzavaId { get; set; }
    }
    public class DrzavaRequest
    {
        public string Naziv { get; set; }
    }
}
