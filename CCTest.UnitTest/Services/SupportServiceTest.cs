using CCTest.Service.Contracts;
using CCTest.Service.Shared;
using System.Threading.Tasks;

namespace CCTest.UnitTest.Services
{
    public class SupportServiceTest : ISupportService
    {
        public async Task<ServiceResponse> InitiateSupportRequest(string userId)
        {
            if (userId == "Ok")
            {
                return new ServiceResponse(ApiResponseCodes.Success, false, "Ok", "Ok");
            }
            return new ServiceResponse(ApiResponseCodes.Success, false, "NOk", "NOk");
        }
    }
}
