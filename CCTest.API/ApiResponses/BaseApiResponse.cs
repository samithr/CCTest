using System.Net;

namespace CCTest.API.ApiResponses
{
    public class BaseApiResponse
    {
        public HttpStatusCode StatusCode { get; }
        public object Result { get; }
        public string Message { get; }

        public BaseApiResponse(HttpStatusCode statusCode, string message, object response)
        {
            Result = response;
            StatusCode = statusCode;
            Message = message;
        }
    }
}
