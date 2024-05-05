using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Helpers;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class KorisnikController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public KorisnikController(AuthService _auth, MyDbContext _db)
        {
            db = _db;
            auth = _auth;
        }
        [HttpPatch]
        public async Task<bool> generisiNovuLozinku([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            var korisnik = await db.Korisnici.FindAsync(id);
            if (korisnik == null) return false;
            var lozinka= GeneratorLozinke.generisiLozinku();
            korisnik.Lozinka = lozinka;
            db.SaveChanges();
            await EmailService.SendEmailAsync(korisnik.Email, "Nova lozinka za EStudentskaSluzba app",
                $"Vaša nova lozinka za pristup EStudenskaSluzba aplikaciji je:{lozinka}");
            return true;
        }
        [HttpGet]
        public async Task<Korisnik> getReferentById([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if  (prijava.Prijava.Korisnik.Id != id) throw new Exception("Nemate pravo pristupa!");
            return await db.Korisnici.FindAsync(id);
        }
        [HttpPatch]
        public async Task<EditResponse> NovaLozinka([FromBody] LozinkaRequest req)
        {
            var prijava = await auth.getInfo();
            if (prijava.Prijava.Korisnik.Id != req.StudentId) return new EditResponse
            {
                Editovan = false,
                Greska = "Nemate pravo pristupa!"
            };
            var korisnik = await db.Korisnici.Where
                (s => s.Id == req.StudentId && s.Lozinka == req.StaraLozinka).FirstOrDefaultAsync();
            if (korisnik == null) return new EditResponse
            {
                Editovan = false,
                Greska = "Pogresno unesena trenutna lozinka!"
            };
            if (req.NovaLozinka.Length < 8) return new EditResponse
            {
                Editovan = false,
                Greska = "Lozinka mora sadrzavati minimalno 8 karaktera!"
            };
        korisnik.Lozinka = req.NovaLozinka;
            db.SaveChanges();
            return new EditResponse
            {
                Editovan = true,
                Greska = ""
            };
        }
        [HttpPatch]
        public async Task<EditResponse> profesorEdit([FromBody] ProfesorRequest req)
        {
            var prijava = await auth.getInfo();
            if (prijava.Prijava.Korisnik.Id != req.ProfesorId) return new EditResponse
            {
                Editovan = false,
                Greska = "Nemate pravo pristupa!"
            };
            var profesor = await db.Nastavnici.Where
                (s => s.Id == req.ProfesorId).FirstOrDefaultAsync();
            if (profesor == null) return new EditResponse
            {
                Editovan = false,
                Greska = "Ne postoji korisnik!"
            };
            if (req.Email != profesor.Email && await db.Korisnici.Where(n => n.Email == req.Email).AnyAsync())
            {
                return new EditResponse
                {
                    Editovan = false,
                    Greska = "Email koji ste unijeli vec postoji!"
                };
            }
            profesor.Email = req.Email;
            db.SaveChanges();
            return new EditResponse
            {
                Editovan = true,
                Greska = ""
            };
        }
        [HttpPut]
        public async Task<EditResponse> referentEdit([FromBody] ReferentRequest req)
        {
            var prijava = await auth.getInfo();
            if (prijava.Prijava.Korisnik.Id != req.ReferentId) return new EditResponse
            {
                Editovan = false,
                Greska = "Nemate pravo pristupa!"
            };
            var referent = await db.Korisnici.Where
                (s => s.Id == req.ReferentId).FirstOrDefaultAsync();
            if (referent == null)  return new EditResponse
            {
                Editovan = false,
                Greska = "Ne postoji korisnik!"
            };
            if (referent.Email != req.Email && await db.Korisnici.Where(k => k.Email == req.Email).AnyAsync())
            {
                return new EditResponse
                {
                    Editovan = false,
                    Greska = "Unijeli ste email koji vec postoji!"
                };
            }
            referent.Ime = req.Ime;
            referent.Prezime = req.Prezime;
            referent.Email = req.Email;
            referent.DatumRodjenja = req.DatumRodjenja;
            referent.OpstinaId = req.OpstinaId;
            referent.KorisnickoIme = req.KorisnickoIme;
            db.SaveChanges();
            return new EditResponse
            {
                Editovan = true,
                Greska = ""
            };
        }
    }
    public class EditResponse {
        public bool Editovan { get; set; }
        public string Greska { get; set; }
    }
    public class LozinkaRequest
    {
        public int StudentId { get; set; }
        public string StaraLozinka { get; set; }
        public string NovaLozinka { get; set; }
    }
    public class ProfesorRequest
    {
        public int ProfesorId { get; set; }
        public string Email { get; set; }

    }
    public class ReferentRequest
    {
        public int ReferentId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int OpstinaId { get; set; }
        public string KorisnickoIme { get; set; }
        public string Email { get; set; }
    }


}
