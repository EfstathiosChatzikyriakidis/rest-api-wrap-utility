using System;

namespace RestApiWrapUtility.Exceptions
{
    /// <summary>
    /// This is the exception class for the requests that have timed out.
    /// </summary>
    public class TimeoutException : Exception
    {
        #region Constructors

        public TimeoutException(string message) : base(message) {}

        public TimeoutException(string message, Exception exception) : base(message, exception) {}

        #endregion
    }
}
