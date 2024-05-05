using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Ocjene")]
    public class Ocjena
    {
        public int Id { get; set; }
        public int Vrijednost { get; set; }
        public string OpisnaOcjena { get; set; }
        public float Poeni { get; set; }
        public DateTime DatumPolaganja { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey(nameof(Predmet))]
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }
    }
}
