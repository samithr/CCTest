namespace CCTest.API.ApiResponses
{
    public class UnAuthorizedResponse : BaseApiResponse
    {
        public bool IsError { get; }
        public UnAuthorizedResponse(string message, object response) : base(System.Net.HttpStatusCode.Unauthorized, message, response)
        {
            IsError = true;
        }
    }
}
