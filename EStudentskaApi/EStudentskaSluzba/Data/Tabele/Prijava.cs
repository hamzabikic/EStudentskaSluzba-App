using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Prijave")]
    public class Prijava
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey (nameof(Korisnik))]
        public int KorisnikId { get; set; }

        public Korisnik Korisnik { get; set; }
        public DateTime DatumPrijave { get; set; }
        public string IpAdresa { get; set; }
        public string Token { get; set; }


   }
}
