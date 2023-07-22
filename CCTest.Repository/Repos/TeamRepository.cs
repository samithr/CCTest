using CCTest.Database.Common;
using CCTest.Database.Entities;
using CCTest.Repository.Contracts;

namespace CCTest.Repository.Repos
{
    public class TeamRepository : ITeamRepository
    {
        private readonly CCTestDbContext _dbContext;

        public TeamRepository(CCTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Team>> GetTeamDetailsBySession(bool dayShift)
        {
            throw new NotImplementedException();
        }
    }
}