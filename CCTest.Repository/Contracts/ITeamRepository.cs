using CCTest.Database.Entities;

namespace CCTest.Repository.Contracts
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetTeamDetailsBySession(bool dayShift);
    }
}
