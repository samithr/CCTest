using CCTest.Common.RequestObjects;
using CCTest.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CCTest.API.SignalRHubs
{
    public class ChatManagementHub : Hub
    {
        private readonly IAgentService _agentService;
        private readonly ILogger<ChatManagementHub> _logger;

        public ChatManagementHub(IAgentService agentService,
                                 ILogger<ChatManagementHub> logger)
        {
            _agentService = agentService;
            _logger = logger;
        }

        public async Task Chat([FromBody] ChatRequest input)
        {
            try
            {
                if (input != null)
                {
                    var connectionId = Context.ConnectionId;
                    var agentResponse = await _agentService.GetResponse(input.Message);
                    await Clients.Client(connectionId).SendAsync("Chat response", agentResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            _logger.LogError(e.Message);
            await base.OnDisconnectedAsync(e);
        }
        /// connect with session service
        /// monitor the queue
        /// assign agents based on team and availability
        /// in offic hours, assign overflow team to chats when normal queue exceeds
    }
}
