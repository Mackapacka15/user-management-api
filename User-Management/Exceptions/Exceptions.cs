namespace User_Management.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("The requested resource was not found.") { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base("The request was invalid.") { }

        public BadRequestException(string message)
            : base(message) { }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
