namespace CCTest.API.ApiResponses
{
    public class OkResponse : BaseApiResponse
    {
        public bool IsError { get; }

        public OkResponse(string message, object response) : base(System.Net.HttpStatusCode.OK, message, response)
        {
            IsError = false;
        }
    }
}
