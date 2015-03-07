using System;
using log4net;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

using RestApiWrapUtility.Helpers;
using RestApiWrapUtility.Exceptions;

using TimeoutException = RestApiWrapUtility.Exceptions.TimeoutException;

namespace RestApiWrapUtility.Core
{
    /// <summary>
    /// This class is used as the base class for all Rest Callers.
    /// </summary>
    public class BasicRestCaller
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(BasicRestCaller));

        #endregion

        #region Private Constant Variables

        /// <summary>
        /// Response message for indicating that a request has timed out.
        /// </summary>
        private const string TIMEOUT_RESPONSE_MESSAGE = "Request to remote API call timed out.";

        /// <summary>
        /// Text pattern in order to recognize a timed out response.
        /// </summary>
        private const string TIMEOUT_RESPONSE_RECOGNITION_PATTERN = "timed out";

        /// <summary>
        /// Canal Id parameter name.
        /// </summary>
        private const string CANAL_ID_PARAMETER_NAME = "canalId";

        /// <summary>
        /// OAuth Access Token parameter name.
        /// </summary>
        private const string ACCESS_TOKEN_PARAMETER_NAME = "access_token";

        /// <summary>
        /// Message Format with Error Message and Status Code.
        /// </summary>
        private const string ERROR_MESSAGE_AND_STATUS_CODE_MESSAGE_FORMAT = "ErrorMessage: {0}, StatusCode: {1}.";

        /// <summary>
        /// Message Format with Content and Status Code.
        /// </summary>
        private const string CONTENT_AND_STATUS_CODE_MESSAGE_FORMAT = "Content: {0}, StatusCode: {1}.";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="BasicRestCaller"/> with <paramref name="parameters"/>.
        /// </summary>
        ///
        /// <param name="parameters">The <see cref="Parameters"/>.</param>
        public BasicRestCaller(RestCallerParameters parameters)
        {
            if (parameters == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => parameters)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => parameters)));
            }

            Parameters = parameters;

            Parameters.ServerCaller.Callers.Add(this);
        }

        #endregion

        #region Public Methods

        #region Strongly Typed Model

        /// <summary>
        /// This method executes the <paramref name="request"/> with <paramref name="timeout"/> and returns the response as a strongly typed model.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// 
        /// <returns>A strongly typed model specified by the caller.</returns>
        /// 
        /// <remarks>This method uses OAuth.</remarks>
        public T ExecuteWithAccessToken<T>(IRestRequest request, int timeout) where T : new()
        {
            return Execute<T>(request, timeout, true);
        }

        /// <summary>
        /// This method executes the <paramref name="request"/> with <paramref name="timeout"/> and returns the response as a strongly typed model.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// 
        /// <returns>A strongly typed model specified by the caller.</returns>
        /// 
        /// <remarks>This method does not use OAuth.</remarks>
        public T ExecuteWithoutAccessToken<T>(IRestRequest request, int timeout) where T : new()
        {
            return Execute<T>(request, timeout, false);
        }

        #endregion

        #region Runtime Dynamic Model

        /// <summary>
        /// This method executes the <paramref name="request"/> with <paramref name="timeout"/> and returns the response as a dynamic C# object.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// 
        /// <returns>A dynamic C# object.</returns>
        /// 
        /// <remarks>This method uses OAuth.</remarks>
        public dynamic ExecuteWithAccessToken(IRestRequest request, int timeout)
        {
            return Execute(request, timeout, true);
        }

        /// <summary>
        /// This method executes the <paramref name="request"/> with <paramref name="timeout"/> and returns the response as a dynamic C# object.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// 
        /// <returns>A dynamic C# object.</returns>
        /// 
        /// <remarks>This method does not use OAuth.</remarks>
        public dynamic ExecuteWithoutAccessToken(IRestRequest request, int timeout)
        {
            return Execute(request, timeout, false);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// This method executes the <paramref name="request"/> with <paramref name="timeout"/> and returns the response as a strongly typed model.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="useAccessToken">Whether to use OAuth or not.</param>
        /// 
        /// <returns>A strongly typed model specified by the caller.</returns>
        private T Execute<T>(IRestRequest request, int timeout, bool useAccessToken) where T : new()
        {
            HandleRequest(request, useAccessToken);

            var client = new RestClient(Parameters.BaseServerUrl.Url);

            SetClientTimeout(client, timeout);

            var response = client.Execute<T>(request);

            HandleResponse(response);

            Logging(request, response, client);

            return response.Data;
        }

        /// <summary>
        /// This method executes the <paramref name="request"/> with <paramref name="timeout"/> and returns the response as a dynamic C# object.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="useAccessToken">Whether to use OAuth or not.</param>
        /// 
        /// <returns>A dynamic C# object.</returns>
        private dynamic Execute(IRestRequest request, int timeout, bool useAccessToken)
        {
            HandleRequest(request, useAccessToken);

            var client = new RestClient(Parameters.BaseServerUrl.Url);

            SetClientTimeout(client, timeout);

            var response = client.Execute(request);

            HandleResponse(response);

            Logging(request, response, client);

            return JsonConvert.DeserializeObject(response.Content);
        }

        /// <summary>
        /// This method handles the <paramref name="request"/>.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="useAccessToken">Whether to use OAuth or not.</param>
        private void HandleRequest(IRestRequest request, bool useAccessToken)
        {
            ValidateRequest(request);

            PrepareRequest(request, useAccessToken);
        }

        /// <summary>
        /// This method prepares the <paramref name="request"/> by adding to it appropriate parameters, headers.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="useAccessToken">Whether to use OAuth or not.</param>
        private void PrepareRequest(IRestRequest request, bool useAccessToken)
        {
            if (useAccessToken)
            {
                AddAccessTokenParameter(request);
            }

            AddBasicParameters(request);
        }

        /// <summary>
        /// This method adds in the <paramref name="request"/> some basic parameters, headers.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        private void AddBasicParameters(IRestRequest request)
        {
            request.AddHeader(CANAL_ID_PARAMETER_NAME, Parameters.Canal.Id.ToString());
        }

        /// <summary>
        /// This method adds in the <paramref name="request"/> the OAuth Access Token parameter.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        private void AddAccessTokenParameter(IRestRequest request)
        {
            Parameters.AccessToken.ValidateKey();

            ParameterType parameterType = GetParameterTypeDependingOnHttpMethod(request.Method);

            if (parameterType == ParameterType.UrlSegment)
            {
                request.Resource += ("?" + ACCESS_TOKEN_PARAMETER_NAME + "={" + ACCESS_TOKEN_PARAMETER_NAME + "}");
            }

            request.AddParameter(ACCESS_TOKEN_PARAMETER_NAME, Parameters.AccessToken.Key, parameterType);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// This method logs information related to <paramref name="request"/>, <paramref name="response"/> and <paramref name="client"/>.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="client">The client.</param>
        private static void Logging(IRestRequest request, IRestResponse response, IRestClient client)
        {
            client.LogMe(LOGGER);

            request.LogMe(LOGGER);

            response.LogMe(LOGGER);
        }

        /// <summary>
        /// This method validates the <paramref name="request"/>.
        /// </summary>
        ///
        /// <param name="request">The request.</param>
        private static void ValidateRequest(IRestRequest request)
        {
            if (request == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => request)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => request)));
            }
        }

        /// <summary>
        /// This method handles the <paramref name="response"/>.
        /// </summary>
        ///
        /// <param name="response">The response.</param>
        private static void HandleResponse(IRestResponse response)
        {
            ValidateResponse(response);

            CheckResponse(response);
        }

        /// <summary>
        /// This method validates the <paramref name="response"/>.
        /// </summary>
        ///
        /// <param name="response">The response.</param>
        private static void ValidateResponse(IRestResponse response)
        {
            if (response == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => response)));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, ObjectMetadata.GetMemberName(() => response)));
            }
        }

        /// <summary>
        /// This method checks the <paramref name="response"/> for any possible errors.
        /// </summary>
        ///
        /// <param name="response">The response.</param>
        private static void CheckResponse(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                LOGGER.Error(response.ErrorException);

                throw response.ErrorException;
            }

            var httpStatusCode = (int)response.StatusCode;

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.TimedOut)
                {
                    LOGGER.Error(TIMEOUT_RESPONSE_MESSAGE);

                    throw new TimeoutException(TIMEOUT_RESPONSE_MESSAGE);
                }

                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    if (!string.IsNullOrEmpty(response.ErrorMessage) && response.ErrorMessage.Contains(TIMEOUT_RESPONSE_RECOGNITION_PATTERN))
                    {
                        LOGGER.Error(TIMEOUT_RESPONSE_MESSAGE);

                        throw new TimeoutException(TIMEOUT_RESPONSE_MESSAGE);
                    }
                }

                LOGGER.ErrorFormat(ERROR_MESSAGE_AND_STATUS_CODE_MESSAGE_FORMAT, response.ErrorMessage, httpStatusCode);

                throw new RequestException(response.ErrorMessage, httpStatusCode);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                LOGGER.ErrorFormat(CONTENT_AND_STATUS_CODE_MESSAGE_FORMAT, response.Content, httpStatusCode);

                throw new RequestException(response.Content, httpStatusCode);
            }
        }

        /// <summary>
        /// This method sets the <paramref name="timeout"/> of the <paramref name="client"/>.
        /// </summary>
        ///
        /// <param name="client">The client.</param>
        /// <param name="timeout">The timeout.</param>
        private static void SetClientTimeout(IRestClient client, int timeout)
        {
            var timeoutObject = new Timeout(timeout);

            client.Timeout = timeoutObject.Milliseconds;
        }

        /// <summary>
        /// This method returns a <see cref="ParameterType"/> depending on the <paramref name="method"/>.
        /// </summary>
        ///
        /// <param name="method">The <see cref="Method"/>.</param>
        /// 
        /// <returns>A <see cref="ParameterType"/> depending on the <paramref name="method"/>.</returns>
        private static ParameterType GetParameterTypeDependingOnHttpMethod(Method method)
        {
            if (method.Equals(Method.POST) || method.Equals(Method.PUT))
            {
                return ParameterType.UrlSegment;
            }

            return ParameterType.GetOrPost;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The <see cref="RestCallerParameters"/>.
        /// </summary>
        public RestCallerParameters Parameters { get; private set; }

        #endregion
    }
}
