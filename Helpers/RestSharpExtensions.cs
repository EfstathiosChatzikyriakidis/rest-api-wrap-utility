using System;
using log4net;
using RestSharp;
using System.Text;
using System.Collections.Generic;

namespace RestApiWrapUtility.Helpers
{
    /// <summary>
    /// This class extends RestSharp by adding new methods to <see cref="IRestClient"/>, <see cref="IRestRequest"/> and <see cref="IRestResponse"/>.
    /// </summary>
    public static class RestSharpExtensions
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(RestSharpExtensions));

        #endregion

        #region Public Static Methods

        #region IRestClient Extensions

        /// <summary>
        /// This method is used to log the information of the <paramref name="client"/> by using the <paramref name="logger"/>.
        /// </summary>
        ///
        /// <param name="client">The client.</param>
        /// <param name="logger">The logger.</param>
        public static void LogMe(this IRestClient client, ILog logger)
        {
            logger.Debug(GetClientReadyForLogging(client));
        }

        #endregion

        #region IRestRequest Extensions

        /// <summary>
        /// This method is used to log the information of the <paramref name="request"/> by using the <paramref name="logger"/>.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="logger">The logger.</param>
        public static void LogMe(this IRestRequest request, ILog logger)
        {
            logger.Debug(GetRequestReadyForLogging(request));
        }

        #endregion

        #region IRestResponse Extensions

        /// <summary>
        /// This method is used to log the information of the <paramref name="response"/> by using the <paramref name="logger"/>.
        /// </summary>
        ///
        /// <param name="response">The response.</param>
        /// <param name="logger">The logger.</param>
        public static void LogMe(this IRestResponse response, ILog logger)
        {
            logger.Debug(GetResponseReadyForLogging(response));
        }

        #endregion

        #endregion

        #region Private Static Methods

        /// <summary>
        /// This method is used for extracting information from <paramref name="client"/> ready for logging.
        /// </summary>
        ///
        /// <param name="client">The client.</param>
        /// 
        /// <returns>The information of the <paramref name="client"/> ready for logging.</returns>
        private static string GetClientReadyForLogging(IRestClient client)
        {
            if (client == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => client)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => client)));
            }

            var message = new StringBuilder();

            message.AppendLine().AppendLine("Client Information");

            message.AppendFormat("\tBase Server URL: <{0}>", client.BaseUrl).AppendLine()
                   .AppendFormat("\tTimeout: <{0}>", client.Timeout).AppendLine()
                   .AppendFormat("\tDefault Parameter(s): <{0}>", client.DefaultParameters.Count).AppendLine();

            message.Append(GetParametersReadyForLogging(client.DefaultParameters, 2));

            return message.ToString();
        }

        /// <summary>
        /// This method is used for extracting information from <paramref name="request"/> ready for logging.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// 
        /// <returns>The information of the <paramref name="request"/> ready for logging.</returns>
        private static string GetRequestReadyForLogging(IRestRequest request)
        {
            if (request == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => request)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => request)));
            }

            var message = new StringBuilder();

            message.AppendLine().AppendLine("Request Information");

            message.AppendFormat("\tMethod: <{0}>", request.Method).AppendLine()
                   .AppendFormat("\tFormat: <{0}>", request.RequestFormat).AppendLine()
                   .AppendFormat("\tResource: <{0}>", request.Resource).AppendLine()
                   .AppendFormat("\tParameter(s): <{0}>", request.Parameters.Count).AppendLine();

            message.Append(GetParametersReadyForLogging(request.Parameters, 2));

            return message.ToString();
        }

        /// <summary>
        /// This method is used for extracting information from <paramref name="response"/> ready for logging.
        /// </summary>
        ///
        /// <param name="response">The response.</param>
        /// 
        /// <returns>The information of the <paramref name="response"/> ready for logging.</returns>
        private static string GetResponseReadyForLogging(IRestResponse response)
        {
            if (response == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => response)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => response)));
            }

            var message = new StringBuilder();

            message.AppendLine().AppendLine("Response Information");

            message.AppendFormat("\tContentLength: <{0}>", response.ContentLength).AppendLine()
                   .AppendFormat("\tContentType: <{0}>", response.ContentType).AppendLine()
                   .AppendFormat("\tResponseStatus: <{0}>", response.ResponseStatus).AppendLine()
                   .AppendFormat("\tServer: <{0}>", response.Server).AppendLine()
                   .AppendFormat("\tStatusCode: <{0}>", response.StatusCode).AppendLine()
                   .AppendFormat("\tContent: <{0}>", response.Content).AppendLine()
                   .AppendFormat("\tHeader(s): <{0}>", response.Headers.Count).AppendLine();

            message.Append(GetParametersReadyForLogging(response.Headers, 2));

            return message.ToString();
        }

        /// <summary>
        /// This method is used for extracting information from <paramref name="parameters"/> ready for logging.
        /// </summary>
        ///
        /// <param name="parameters">An enumeration of <see cref="Parameter"/>s.</param>
        /// <param name="numberOfTabs">The indentation level of the information.</param>
        /// 
        /// <returns>The information of the <paramref name="parameters"/> ready for logging.</returns>
        private static string GetParametersReadyForLogging(IEnumerable<Parameter> parameters, int numberOfTabs)
        {
            if (parameters == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => parameters)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => parameters)));
            }

            if (numberOfTabs < 0)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NEGATIVE_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => numberOfTabs)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NEGATIVE_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => numberOfTabs)));
            }

            var tabsSequence = new string('\t', numberOfTabs);

            var parametersForLogging = new StringBuilder();

            foreach (var parameter in parameters)
            {
                parametersForLogging.AppendFormat("{0}[ Name: <{1}>, Type: <{2}>, Value: <{3}> ]", tabsSequence, parameter.Name, parameter.Type, parameter.Value).AppendLine();
            }

            return parametersForLogging.ToString();
        }

        #endregion
    }
}
