using System.Buffers.Binary;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SagarImitation.Common.Helpers
{
    public class EncryptionDecryption
    {
        #region Variable Declaration

        /// <summary>
        /// key String
        /// </summary>
        private const string keyString = "EA34FF3E-JU84-1974-AW70-BB81D9564426";

        private const int _saltSize = 16; // 128 bits
        private const int _keySize = 32; // 256 bits
        private const int _iterations = 100000;
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

        private const char segmentDelimiter = ':';
        private static readonly byte[] salt = System.Text.Encoding.UTF8.GetBytes("9HsjsHhiQgwEK");

        private readonly byte[] Key;
        private readonly byte[] IV;

        #endregion

        #region Encrypt/Decrypt

        /// <summary>
        /// Get Encrypted Value of Passed value
        /// </summary>
        /// <param name="value">value to Encrypted</param>
        /// <returns>encrypted string</returns>
        public static string GetEncrypt(string value)
        {
            return EncryptWithAESCBC(keyString, value);
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Encrypt</param>
        /// <param name="strData">Message to Encrypt</param>
        /// <returns>encrypted string</returns>
        private static string EncryptWithAESCBC(string strKey, string strData)
        {
            string cipherText = "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(strData);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(strKey, salt, _iterations, _algorithm);
                encryptor.Key = pdb.GetBytes(_keySize);
                encryptor.IV = pdb.GetBytes(_saltSize);
                encryptor.Mode = CipherMode.CBC;
                encryptor.Padding = PaddingMode.PKCS7;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    cipherText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return cipherText;
        }

        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value">value to Decrypted</param>
        /// <returns>Decrypted string</returns>
        public static string GetDecrypt(string value)
        {
            return DecryptWithAESCBC(keyString, value);
        }

        /// <summary>
        /// decrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Decrypt</param>
        /// <param name="strData">Message to Decrypt</param>
        /// <returns>Decrypted string</returns>
        private static string DecryptWithAESCBC(string strKey, string strData)
        {
            string clearText = "";
            byte[] cipherBytes = Convert.FromBase64String(strData);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(strKey, salt, _iterations, _algorithm);
                encryptor.Key = pdb.GetBytes(_keySize);
                encryptor.IV = pdb.GetBytes(_saltSize);
                encryptor.Mode = CipherMode.CBC;
                encryptor.Padding = PaddingMode.PKCS7;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    clearText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                _iterations,
                _algorithm,
                _keySize
            );
            return string.Join(
                segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt)
            );
        }

        public static bool Verify(string input, string hashString, string saltString)
        {
            byte[] hash = Convert.FromHexString(hashString);
            byte[] salt = Convert.FromHexString(saltString);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                _iterations,
                _algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
        #endregion
    }
}