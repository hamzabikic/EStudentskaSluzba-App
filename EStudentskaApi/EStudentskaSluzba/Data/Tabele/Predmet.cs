using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Predmeti")]
    public class Predmet
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikId { get; set; }
        public Nastavnik Nastavnik { get; set; }
        public int Ects { get; set; }
        public int Semestar { get; set; }

    }
}
