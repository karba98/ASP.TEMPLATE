using System;
using System.Security.Cryptography;
using System.Text;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public class CypherService
    {
        public static string EncryptBasico(string contenido)
        {
            byte[] entrada;
            byte[] salida;
            UnicodeEncoding encoding = new UnicodeEncoding();
            SHA1Managed sha = new SHA1Managed();
            entrada = encoding.GetBytes(contenido);
            salida = sha.ComputeHash(entrada);
            string res = encoding.GetString(salida);
            return res;
        }
        public static string Encrypt(string contenido, int iteraciones, string salt)
        {
            string contenidoSalt = contenido + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenidoSalt);
            for (int i = 1; i <= iteraciones; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            string stringsalida = Encoding.UTF8.GetString(salida);
            return stringsalida;
        }
        public static string GetSalt()
        {
            Random r = new Random();
            string salt = "";
            for (int i = 1; i <= 200; i++)
            {
                int aleat = r.Next(0, 9999);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }
        public static byte[] Encrypt(string contenido, string salt)
        {
            string contenidoSalt = contenido + salt;
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
