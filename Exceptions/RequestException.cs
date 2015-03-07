using System;

namespace RestApiWrapUtility.Exceptions
{
    /// <summary>
    /// This is the exception class for the requests that have failed.
    /// </summary>
    public class RequestException : Exception
    {
        #region Constructors

        public RequestException(string message, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public RequestException(string message, int httpStatusCode, Exception exception) : base(message, exception)
        {
            HttpStatusCode = httpStatusCode;
        }

        #endregion

        #region Public Properties

        public int HttpStatusCode { get; private set; }

        #endregion
    }
}
