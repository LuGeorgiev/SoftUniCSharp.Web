namespace SIS.Http.Exceptions
{
    using System;
    using System.Net;

    public class InternalServerErrorException : Exception
    {
        public override string Message => "The Server has encountered an error.";
    }
}
