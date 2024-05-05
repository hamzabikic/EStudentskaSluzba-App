using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Helpers;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class StudentController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public StudentController(MyDbContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpGet()]
        public async Task<List<Student>> getStudenti()
        {
            var prijava = await auth.getInfo();
            if (prijava.Prijava.Korisnik.isStudent) throw new Exception("Nemate pravo pristupa!");
            return await db.Studenti.Include(s => s.Smjer).Include(s => s.Opstina).Include(s => s.Opstina.Drzava).Where(
                s => s.Obrisan == false).OrderByDescending(s => s.DatumDodavanja).ToListAsync();
        }
        [HttpGet()]
        public async Task<List<Smjer>> getSmjerovi()
        {
            return await db.Smjerovi.ToListAsync();
        }
        [HttpPost()]
        public async Task<bool> addSmjer([FromBody] SmjerRequest add)
        {
            var prijava = await auth.getInfo();
            if(!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            var smjer = new Smjer { Opis = add.Naziv };
            db.Smjerovi.Add(smjer);
            db.SaveChanges();
            return true;
        }
        [HttpDelete()]
        public async Task<bool> deleteSmjer([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            var smjer = await db.Smjerovi.FindAsync(id);
            if (smjer == null) return false;
            db.Smjerovi.Remove(smjer);
            db.SaveChanges();
            return true;
        }

        [HttpPost()]
        public async Task<StudentAddResponse> addStudent(StudentAddRequest student)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            if(await db.Studenti.Where(s=> s.BrojIndeksa == student.BrojIndeksa).AnyAsync())
            {
                return new StudentAddResponse
                {
                    Id = 0,
                    Dodan = false,
                    Greska = "Unijeti broj indeksa vec postoji!"
                };
            }
            if (await db.Korisnici.Where(s => s.Email == student.Email).AnyAsync())
            {
                return new StudentAddResponse
                {
                    Id = 0,
                    Dodan = false,
                    Greska = "Unijeti email vec postoji!"
                };
            }
            if (await db.Korisnici.Where(s => s.KorisnickoIme == student.KorisnickoIme).AnyAsync())
            {
                int brojac = 1;
                var ki = student.KorisnickoIme + brojac.ToString();
                while(await db.Korisnici.Where(s => s.KorisnickoIme == ki).AnyAsync()) {
                    brojac++;
                    ki = student.KorisnickoIme + brojac.ToString();
                }
                student.KorisnickoIme = ki;
            }
            var novi = new Student
            {
                Ime = student.Ime,
                Prezime = student.Prezime,
                DatumRodjenja = student.DatumRodjenja,
                BrojIndeksa = student.BrojIndeksa,
                OpstinaId = student.OpstinaId,
                IsReferent = false,
                KorisnickoIme = student.KorisnickoIme,
                DatumDodavanja = DateTime.Now,
                Obrisan = false,
                Email = student.Email,
                GodinaStudija = student.GodinaStudija,
                Slika = "empty",
                SmjerId = student.SmjerId,
                Lozinka = GeneratorLozinke.generisiLozinku()
            };
            db.Studenti.Add(novi);
            db.SaveChanges();
            await EmailService.SendEmailAsync(novi.Email, "Pristupni podaci za EStudentskaSluzba app",
                $"Username: {novi.KorisnickoIme}\nPassword: {novi.Lozinka}");
            return new StudentAddResponse
            {
                Dodan = true,
                Id = novi.Id
            };
        }
        [HttpGet] 
        public async  Task<Student> getById([FromQuery]int id )
        {
            var prijava = await auth.getInfo();
            if (prijava.Prijava.Korisnik.isStudent && prijava.Prijava.Korisnik.Id !=id) throw new Exception("Nemate pravo pristupa!");
            return await db.Studenti.Include(s => s.Opstina).Include(s => s.Smjer).
                Where(s => s.Obrisan !=true && s.Id == id).FirstOrDefaultAsync();
        }
        [HttpPut]
        public async Task<StudentAddResponse> updateStudent ([FromBody] StudentAddRequest update,
            [FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            var student = await db.Studenti.Where(s => s.Obrisan != true).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return new StudentAddResponse
            { Id = 0, Dodan = false, Greska = "Ne postoji student!" };
            if (student.BrojIndeksa != update.BrojIndeksa && await db.Studenti.Where(s => s.BrojIndeksa == update.BrojIndeksa).AnyAsync())
            {
                return new StudentAddResponse
                { Id = id, Dodan = false, Greska = "Unijeti broj indeksa vec postoji!" };
            }
            if (student.Email != update.Email && await db.Korisnici.Where(s => s.Email == update.Email).AnyAsync())
            {
                return new StudentAddResponse
                { Id = id, Dodan = false, Greska = "Unijeti email vec postoji!" };
            }
            student.Ime = update.Ime;
            student.Prezime = update.Prezime;
            student.SmjerId = update.SmjerId;
            student.Email = update.Email;
            student.GodinaStudija = update.GodinaStudija;
            student.DatumRodjenja = update.DatumRodjenja;
            student.BrojIndeksa = update.BrojIndeksa;
            student.OpstinaId = update.OpstinaId;
            db.SaveChanges();
            return new StudentAddResponse { Dodan = true, Id = id, Greska = "" };
        }
        [HttpDelete]
        public async Task<bool> deleteStudent([FromQuery] int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent) throw new Exception("Nemate pravo pristupa!");
            var student = await db.Studenti.FindAsync(id);
            if (student == null) return false;
            db.Studenti.Remove(student);
            db.SaveChanges();
            return true;
        }
    }

    public class StudentAddResponse
    {
        public bool Dodan { get; set; }
        public int Id { get; set; }
        public string Greska { get; set; }

    }
    public class StudentAddRequest
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int OpstinaId { get; set; }
        public string KorisnickoIme { get; set; }
        public string Email { get; set; }
        public string BrojIndeksa { get; set; }
        public int GodinaStudija { get; set; }
        public int SmjerId { get; set; }

    }
    public class SmjerRequest
    {
        public string Naziv { get; set; }
    }
}
