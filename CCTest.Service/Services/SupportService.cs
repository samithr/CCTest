using CCTest.Common.Util;
using CCTest.Service.Contracts;
using CCTest.Service.Shared;
using Microsoft.Extensions.Configuration;

namespace CCTest.Service.Services
{
    public class SupportService : ISupportService
    {
        #region Services Injection
        private readonly IAgentService _agentService;
        private readonly ISessionService _sessionService;
        private readonly IConfiguration _configuration;
        #endregion

        private readonly short officeStartHour;
        private readonly short officeEndHour;

        #region Constructor
        public SupportService(IAgentService agentService,
                              ISessionService sessionService,
                              IConfiguration configuration)
        {
            _agentService = agentService;
            _sessionService = sessionService;
            short.TryParse(_configuration["OfficeStartHour"], out officeStartHour);
            short.TryParse(_configuration["OfficeEndHour"], out officeEndHour);
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
                if (sessionAvailable && await UpdateSessionQueue(userId))
                {
                    return await SendOkResponse();
                }
                else
                {
                    var overflowTeamAvailable = await CheckOverflowTeamAvailablity(userId);
                    if (overflowTeamAvailable && await UpdateSessionQueue(userId))
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
        private async Task<bool> UpdateSessionQueue(string userId)
        {
            var dayTime = CommonCalculations.IsDayShift(officeStartHour, officeEndHour);
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
