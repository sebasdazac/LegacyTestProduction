using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LegacyTest.Tools
{ 
    public class Crypto
    {
        byte[] _key;
        byte[] _iv;
        public void CriptoUtil()
        {
            _key = Encoding.ASCII.GetBytes("L3G4CY13ST1X3Y5Z");
            _iv = Encoding.ASCII.GetBytes("L3G4CY13ST1X3Y5Z");

        }

        public string Encrypt(string inputstring)
        {
            CriptoUtil();

            if (string.IsNullOrEmpty(inputstring))
            {
                throw new ArgumentNullException("inputstring", "la cadena a encriptar no puede ser nula");
            }



            byte[] cipheredtext;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(_key, _iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(inputstring);
                        }

                        cipheredtext = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(cipheredtext);
        }

        public string Decrypt(string cryptedString)
        {
            CriptoUtil();
            if (string.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("cryptedString", "la cadena a desencriptar no puede ser nula");
            }           

            string simpletext = String.Empty;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(_key, _iv);
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            simpletext = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return simpletext;
        }
    }
}




