using CCTest.Service.Contracts;

namespace CCTest.Service.Services
{
    public class AgentService : IAgentService
    {
        #region Services/Repos Injection
        #endregion

        #region Constructor
        public AgentService()
        {
            
        }
        #endregion

        #region Public methods

        public async Task<bool> AssignChatForAgent(string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get response from agets to client for the incoming chat messages
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(string inputMessage)
        {
            if (inputMessage == null)
            {
                return string.Empty;
            }
            return await Task.FromResult("Sample response");
        }

        #endregion
    }
}
