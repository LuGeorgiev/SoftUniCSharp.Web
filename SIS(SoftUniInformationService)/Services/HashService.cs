namespace Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Contracts;
    using Services.Logger.Contracts;

    public class HashService : IHashService
    {
        private readonly ILogger logger;

        public HashService(ILogger logger)
        {
            this.logger = logger;
        }
        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "MyAppSalt#";

            var crypt = new SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
           
            var hash = BitConverter.ToString(crypto).Replace("-", "").ToLower();

            this.logger.Log(hash);
            return hash;
        }
    }
}
