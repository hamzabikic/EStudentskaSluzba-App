namespace EStudentskaSluzba.Helpers
{
    public class GeneratorLozinke
    {
        public static string generisiLozinku()
        {
            var znakovi = "qwertzuiopasdfghjklyxcvbnm1234567890+!#$&/()=?*+";
            var lozinka = "";
            var random = new Random();
            for(int i =0; i<8; i++)
            {
                lozinka += znakovi[random.Next(0, znakovi.Length)];
            }
            return lozinka;
        }
    }
}
