using System;
using log4net;

using RestApiWrapUtility.Helpers;

namespace RestApiWrapUtility.Core
{
    /// <summary>
    /// This is a wrapper class which is used for passing any parameters needed in the <see cref="BasicRestCaller"/>.
    /// </summary>
    public class RestCallerParameters
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(RestCallerParameters));

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="RestCallerParameters"/> with <paramref name="serverCaller"/>, <paramref name="baseServerUrl"/>, <paramref name="accessToken"/> and <paramref name="canal"/>.
        /// </summary>
        ///
        /// <param name="serverCaller">The <see cref="ServerCaller"/>.</param>
        /// <param name="baseServerUrl">The <see cref="BaseServerUrl"/>.</param>
        /// <param name="accessToken">The <see cref="AccessToken"/>.</param>
        /// <param name="canal">The <see cref="Canal"/>.</param>
        public RestCallerParameters(ServerCaller serverCaller, BaseServerUrl baseServerUrl, AccessToken accessToken, Canal canal)
        {
            if (serverCaller == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => serverCaller)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => serverCaller)));
            }

            if (baseServerUrl == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => baseServerUrl)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => baseServerUrl)));
            }

            if (accessToken == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => accessToken)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => accessToken)));
            }

            if (canal == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => canal)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => canal)));
            }

            ServerCaller = serverCaller;

            BaseServerUrl = baseServerUrl;

            AccessToken = accessToken;

            Canal = canal;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The <see cref="ServerCaller"/>.
        /// </summary>
        public ServerCaller ServerCaller { get; private set; }

        /// <summary>
        /// The <see cref="BaseServerUrl"/>.
        /// </summary>
        public BaseServerUrl BaseServerUrl { get; private set; }

        /// <summary>
        /// The <see cref="AccessToken"/>.
        /// </summary>
        public AccessToken AccessToken { get; private set; }

        /// <summary>
        /// The <see cref="Canal"/>.
        /// </summary>
        public Canal Canal { get; private set; }

        #endregion
    }
}
