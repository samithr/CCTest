namespace CCTest.Service.Contracts
{
    public interface IAgentService
    {
        Task<bool> AssignChatForAgent(string userId);
        Task<string> GetResponse(string inputMessage);
    }
}
