using System;
using log4net;

namespace RestApiWrapUtility.Helpers
{
    /// <summary>
    /// This class is used for specifying a Base Server URL.
    /// </summary>
    public class BaseServerUrl
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(BaseServerUrl));

        #endregion

        #region Private Constant Variables

        private const string BASE_SERVER_URL_VALIDATION_MESSAGE = "The Base Server URL should be a valid absolute well formed URI!";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="BaseServerUrl"/> with <paramref name="url"/>.
        /// </summary>
        ///
        /// <param name="url">The <see cref="Url"/>.</param>
        public BaseServerUrl(string url)
        {
            SetUrl(url);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to set safely the value of <see cref="Url"/>.
        /// </summary>
        ///
        /// <param name="url">The <see cref="Url"/>.</param>
        public void SetUrl(string url)
        {
            ValidateUrl(url);

            Url = url;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// This method is used for validating the value of <paramref name="url"/>.
        /// </summary>
        ///
        /// <param name="url">The <see cref="Url"/>.</param>
        private static void ValidateUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                LOGGER.Error(BASE_SERVER_URL_VALIDATION_MESSAGE);

                throw new ArgumentException(BASE_SERVER_URL_VALIDATION_MESSAGE);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Base Server URL.
        /// </summary>
        public string Url { get; private set; }

        #endregion
    }
}
