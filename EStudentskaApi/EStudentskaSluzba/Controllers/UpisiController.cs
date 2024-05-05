using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class UpisiController
    {
        private readonly MyDbContext db;
        private readonly AuthService authService;
       public UpisiController(MyDbContext _db, AuthService _authservice)
        {
            db = _db;
            authService = _authservice;
        }
        [HttpGet]
        public async Task<List<Upis>> getUpisiById ([FromQuery] int studentid)
        {
            var prijava = await authService.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            return await db.Upisi.Include(u=> u.Referent)
                .Where(u=> u.StudentId == studentid).ToListAsync();
        }
        [HttpPost]
        public async Task<AddUpisResponse> addUpis([FromBody] UpisRequest upis)
        {
            var prijava = await authService.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                return new AddUpisResponse { Upisan = false, Greska = "Nemate pravo pristupa!" };
            }
            var student = await db.Studenti.FindAsync(upis.StudentId);
            if (student == null) return new AddUpisResponse
            { Upisan = false, Greska = "Student ne postoji u sistemu!" };
            if(student.GodinaStudija > upis.UpisanaGodina)
            {
                return new AddUpisResponse
                {
                    Upisan = false,
                    Greska = "Unesena godina studija je manja od trenutne!"
                };
            }
            if (upis.Obnova == false)
            {
                if (await db.Upisi.Where
                    (u => u.StudentId == upis.StudentId && u.UpisanaGodina == upis.UpisanaGodina).AnyAsync())
                {
                    return new AddUpisResponse { Upisan = false, Greska = "Upis je vec zaveden u sistem" };
                }
            }
            var novi = new Upis
            {
                StudentId = upis.StudentId,
                ReferentId = upis.ReferentId,
                DatumUpisa = upis.DatumUpisa,
                UpisanaGodina = upis.UpisanaGodina,
                Obnova = upis.Obnova
            };
            db.Upisi.Add(novi);
            db.SaveChanges();
            student.GodinaStudija = upis.UpisanaGodina;
            db.SaveChanges();
            return new AddUpisResponse { Upisan = true, Greska = "" };
        }
        [HttpDelete]
        public async Task<bool> deleteUpis([FromQuery] int upisId)
        {
            var prijava = await authService.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            var upis = await db.Upisi.FindAsync(upisId);
            if (upis == null) return false;
            db.Upisi.Remove(upis);
            db.SaveChanges();
            return true;
        }
        [HttpPatch]
        public async Task<bool> ovjera([FromBody] OvjeraRequest ovjera)
        {
            var prijava = await authService.getInfo();
            if (!prijava.Prijava.Korisnik.IsReferent)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            var upis = await db.Upisi.FindAsync(ovjera.UpisId);
            if (upis == null) return false;
            upis.DatumOvjere = ovjera.DatumOvjere;
            db.SaveChanges();
            return true;
        }
    }
    public class UpisRequest
    {
        public int StudentId { get; set; }
        public int ReferentId { get; set; }
        public DateTime DatumUpisa { get; set; }
        public int UpisanaGodina { get; set; }
        public bool Obnova { get; set; }
    }
    public class OvjeraRequest
    {
        public int UpisId { get; set; }
        public DateTime DatumOvjere { get; set; }
    }
    public class AddUpisResponse
    {
        public bool Upisan { get; set; }
        public string Greska { get; set; }
    }
}
