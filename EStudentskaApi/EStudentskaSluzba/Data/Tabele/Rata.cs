using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Rate")]
    public class Rata
    {
        public int Id { get; set; }
        public int Godina { get; set; }
        public int RedniBroj { get; set; }
        public bool Obnova { get; set; }
        public DateTime Datum { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey(nameof(Referent))]
        public int ReferentId { get; set; }
        public Korisnik Referent { get; set; }
        public float Iznos { get; set; }

    }
}
