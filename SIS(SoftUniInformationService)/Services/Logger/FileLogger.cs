namespace Services.Logger
{
    using System;
    using System.IO;
    using Contracts;

    public class FileLogger : ILogger
    {
        private static object LockObject = new object();

        private readonly string _fileName;

        public FileLogger(string fileName= "log.txt")
        {
            _fileName = fileName;
        }

        public void Log(string message)
        {
            lock (LockObject)
            {
                File.AppendAllText(this._fileName, message +DateTime.Now + Environment.NewLine);
            }
        }
    }
}
