
namespace SIS.Http.Exceptions
{
    using System;
    using System.Net;

    public class BadRequestException :Exception
    {
        public BadRequestException(string message)
        {
            this.exceptionMessage = message;
        }

        public string exceptionMessage { get; private set; }

        //Allows for a more descriptive message to be put when throwing an exception for easier error handling
        public override string Message => this.exceptionMessage;

        //"The Request was malformed or contains unsupported elements.";
    }
}
