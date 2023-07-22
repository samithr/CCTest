using System.Net;

namespace CCTest.API.ApiResponses
{
    public class InternalServerErrorResponse : BaseApiResponse
    {
        public bool IsError { get; }
        public InternalServerErrorResponse(string message, object response) : base(System.Net.HttpStatusCode.InternalServerError, message, response)
        {
            IsError = true;
        }
    }
}