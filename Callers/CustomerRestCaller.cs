using RestSharp;

using RestApiWrapUtility.Core;
using RestApiWrapUtility.Helpers;

namespace RestApiWrapUtility.Callers
{
    public class CustomerRestCaller : BasicRestCaller
    {
        #region Private Constant Variables

        private const string GET_FIELDS = "['id', 'name']";

        #endregion

        #region Constructors

        public CustomerRestCaller(RestCallerParameters parameters) : base(parameters) { }

        #endregion

        #region API Calls

        public dynamic Get(int customerId, int accountId, int timeout = Timeout.DEFAULT_TIMEOUT_MILLISECONDS, string fields = GET_FIELDS)
        {
            var request = new RestRequest("customers/{customerId}", Method.GET);

            request.AddUrlSegment("customerId", customerId);

            request.AddParameter("accountId", accountId);

            request.AddHeader("fields", fields);

            return ExecuteWithAccessToken(request, timeout);
        }

        #endregion
    }
}
