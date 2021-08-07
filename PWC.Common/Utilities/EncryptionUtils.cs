using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PWC.Common.Utilities
{
    static public class EncryptionUtils
    {
        #region Constants
        private const string INVALID_BASE_64_STRING_MSG = " : The encrypted activation code is not in base 64 string format!";
        #endregion

        #region Variables

        private static readonly byte[] salt = Encoding.ASCII.GetBytes("A3B^D #K9D!F JD$@ D0V1GK& DK@F2M");
        private static readonly string defaultEncryptionPassword = "#ALM@NH@L!";

        #endregion

        #region Methods

        public static string Encrypt(string textToEncrypt, string encryptionPassword)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Encrypt(string textToEncrypt)
        {
            var algorithm = GetAlgorithm(defaultEncryptionPassword);

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encryptedText, string encryptionPassword)
        {
            var algorithm = GetAlgorithm(encryptionPassword);
            string decryptedText = string.Empty;

            byte[] descryptedBytes = new byte[0];
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            {
                byte[] encryptedBytes = new byte[0];
                if (IsBase64(encryptedText))
                {
                    encryptedBytes = Convert.FromBase64String(encryptedText);
                    descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
                    decryptedText = Encoding.UTF8.GetString(descryptedBytes);
                }
                else
                {
                    decryptedText = string.Empty;
                    
                }
            }

            return decryptedText;
        }

        public static string Decrypt(string encryptedText)
        {
            var algorithm = GetAlgorithm(defaultEncryptionPassword);
            string decryptedText = string.Empty;

            byte[] descryptedBytes;
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            {
                byte[] encryptedBytes = new byte[0];
                if (IsBase64(encryptedText))
                {
                    encryptedBytes = Convert.FromBase64String(encryptedText);
                    descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
                    decryptedText = Encoding.UTF8.GetString(descryptedBytes);
                }
                else
                {
                    decryptedText = string.Empty;
                }
            }
            return decryptedText;
        }

        // Performs an in-memory encrypt/decrypt transformation on a byte array.
        private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            MemoryStream memory = new MemoryStream();
            using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                stream.Write(data, 0, data.Length);
            }
            return memory.ToArray();
        }

        // Defines a RijndaelManaged algorithm and sets its key and Initialization Vector (IV) 
        // values based on the encryptionPassword received.
        private static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(defaultEncryptionPassword, salt);

            // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }

        // To ensure if string parameter is a base 64 base string
        private static bool IsBase64(this string base64String)
        {
            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }

        #endregion
    }
}