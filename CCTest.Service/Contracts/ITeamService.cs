using CCTest.Common.DTO;

namespace CCTest.Service.Contracts
{
    public interface ITeamService
    {
        Task<List<TeamDto>> GetTeams();
    }
}
