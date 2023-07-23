namespace CCTest.Service.Contracts
{
    public interface IAgentService
    {
        Task<bool> AssignChatForAgent();
        Task<string> GetResponse(string inputMessage);
    }
}
