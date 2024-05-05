using EStudentskaSluzba.Data;
using EStudentskaSluzba.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace EStudentskaSluzba.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ImageController :ControllerBase
    {
        private readonly MyDbContext db;
        private readonly AuthService auth;
        public ImageController(MyDbContext _db, AuthService _auth)
        {
            auth = _auth;
            db = _db;
        }
        [HttpGet]
        public async Task<FileContentResult> getImageById([FromQuery] int id)
        {
            var student = await db.Korisnici.FindAsync(id);
            if (student == null) return null;
            byte[] slika;
            try
            {
                slika = await System.IO.File.ReadAllBytesAsync(student.Slika);
            }
            catch(Exception ex)
            {
                return null;
            }
            return File(slika, "image/jpeg", "slika.jpg");

        }
        [HttpPost]
        public async Task<bool> dodajSliku (AddPhotoRequest photoreq)
        {
            var prijava = await auth.getInfo();
            if (!prijava.JeLogiran) throw new Exception("Nemate pravo pristupa!");
            if (!prijava.Prijava.Korisnik.IsReferent && photoreq.StudentId !=prijava.Prijava.KorisnikId)
                throw new Exception("Nemate pravo pristupa!");
            var student = await db.Korisnici.FindAsync(photoreq.StudentId);
            if (student == null) return false;
            byte[] photoblob;
            string base64string = photoreq.Base64string.Split(",")[1];
            try
            {

                photoblob = Convert.FromBase64String(base64string);
            }
            catch(Exception ex)
            {
                return false;
            }
            if(!Directory.Exists("student-profile-images"))
            {
                Directory.CreateDirectory("student-profile-images");
            }
            var putanja = $"student-profile-images/slika-{photoreq.StudentId}";
            await System.IO.File.WriteAllBytesAsync
                (putanja, photoblob);
            student.Slika = putanja;
            db.SaveChanges();
            return true;
        }
    }
    public class AddPhotoRequest
    {
        public int StudentId { get; set; }
        public string Base64string { get; set; }
    }
}
