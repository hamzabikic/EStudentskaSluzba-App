using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Nastavnici")]
    public class Nastavnik:Korisnik
    {
        public string Zvanje { get; set; }
        public DateTime DatumZaposlenja { get; set; }
    }
}
