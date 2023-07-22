using CCTest.Common.RequestObjects;
using CCTest.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace CCTest.API.SignalRHubs
{
    public class ChatManagementHub : Hub
    {
        private readonly ISessionService _sessionService;
        private readonly IAgentService _agentService;
        private readonly ILogger<ChatManagementHub> _logger;
        private readonly IMemoryCache _chatQueueData;

        public ChatManagementHub(ISessionService sessionService,
                                 IAgentService agentService,
                                 ILogger<ChatManagementHub> logger,
                                 IMemoryCache chatQueueData)
        {
            _sessionService = sessionService;
            _agentService = agentService;
            _logger = logger;
            _chatQueueData = chatQueueData;
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
