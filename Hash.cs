using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Secure_passwords
{
    public class Hash
    {
        public static byte[] GenerateSalt()
        {
            int saltByteLength = 32;

            using (var RNG = new RNGCryptoServiceProvider())
            {
                var salt = new byte[saltByteLength];
                RNG.GetBytes(salt);

                return salt;
            }
        }
        //this is meant for combining the password with the salt
        private static byte[] Combine(byte[] first, byte[] second)
        {
            int tempi = 0;
            byte[] finalArray = new byte[first.Length + second.Length];
            for (int i = 0; i < first.Length; i++)
            {
                finalArray[i] = first[i];
            }

            for (int i = first.Length; i < second.Length; i++)
            {
                finalArray[i] = second[tempi];
                tempi++;
            }
            return finalArray;
        }
        public static string HashPasswordWithSalt(byte[] input, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Combine(input, salt));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
