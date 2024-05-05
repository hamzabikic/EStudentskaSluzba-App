using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStudentskaSluzba.Data.Tabele
{
    [Table("Studenti")]
    public class Student:Korisnik
    {
        public string BrojIndeksa { get; set; }
        public int GodinaStudija { get; set; }
        [ForeignKey(nameof(Smjer))]
        public int SmjerId { get; set; }
        [JsonIgnore]
        public Smjer Smjer { get; set; }
    }
}
