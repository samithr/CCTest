using CCTest.Service.Shared;

namespace CCTest.Service.Contracts
{
    public interface ISupportService
    {
        Task<ServiceResponse> InitiateSupportRequest(string userId);
    }
}
