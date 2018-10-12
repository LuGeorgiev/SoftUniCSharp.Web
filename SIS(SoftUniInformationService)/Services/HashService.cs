namespace Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Contracts;

    public class HashService : IHashService
    {
        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "MyAppSalt#";

            var crypt = new SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
           
            var hash = BitConverter.ToString(crypto).Replace("-", "").ToLower();
            return hash;
        }
    }
}
