using CCTest.Common.DTO;
using CCTest.Database.Entities;

namespace CCTest.Repository.Contracts
{
    public interface ITeamRepository
    {
        Task<List<TeamDto>> GetTeams();
    }
}
