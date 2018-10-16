namespace Services.Logger
{
    using System;
    using System.IO;
    using Contracts;

    public class FileLogger : ILogger
    {
        private static object LockObject = new object();

        private readonly string _fileName;

        public FileLogger()
        {
            _fileName = "log.txt";
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
