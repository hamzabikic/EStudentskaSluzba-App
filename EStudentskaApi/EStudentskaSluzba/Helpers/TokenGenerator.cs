namespace EStudentskaSluzba.Helpers
{
    public class TokenGenerator
    {
        public static string generisiToken()
        {
            var znakovi = "qwertzuiopasdfghjklyxcvbnm1234567890+!#$&/()=?*+";
            var token = "";
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                token += znakovi[random.Next(0, znakovi.Length)];
            }
            return token;
        }
    }
}
