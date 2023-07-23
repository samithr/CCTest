using CCTest.Service.Contracts;
using CCTest.Service.Shared;

namespace CCTest.Service.Services
{
    public class SupportService : ISupportService
    {
        #region Services Injection
        private readonly IAgentService _agentService;
        private readonly ISessionService _sessionService;
        #endregion

        #region Constructor
        public SupportService(IAgentService agentService,
                              ISessionService sessionService)
        {
            _agentService = agentService;
            _sessionService = sessionService;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Initiate support request by updating the session queue
        /// If limit for the queue is exceeded, NOK will sent to the client
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse> InitiateSupportRequest(string userId)
        {
            try
            {
                var sessionAvailable = await CheckSessionAvailability(userId);
                if (sessionAvailable && await UpdateSessionQueueu(userId))
                {
                    return await SendOkResponse();
                }
                else
                {
                    var overflowTeamAvailable = await CheckOverflowTeamAvailablity(userId);
                    if (overflowTeamAvailable && await UpdateSessionQueueu(userId))
                    {
                        return await SendOkResponse();
                    }
                }
                return new ServiceResponse(ApiResponseCodes.Success, false, "No agents available at the moment", ChatInitiateResponse.NOk.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check session queue
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> CheckSessionAvailability(string userId)
        {
            try
            {
                return await _sessionService.CheckSessionAvailability(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check overflow team availability to assign additional requests
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckOverflowTeamAvailablity(string userId)
        {
            try
            {
                return await _sessionService.CheckOverflowAgents(userId);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Send ok response to client when service request is initiate
        /// </summary>
        /// <returns></returns>
        private async Task<ServiceResponse> SendOkResponse()
        {
            try
            {
                return await Task.FromResult(new ServiceResponse(ApiResponseCodes.Success, false, ChatInitiateResponse.Ok.ToString(), ChatInitiateResponse.Ok.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add new item to the session queue
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> UpdateSessionQueueu(string userId)
        {
            var dayTime = (DateTime.UtcNow.Hour < 16 && DateTime.UtcNow.Hour > 8);
            if (dayTime)
            {
                return await _sessionService.UpdateSessionQueueDayShift(userId, true);
            }
            else
            {
                return await _sessionService.UpdateSessionQueueNightShift(userId, true);
            }
        }

        #endregion
    }
}
