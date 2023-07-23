using CCTest.Common.DTO;

namespace CCTest.Repository.Contracts
{
    public interface IAgentRepository
    {
        Task<bool> UpdateAgentChatCount(Guid agentId);
        Task<List<AgentDto>> GetAgentsByTeamIds(List<Guid> teamIds);
    }
}
