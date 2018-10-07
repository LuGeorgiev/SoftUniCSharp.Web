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
            //var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            //foreach (byte theByte in crypto)
            //{
            //    hash.Append(theByte.ToString("x2"));
            //}
            var hash = BitConverter.ToString(crypto).Replace("-", "").ToLower();
            return hash;
            //return hash.ToString();
        }
    }
}
