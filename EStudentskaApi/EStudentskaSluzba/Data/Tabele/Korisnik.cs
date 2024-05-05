using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Korisnici")]
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        [ForeignKey(nameof(Opstina))]
        public int OpstinaId { get; set; }
        public Opstina Opstina { get; set; }
        public string KorisnickoIme { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Lozinka { get; set; }
        [JsonIgnore]
        public string Slika { get; set; }
        public bool IsReferent { get; set; }
        [JsonIgnore]
        public bool Obrisan { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public bool isStudent => (this as Student) != null ? true : false;
        public bool isNastavnik => (this as Nastavnik) != null ? true : false;

    }
}
