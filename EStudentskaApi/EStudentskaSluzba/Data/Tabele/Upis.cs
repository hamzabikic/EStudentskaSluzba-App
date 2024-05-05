using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Upisi")]
    public class Upis
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey(nameof(Referent))]
        public int ReferentId { get; set; }
        public Korisnik Referent { get; set; }
        public DateTime DatumUpisa { get; set; }
        public DateTime? DatumOvjere { get; set; }
        public int UpisanaGodina { get; set; }
        public bool Obnova { get; set; }
    }
}
