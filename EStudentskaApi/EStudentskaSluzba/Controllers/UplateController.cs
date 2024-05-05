using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Auth]
    public class UplateController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public UplateController(MyDbContext _db,AuthService _auth )
        {
            db = _db;
            auth = _auth;
        }
        [HttpGet]
        public async Task<List<Rata>> getRateById ([FromQuery] int studentId)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            return await db.Rate.Include(r=> r.Referent).Where(r=> r.StudentId == studentId).ToListAsync();
        }
        [HttpPost]
        public async Task<bool> addRata([FromBody] RataRequest rata)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            var nova = new Rata
            {
                Godina = rata.Godina,
                RedniBroj = rata.RedniBroj,
                Obnova = rata.Obnova,
                Datum = rata.Datum,
                StudentId = rata.StudentId,
                ReferentId = rata.ReferentId,
                Iznos = rata.Iznos
            };
            db.Rate.Add(nova);
            db.SaveChanges();
            return true;
        }
        [HttpDelete]
        public async Task<bool> deleteRata (int rataId)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            var rata = await db.Rate.FindAsync(rataId);
            if (rata == null) return false;
            db.Rate.Remove(rata);
            db.SaveChanges();
            return true;
        }

    }
    public class RataRequest
    {
        public int Godina { get; set; }
        public int RedniBroj { get; set; }
        public bool Obnova { get; set; }
        public DateTime Datum { get; set; }
        public int StudentId { get; set; }
        public int ReferentId { get; set; }
        public float Iznos { get; set; }
    }

}
