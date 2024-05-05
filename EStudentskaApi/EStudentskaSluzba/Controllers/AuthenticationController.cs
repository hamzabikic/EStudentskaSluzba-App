using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Helpers;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController
    {
        private readonly MyDbContext db;
        private readonly IHttpContextAccessor context;
        private readonly AuthService auth;
        public AuthenticationController (MyDbContext _db, IHttpContextAccessor _context,
            AuthService _auth)
        {
            db = _db;
            context = _context;
            auth = _auth;
        }
        [HttpPost]
        public async Task<PrijavaResponse> prijava (PrijavaRequest request)
        {
            Korisnik korisnik;
            try
            {
                korisnik = await db.Korisnici.Where
                   (k => k.Obrisan == false &&
                   k.KorisnickoIme == request.Username && k.Lozinka == request.Password).FirstAsync();
            }
            catch (Exception ex)
            {
                return new PrijavaResponse
                {
                    Prijavljen = false,
                    Prijava = null
                };
            }
            
            var prijava = new Prijava
            {
                KorisnikId = korisnik.Id,
                DatumPrijave = DateTime.Now,
                IpAdresa = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
                Token = TokenGenerator.generisiToken()
            };
            db.Prijave.Add(prijava);
            db.SaveChanges();
            return new PrijavaResponse
            {
                Prijavljen = true,
                Prijava = prijava
            };
        }
        [HttpDelete]
        public async Task<bool> odjava ()
        {
            var obj = await auth.getInfo();
            if (!obj.JeLogiran) return false;
            var prijava = await db.Prijave.FindAsync(obj.Prijava.Id);
            db.Prijave.Remove(prijava);
            db.SaveChanges();
            return true;
        }

    }
    public class PrijavaRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class PrijavaResponse
    {
        public bool Prijavljen { get; set; }
        public Prijava Prijava { get; set; }
    }
   
}
