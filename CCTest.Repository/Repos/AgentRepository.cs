using CCTest.Common.DTO;
using CCTest.Database.Common;
using CCTest.Database.Entities;
using CCTest.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CCTest.Repository.Repos
{
    public class AgentRepository : IAgentRepository
    {
        private readonly CCTestDbContext _dbContext;
        private readonly IEntityMapper _mapper;

        #region Constructor
        public AgentRepository(CCTestDbContext dbContext,
                               IEntityMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Update current chat count for agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAgentChatCount(Guid agentId)
        {
            try
            {
                var agent = await _dbContext.Agents.FirstOrDefaultAsync(o => o.Id.Equals(agentId));
                if (agent == null && agent.CurrentChatCount < agent.SeniorityFactor * 10)
                {
                    agent.CurrentChatCount++;
                    _dbContext.Agents.Update(agent);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get agents by list of team Ids
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public async Task<List<AgentDto>> GetAgentsByTeamIds(List<Guid> teamIds)
        {
            try
            {
                var agentList = await _dbContext.Agents.Where(o => teamIds.Contains(o.TeamId)).ToListAsync();
                return _mapper.Map<List<Agent>, List<AgentDto>>(agentList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
