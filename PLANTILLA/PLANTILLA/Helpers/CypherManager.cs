using System;
using System.Security.Cryptography;
using System.Text;

namespace PLANTILLA.Helpers
{
    public class CypherManager
    {
        public static String EncryptBasico(String contenido)
        {
            byte[] entrada;
            byte[] salida;
            UnicodeEncoding encoding = new UnicodeEncoding();
            SHA1Managed sha = new SHA1Managed();
            entrada = encoding.GetBytes(contenido);
            salida = sha.ComputeHash(entrada);
            String res = encoding.GetString(salida);
            return res;
        }
        public static String Encrypt(String contenido, int iteraciones, String salt)
        {
            String contenidoSalt = contenido + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenidoSalt);
            for (int i = 1; i <= iteraciones; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            String stringsalida = Encoding.UTF8.GetString(salida);
            return stringsalida;
        }
        public static String GetSalt()
        {
            Random r = new Random();
            String salt = "";
            for (int i = 1; i <= 200; i++)
            {
                int aleat = r.Next(0, 9999);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }
        public static byte[] Encrypt(String contenido, String salt)
        {
            String contenidoSalt = contenido + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenidoSalt);
            for (int i = 1; i <= 19982; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;
        }
    }
}
