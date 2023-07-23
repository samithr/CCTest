using CCTest.Common.DTO;
using CCTest.Repository.Contracts;
using CCTest.Service.Contracts;

namespace CCTest.Service.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        #region Constructor
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        #endregion

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        public Task<List<TeamDto>> GetTeams()
        {
            try
            {
                return _teamRepository.GetTeams();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
