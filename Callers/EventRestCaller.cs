using RestSharp;

using RestApiWrapUtility.Core;
using RestApiWrapUtility.Helpers;

namespace RestApiWrapUtility.Callers
{
    public class EventRestCaller : BasicRestCaller
    {
        #region Private Constant Variables

        private const string UPLOAD_PICTURE_FIELDS = "['id']";

        #endregion

        #region Constructors

        public EventRestCaller(RestCallerParameters parameters) : base(parameters) { }

        #endregion

        #region API Calls

        public dynamic UploadPicture(int eventId, string pictureName, string pictureData, int timeout = Timeout.DEFAULT_TIMEOUT_MILLISECONDS, string fields = UPLOAD_PICTURE_FIELDS)
        {
            var request = new RestRequest("events/{eventId}/pictures", Method.POST);

            request.AddUrlSegment("eventId", eventId);

            var pictureJsonObject = new JsonObject { { "name", pictureName },
                                                     { "data", pictureData } };

            request.AddParameter("text/json", pictureJsonObject, ParameterType.RequestBody);

            request.AddHeader("fields", fields);

            return ExecuteWithAccessToken(request, timeout);
        }

        #endregion
    }
}
