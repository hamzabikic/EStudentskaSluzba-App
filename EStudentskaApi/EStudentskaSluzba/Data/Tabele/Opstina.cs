using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Opstine")]
    public class Opstina
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
        [ForeignKey(nameof(Drzava)) ]
        public int DrzavaId { get; set; }
        public Drzava Drzava { get; set; }
    }
}
