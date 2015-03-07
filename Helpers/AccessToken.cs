using System;
using log4net;

namespace RestApiWrapUtility.Helpers
{
    /// <summary>
    /// This class is used for specifying an OAuth Access Token.
    /// </summary>
    public class AccessToken
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(AccessToken));

        #endregion

        #region Private Constant Variables

        private const string ACCESS_TOKEN_VALIDATION_MESSAGE = "The OAuth Access Token cannot be null or empty!";

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to set safely the value of <see cref="Key"/>.
        /// </summary>
        ///
        /// <param name="key">The <see cref="Key"/>.</param>
        public void SetKey(string key)
        {
            ValidateKey(key);

            Key = key;
        }

        /// <summary>
        /// This method is used for validating the value of <see cref="Key"/>.
        /// </summary>
        public void ValidateKey()
        {
            ValidateKey(Key);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// This method is used for validating the value of <paramref name="key"/>.
        /// </summary>
        ///
        /// <param name="key">The <see cref="Key"/>.</param>
        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                LOGGER.Error(ACCESS_TOKEN_VALIDATION_MESSAGE);

                throw new ArgumentException(ACCESS_TOKEN_VALIDATION_MESSAGE);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The OAuth Access Token.
        /// </summary>
        public string Key { get; private set; }

        #endregion
    }
}
