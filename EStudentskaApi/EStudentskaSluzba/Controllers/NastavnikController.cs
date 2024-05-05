using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Helpers;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class NastavnikController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public NastavnikController(AuthService _auth, MyDbContext _db)
        {
            db = _db;
            auth = _auth;
        }
        [HttpGet]
        public async Task<List<Nastavnik>> getNastavnici ()
        {
            var prijava = await auth.getInfo();
            if(!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            return await db.Nastavnici.Include(n => n.Opstina).Include(n => n.Opstina.Drzava).ToListAsync();
        }
        [HttpPost]
        public async Task<AddNastavnikResponse> addNastavnik([FromBody] NastavnikRequest add)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            if(await db.Korisnici.Where(n=> n.Email == add.Email).AnyAsync())
            {
                return new AddNastavnikResponse { Dodan = false, Id = 0, Greska = "Unijeti Email vec postoji!" };
            }
            if(await db.Korisnici.Where(n=> n.KorisnickoIme == add.KorisnickoIme).AnyAsync())
            {
                int brojac = 1;
                var ki = add.KorisnickoIme + brojac.ToString();
                while(await db.Korisnici.Where(n=> n.KorisnickoIme == ki).AnyAsync())
                {
                    brojac++;
                    ki = add.KorisnickoIme + brojac.ToString();
                }
                add.KorisnickoIme = ki;
            }
            var nastavnik = new Nastavnik
            {
                Ime = add.Ime,
                Prezime = add.Prezime,
                KorisnickoIme = add.KorisnickoIme,
                DatumRodjenja = add.DatumRodjenja,
                DatumZaposlenja = add.DatumZaposlenja,
                Zvanje = add.Zvanje,
                Email = add.Email,
                OpstinaId = add.OpstinaId,
                DatumDodavanja = DateTime.Now,
                Lozinka = GeneratorLozinke.generisiLozinku(),
                Slika = "empty",
                IsReferent = false,
                Obrisan = false
            };
            db.Nastavnici.Add(nastavnik);
            db.SaveChanges();
            await EmailService.SendEmailAsync(nastavnik.Email, "Pristupni podaci za EStudentskaSluzba app",
                $"Username: {nastavnik.KorisnickoIme}\nPassword: {nastavnik.Lozinka}");
            return new AddNastavnikResponse
            {
                Dodan = true,
                Id = nastavnik.Id,
                Greska = ""
            };
        }
        [HttpGet]
        public async Task<Nastavnik> getNastavnikById ([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            return await db.Nastavnici.Include(n => n.Opstina).FirstOrDefaultAsync(n => n.Id == id);
        }
        [HttpPut]
        public async Task<AddNastavnikResponse> editNastavnik([FromQuery] int id, [FromBody] NastavnikRequest req)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            var nastavnik = await db.Nastavnici.FindAsync(id);
            if (nastavnik == null) return new AddNastavnikResponse
            { Dodan = false, Id = id, Greska = "Ne postoji nastavnik!" };
            if(req.Email != nastavnik.Email && await db.Korisnici.Where(n=> n.Email == req.Email).AnyAsync())
            {
                return new AddNastavnikResponse { Dodan = false, Id = id, Greska = "Unijeti email vec postoji!" };
            }
            nastavnik.Ime = req.Ime;
            nastavnik.Prezime = req.Prezime;
            nastavnik.Email = req.Email;
            nastavnik.DatumRodjenja = req.DatumRodjenja;
            nastavnik.DatumZaposlenja = req.DatumZaposlenja;
            nastavnik.Zvanje = req.Zvanje;
            nastavnik.OpstinaId = req.OpstinaId;
            db.SaveChanges();
            return new AddNastavnikResponse { Dodan = true, Id = id, Greska = "" };
        }
        [HttpDelete]
        public async Task<bool> deleteNastavnik([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            var nastavnik = await db.Nastavnici.FindAsync(id);
            if (nastavnik == null) return false;
            db.Nastavnici.Remove(nastavnik);
            db.SaveChanges();
            return true;
        }


    }
    public class NastavnikRequest
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int OpstinaId { get; set; }
        public string KorisnickoIme { get; set; }
        public string Email { get; set; }
        public DateTime DatumZaposlenja { get; set; }
        public string Zvanje { get; set; }
    }
    public class AddNastavnikResponse
    {
        public bool Dodan { get; set; }
        public int Id { get; set; }
        public string Greska { get; set; }
    }
}
