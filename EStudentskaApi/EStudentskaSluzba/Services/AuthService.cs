using EStudentskaSluzba.Controllers;
using EStudentskaSluzba.Data;
using EStudentskaSluzba.Data.Tabele;
using Microsoft.EntityFrameworkCore;

namespace EStudentskaSluzba.Services
{
    public class AuthService
    {
        private readonly MyDbContext db;
        private readonly IHttpContextAccessor context;
        public AuthService(MyDbContext _db, IHttpContextAccessor _context)
        {
            db = _db;
            context = _context;
        }
        public async Task<PrijavaInfo> getInfo()
        {
            string token = context.HttpContext.Request.Headers["my-token"];
            var prijava = await db.Prijave.Include(p => p.Korisnik).FirstOrDefaultAsync(
                p => p.Token == token);
            if (prijava == null) return new PrijavaInfo
            {
                JeLogiran = false,
                Prijava = null
            };
            return new PrijavaInfo
            {
                JeLogiran = true,
                Prijava = prijava
            };
        }
    }
    public class PrijavaInfo
    {
        public bool JeLogiran { get; set; }
        public Prijava Prijava { get; set; }
    }
}
