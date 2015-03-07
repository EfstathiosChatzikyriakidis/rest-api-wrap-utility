using System;
using log4net;

using RestApiWrapUtility.Helpers;

namespace RestApiWrapUtility.Core
{
    /// <summary>
    /// This is a wrapper class which is used for passing any parameters needed in the <see cref="ServerCaller"/>.
    /// </summary>
    public class ServerCallerParameters
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ServerCallerParameters));

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="ServerCallerParameters"/> with <paramref name="baseServerUrl"/> and <paramref name="canal"/>.
        /// </summary>
        ///
        /// <param name="baseServerUrl">The Base Server URL.</param>
        /// <param name="canal">The <see cref="Canal"/>.</param>
        public ServerCallerParameters(string baseServerUrl, Canal canal)
        {
            if (canal == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => canal)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => canal)));
            }

            BaseServerUrl = new BaseServerUrl(baseServerUrl);

            Canal = canal;

            AccessToken = new AccessToken();
        }

        /// <summary>
        /// Constructor method for creating <see cref="ServerCallerParameters"/> with <paramref name="baseServerUrl"/>, <paramref name="canal"/> and <paramref name="accessToken"/>.
        /// </summary>
        ///
        /// <param name="baseServerUrl">The Base Server URL.</param>
        /// <param name="canal">The <see cref="Canal"/>.</param>
        /// <param name="accessToken">The OAuth Access Token.</param>
        public ServerCallerParameters(string baseServerUrl, Canal canal, string accessToken) : this (baseServerUrl, canal)
        {
            AccessToken.SetKey(accessToken);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The <see cref="BaseServerUrl"/>.
        /// </summary>
        public BaseServerUrl BaseServerUrl { get; private set; }

        /// <summary>
        /// The <see cref="Canal"/>.
        /// </summary>
        public Canal Canal { get; private set; }

        /// <summary>
        /// The <see cref="AccessToken"/>.
        /// </summary>
        public AccessToken AccessToken { get; private set; }

        #endregion
    }
}
