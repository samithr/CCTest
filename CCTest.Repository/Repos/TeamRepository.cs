using CCTest.Common.DTO;
using CCTest.Database.Common;
using CCTest.Database.Entities;
using CCTest.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CCTest.Repository.Repos
{
    public class TeamRepository : ITeamRepository
    {
        private readonly CCTestDbContext _dbContext;
        private readonly IEntityMapper _mapper;

        #region Contructor
        public TeamRepository(CCTestDbContext dbContext,
                              IEntityMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        public async Task<List<TeamDto>> GetTeams()
        {
            try
            {
                var teamsList = await _dbContext.Teams.ToListAsync();
                return _mapper.Map<List<Team>, List<TeamDto>>(teamsList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}