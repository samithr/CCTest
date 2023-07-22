using CCTest.Database.Common;
using CCTest.Repository.Contracts;

namespace CCTest.Repository.Repos
{
    public class AgentRepository : IAgentRepository
    {
        private readonly CCTestDbContext _dbContext;

        public AgentRepository(CCTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
