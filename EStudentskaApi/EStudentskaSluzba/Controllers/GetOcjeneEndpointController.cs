using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetOcjeneEndpointController
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public GetOcjeneEndpointController(MyDbContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpGet]
        public async Task<List<Ocjena>> getOcjeneById (int id)
        {
            var prijava = await auth.getInfo();
            if (!prijava.JeLogiran) return new List<Ocjena>();
            if(prijava.Prijava.Korisnik.IsReferent || prijava.Prijava.Korisnik.isNastavnik ||
                prijava.Prijava.KorisnikId == id)
            {
                return await db.Ocjene.Include(o=> o.Student).Include(o=> o.Predmet)
                    .Include(o=>o.Predmet.Nastavnik).Where
                    (o=> o.Student.Id == id && o.Student.Obrisan!=true).ToListAsync();
            }
            
            return new List<Ocjena>() { };
        }
    }
}
