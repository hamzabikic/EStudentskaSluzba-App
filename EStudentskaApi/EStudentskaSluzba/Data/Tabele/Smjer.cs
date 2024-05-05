using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Smjerovi")]
    public class Smjer
    {
        [Key]
        public int Id { get; set; }
        public string Opis { get; set; }
    }
}
