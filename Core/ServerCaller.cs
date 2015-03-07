using System;
using log4net;
using System.Collections.Generic;

using RestApiWrapUtility.Callers;
using RestApiWrapUtility.Helpers;

namespace RestApiWrapUtility.Core
{
    /// <summary>
    /// This class implements a Server Caller that can be used from a client to request calls.
    /// </summary>
    public class ServerCaller
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ServerCaller));

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="ServerCaller"/> with <paramref name="serverCallerParameters"/>.
        /// </summary>
        ///
        /// <param name="serverCallerParameters">The <see cref="ServerCallerParameters"/>.</param>
        public ServerCaller(ServerCallerParameters serverCallerParameters)
        {
            if (serverCallerParameters == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => serverCallerParameters)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => serverCallerParameters)));
            }

            var restCallerParameters = new RestCallerParameters(this, serverCallerParameters.BaseServerUrl, serverCallerParameters.AccessToken, serverCallerParameters.Canal);

            Callers = new List<BasicRestCaller>();

            OAuth = new OAuthRestCaller(restCallerParameters);
            
            Event = new EventRestCaller(restCallerParameters);
            
            Customer = new CustomerRestCaller(restCallerParameters);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to set safely an OAuth Access Token to the <see cref="ServerCaller"/>.
        /// </summary>
        ///
        /// <param name="accessToken">The OAuth Access Token.</param>
        public void SetAccessToken(string accessToken)
        {
            foreach (var caller in Callers)
            {
                caller.Parameters.AccessToken.SetKey(accessToken);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The <see cref="OAuthRestCaller"/>.
        /// </summary>
        public OAuthRestCaller OAuth { get; private set; }

        /// <summary>
        /// The <see cref="EventRestCaller"/>.
        /// </summary>
        public EventRestCaller Event { get; private set; }

        /// <summary>
        /// The <see cref="CustomerRestCaller"/>.
        /// </summary>
        public CustomerRestCaller Customer { get; private set; }

        /// <summary>
        /// A collection with all the Rest Callers of the <see cref="ServerCaller"/>.
        /// </summary>
        public List<BasicRestCaller> Callers { get; private set; }

        #endregion
    }
}
