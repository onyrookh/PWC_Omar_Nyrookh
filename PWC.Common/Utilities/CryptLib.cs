using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PWC.GlobalComponent.Utils
{
    /// <summary>
    /// This class uses AES 256 for encryption
    /// Features: <br/>
    /// 1. 256 bit AES encryption
    /// 2. Random IV generation. 
    /// 3. Provision for SHA256 hashing of key. 
    /// </summary>
    public class CryptLib
    {
        #region Variables

        private UTF8Encoding _Enc;
        private RijndaelManaged _Rcipher;
        private byte[] _Key, _Pwd, _IVBytes, _IV;

        private static readonly char[] _CharacterMatrixForRandomIVStringGeneration = {
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 
			'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
			'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
		};

        #endregion

        #region Constants

        const string SHARED_SECRET = "#ALM@NH@L!";
        const string DEFAULT_IV = "Fk672/TGs4Y26zQ=";

        #endregion

        #region Enums

        private enum EncryptMode { ENCRYPT, DECRYPT };

        #endregion

        #region Constructor

        public CryptLib()
        {
            _Enc = new UTF8Encoding();
            _Rcipher = new RijndaelManaged();
            _Rcipher.Mode = CipherMode.CBC;
            _Rcipher.Padding = PaddingMode.PKCS7;
            _Rcipher.KeySize = 256;
            _Rcipher.BlockSize = 128;
            _Key = new byte[32];
            _IV = new byte[_Rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
            _IVBytes = new byte[16];
        }

        #endregion

        #region Methods

        ///<summary>
        ///This function encrypts the plain text to cipher text using the defeult encryption password
        ///(shared secret).
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public string EncryptAsBase64String(string _plainText)
        {
            string key = CryptLib.getHashSha256(SHARED_SECRET, 32); //32 bytes = 256 bits
            String cypherText = EncryptAsBase64String(_plainText, key, DEFAULT_IV);

            return cypherText;
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the encryption password
        ///(shared secret) provided. You'll have to use the same key for decryption
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        /// <param name="_key">Encryption password (shared secret)</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public string EncryptAsBase64String(string _plainText, string encryptionPassword)
        {
            string key = CryptLib.getHashSha256(encryptionPassword, 32); //32 bytes = 256 bits
            String cypherText = EncryptAsBase64String(_plainText, key, DEFAULT_IV);

            return cypherText;
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the key
        ///provided. You'll have to use the same key for decryption
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        /// <param name="_key">Encryption Key. You'll have to use the same key for decryption</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public string EncryptAsBase64String(string _plainText, string _key, string _initVector)
        {
            return EncryptDecryptAsBase64String(_plainText, _key, EncryptMode.ENCRYPT, _initVector);
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the defeult encryption password
        ///(shared secret).
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public byte[] EncryptAsBytes(string _plainText)
        {
            string key = CryptLib.getHashSha256(SHARED_SECRET, 32); //32 bytes = 256 bits
            byte[] cypherText = EncryptAsBytes(_plainText, key, DEFAULT_IV);

            return cypherText;
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the defeult encryption password
        ///(shared secret).
        /// </summary>
        /// <param name="_plainBytes">Plain text to be encrypted</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public byte[] EncryptAsBytes(byte[] _plainBytes)
        {
            string key = CryptLib.getHashSha256(SHARED_SECRET, 32); //32 bytes = 256 bits
            byte[] cypherText = EncryptAsBytes(_plainBytes, key, DEFAULT_IV);

            return cypherText;
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the encryption password
        ///(shared secret) provided. You'll have to use the same key for decryption
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        /// <param name="_key">Encryption password (shared secret)</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public byte[] EncryptAsBytes(string _plainText, string encryptionPassword)
        {
            string key = CryptLib.getHashSha256(encryptionPassword, 32); //32 bytes = 256 bits
            byte[] cypherText = EncryptAsBytes(_plainText, key, DEFAULT_IV);

            return cypherText;
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the encryption password
        ///(shared secret) provided. You'll have to use the same key for decryption
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        /// <param name="_key">Encryption password (shared secret)</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public byte[] EncryptAsBytes(byte[] _plainBytes, string encryptionPassword)
        {
            string key = CryptLib.getHashSha256(encryptionPassword, 32); //32 bytes = 256 bits
            byte[] cypherText = EncryptAsBytes(_plainBytes, key, DEFAULT_IV);

            return cypherText;
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the key
        ///provided. You'll have to use the same key for decryption
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        /// <param name="_key">Encryption Key. You'll have to use the same key for decryption</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public byte[] EncryptAsBytes(string _plainText, string _key, string _initVector)
        {
            return EncryptDecryptAsBytes(_plainText, _key, EncryptMode.ENCRYPT, _initVector);
        }

        ///<summary>
        ///This function encrypts the plain text to cipher text using the key
        ///provided. You'll have to use the same key for decryption
        /// </summary>
        /// <param name="_plainText">Plain text to be encrypted</param>
        /// <param name="_key">Encryption Key. You'll have to use the same key for decryption</param>
        ///<returns> returns encrypted (cipher) text</returns>
        public byte[] EncryptAsBytes(byte[] _plainBytes, string _key, string _initVector)
        {
            return EncryptDecryptAsBytes(_plainBytes, _key, EncryptMode.ENCRYPT, _initVector);
        }

        /// <summary>
        ///  This funtion decrypts the encrypted text to plain text using the default encryption password
        /// (shared secret).
        /// </summary>
        /// <param name="_encryptedText">Encrypted/Cipher text to be decrypted</param>
        /// <returns>encrypted value</returns>
        public string DecryptAsBase64String(string _encryptedText)
        {
            string key = CryptLib.getHashSha256(SHARED_SECRET, 32); //32 bytes = 256 bits
            String cypherText = DecryptAsBase64String(_encryptedText, key, DEFAULT_IV);

            return cypherText;
        }

        /// <summary>
        ///  This funtion decrypts the encrypted text to plain text using the encryption password
        /// (shared secret) provided. You'll have to use the same key which you used during
        ///encryprtion
        /// </summary>
        /// <param name="_encryptedText">Encrypted/Cipher text to be decrypted</param>
        /// <param name="_key">Encryption key which you used during encryption</param>
        /// <returns>encrypted value</returns>
        public string DecryptAsBase64String(string _encryptedText, string encryptionPassword)
        {
            string key = CryptLib.getHashSha256(encryptionPassword, 32); //32 bytes = 256 bits
            String cypherText = DecryptAsBase64String(_encryptedText, key, DEFAULT_IV);

            return cypherText;
        }

        /// <summary>
        ///  This funtion decrypts the encrypted text to plain text using the key
        /// provided. You'll have to use the same key which you used during
        ///encryprtion
        /// </summary>
        /// <param name="_encryptedText">Encrypted/Cipher text to be decrypted</param>
        /// <param name="_key">Encryption key which you used during encryption</param>
        /// <returns>encrypted value</returns>
        public string DecryptAsBase64String(string _encryptedText, string _key, string _initVector)
        {
            return DecryptAsBase64String(_encryptedText, _key, EncryptMode.DECRYPT, _initVector);
        }

        /// <summary>
        ///  This funtion decrypts the encrypted text to plain text using the default encryption password
        /// (shared secret).
        /// </summary>
        /// <param name="_encryptedText">Encrypted/Cipher text to be decrypted</param>
        /// <returns>encrypted value</returns>
        public byte[] DecryptAsBytes(string _encryptedText)
        {
            string key = CryptLib.getHashSha256(SHARED_SECRET, 32); //32 bytes = 256 bits
            byte[] cypherText = DecryptAsBytes(_encryptedText, key, DEFAULT_IV);

            return cypherText;
        }

        /// <summary>
        ///  This funtion decrypts the encrypted text to plain text using the encryption password
        /// (shared secret) provided. You'll have to use the same key which you used during
        ///encryprtion
        /// </summary>
        /// <param name="_encryptedText">Encrypted/Cipher text to be decrypted</param>
        /// <param name="_key">Encryption key which you used during encryption</param>
        /// <returns>encrypted value</returns>
        public byte[] DecryptAsBytes(string _encryptedText, string encryptionPassword)
        {
            string key = CryptLib.getHashSha256(encryptionPassword, 32); //32 bytes = 256 bits
            byte[] cypherText = DecryptAsBytes(_encryptedText, key, DEFAULT_IV);

            return cypherText;
        }

        /// <summary>
        ///  This funtion decrypts the encrypted text to plain text using the key
        /// provided. You'll have to use the same key which you used during
        ///encryprtion
        /// </summary>
        /// <param name="_encryptedText">Encrypted/Cipher text to be decrypted</param>
        /// <param name="_key">Encryption key which you used during encryption</param>
        /// <returns>encrypted value</returns>
        public byte[] DecryptAsBytes(string _encryptedText, string _key, string _initVector)
        {
            return EncryptDecryptAsBytes(_encryptedText, _key, EncryptMode.DECRYPT, _initVector);
        }

        ///<summary>
        /// Text encryption/decryption
        ///</summary>
        ///<param name="_inputText">Text to be encrypted or decrypted</param>
        ///<param name="_encryptionKey">Encryption key to used for encryption / decryption</param>
        ///<param name="_mode">specify the mode encryption / decryption</param>
        ///<param name="_initVector"> initialization vector</param>
        ///<returns>encrypted or decrypted string based on the mode</returns>
        private String EncryptDecryptAsBase64String(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
        {
            string _out = "";// output string
            byte[] plainText = EncryptDecryptAsBytes(_inputText, _encryptionKey, _mode, _initVector);
            _out = _Enc.GetString(plainText);
            _out = Convert.ToBase64String(plainText);

            return _out;
        }

        private String DecryptAsBase64String(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
        {
            string _out = "";// output string
            byte[] plainText = EncryptDecryptAsBytes(System.Convert.FromBase64String(_inputText), _encryptionKey, _mode, _initVector);
            _out = _Enc.GetString(plainText);
            
            return _out;
        }

        ///<summary>
        /// Text encryption/decryption
        ///</summary>
        ///<param name="_inputText">Text to be encrypted or decrypted</param>
        ///<param name="_encryptionKey">Encryption key to used for encryption / decryption</param>
        ///<param name="_mode">specify the mode encryption / decryption</param>
        ///<param name="_initVector"> initialization vector</param>
        ///<returns>encrypted or decrypted string based on the mode</returns>
        private byte[] EncryptDecryptAsBytes(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
        {
            return EncryptDecryptAsBytes(_Enc.GetBytes(_inputText), _encryptionKey, _mode, _initVector);
        }

        ///<summary>
        /// Text encryption/decryption
        ///</summary>
        ///<param name="_inputBytes">Text to be encrypted or decrypted</param>
        ///<param name="_encryptionKey">Encryption key to used for encryption / decryption</param>
        ///<param name="_mode">specify the mode encryption / decryption</param>
        ///<param name="_initVector"> initialization vector</param>
        ///<returns>encrypted or decrypted string based on the mode</returns>
        private byte[] EncryptDecryptAsBytes(byte[] _inputBytes, string _encryptionKey, EncryptMode _mode, string _initVector)
        {
            byte[] _out = null;// output string
            //_encryptionKey = MD5Hash (_encryptionKey);
            _Pwd = Encoding.UTF8.GetBytes(_encryptionKey);
            _IVBytes = Encoding.UTF8.GetBytes(_initVector);

            int len = _Pwd.Length;
            if (len > _Key.Length)
            {
                len = _Key.Length;
            }
            int ivLenth = _IVBytes.Length;
            if (ivLenth > _IV.Length)
            {
                ivLenth = _IV.Length;
            }

            Array.Copy(_Pwd, _Key, len);
            Array.Copy(_IVBytes, _IV, ivLenth);
            _Rcipher.Key = _Key;
            _Rcipher.IV = _IV;

            if (_mode.Equals(EncryptMode.ENCRYPT))
            {
                //encrypt
                _out = _Rcipher.CreateEncryptor().TransformFinalBlock(_inputBytes, 0, _inputBytes.Length);

            }
            if (_mode.Equals(EncryptMode.DECRYPT))
            {
                //decrypt
                _out = _Rcipher.CreateDecryptor().TransformFinalBlock(_inputBytes, 0, _inputBytes.Length);

            }
            _Rcipher.Dispose();
            return _out;// return encrypted/decrypted string
        }

        #endregion

        #region Helpers

        ///<summary>
        ///This function generates random string of the given input length.
        ///</summary>
        internal static string GenerateRandomIV(int length)
        {
            char[] _iv = new char[length];
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            for (int i = 0; i < _iv.Length; i++)
            {
                int ptr = randomBytes[i] % _CharacterMatrixForRandomIVStringGeneration.Length;
                _iv[i] = _CharacterMatrixForRandomIVStringGeneration[ptr];
            }

            return new string(_iv);
        }

        public static string getHashSha256(string text, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x); //covert to hex string
            }
            if (length > hashString.Length)
                return hashString;
            else
                return hashString.Substring(0, length);
        }

        #endregion
    }
}
