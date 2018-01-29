using System;

namespace Force.Ddd
{
    public class ExceptionFailure: Failure
    {
        public Exception Exception { get; }

        public ExceptionFailure(Exception exception):base(exception.Message)
        {
            Exception = exception;
        }
    }
}