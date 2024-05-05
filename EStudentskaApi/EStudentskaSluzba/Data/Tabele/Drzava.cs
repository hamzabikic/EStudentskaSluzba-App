using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Drzave")]
    public class Drzava
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
    }
}
