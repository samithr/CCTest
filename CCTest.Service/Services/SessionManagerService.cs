using CCTest.Service.Contracts;

namespace CCTest.Service.Services
{
    public class SessionManagerService : ISessionManagerService
    {
        #region Services Injection
        private readonly ISessionService _sessionService;
        private readonly IAgentService _agentService;
        #endregion

        #region Constructor
        public SessionManagerService(ISessionService sessionService,
                                     IAgentService agentService)
        {
            _sessionService = sessionService;
            _agentService = agentService;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Monitor session queueu and process sessions by assigning agents
        /// </summary>
        /// <returns></returns>
        public async Task MonitorAndProcessChatQueue()
        {
            try
            {
                var sessionCount = await _sessionService.GetSessionQueueCount();
                if (sessionCount > 0)
                {
                    if (await AssignAgent())
                    {
                        var user = await _sessionService.DequeueSession();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Assign agent for a session
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<bool> AssignAgent()
        {
            try
            {
                return await _agentService.AssignChatForAgent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
