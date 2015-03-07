using RestSharp;

using RestApiWrapUtility.Core;
using RestApiWrapUtility.Models;
using RestApiWrapUtility.Helpers;

namespace RestApiWrapUtility.Callers
{
    public class OAuthRestCaller : BasicRestCaller
    {
        #region Constructors

        public OAuthRestCaller(RestCallerParameters parameters) : base(parameters) { }

        #endregion

        #region API Calls

        public string Login(string grantType, string scope, string clientId, string clientSecret, string username, string password, int timeout = Timeout.DEFAULT_TIMEOUT_MILLISECONDS)
        {
            var request = new RestRequest("oauth/token", Method.POST);

            request.AddParameter("grant_type", grantType);
            request.AddParameter("scope", scope);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            var response = ExecuteWithoutAccessToken<OAuth>(request, timeout);

            return response.AccessToken;
        }

        #endregion
    }
}
