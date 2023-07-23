using CCTest.Common.DTO;
using CCTest.Common.Util;
using CCTest.Repository.Contracts;
using CCTest.Service.Contracts;
using Microsoft.Extensions.Configuration;

namespace CCTest.Service.Services
{
    public class AgentService : IAgentService
    {
        #region Services/Repos Injection
        private readonly IConfiguration _configuration;
        private readonly IAgentRepository _agentRepository;
        private readonly ITeamService _teamService;

        #endregion

        private short officeStartHour;
        private short officeEndHour;

        #region Constructor
        public AgentService(IConfiguration configuration,
                            IAgentRepository agentRepository,
                            ITeamService teamService)
        {
            _configuration = configuration;
            _agentRepository = agentRepository;
            _teamService = teamService;
        }

        #endregion

        #region Public methods

        public async Task<bool> AssignChatForAgent()
        {
            try
            {
                var teams = await _teamService.GetTeams();
                if (teams.Any())
                {
                    short.TryParse(_configuration["OfficeStartHour"], out officeStartHour);
                    short.TryParse(_configuration["OfficeEndHour"], out officeEndHour);
                    var dayTime = CommonCalculations.IsDayShift(officeStartHour, officeEndHour);
                    if (dayTime)
                    {
                        return await AssignAgent(teams.Where(o => !o.NightShift).ToList());
                    }
                    else
                    {
                        return await AssignAgent(teams.Where(o => o.NightShift).ToList());
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get response from agents to client for the incoming chat messages
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(string inputMessage)
        {
            if (inputMessage == null)
            {
                return string.Empty;
            }
            return await Task.FromResult("Sample response");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Assign agent from day shift to chat
        /// </summary>
        /// <param name="teamNames"></param>
        /// <returns></returns>
        private async Task<bool> AssignAgent(List<TeamDto> teams)
        {
            try
            {
                bool agentAssigned = false;
                if (await AssignJuniorAgent(teams, false))
                {
                    agentAssigned = true;
                }
                else
                {
                    if (await AssignMidLevelAgent(teams))
                    {
                        agentAssigned = true;
                    }
                    else
                    {
                        if (await AssignSeniorAgent(teams))
                        {
                            agentAssigned = true;
                        }
                        else
                        {
                            if (await AssignTeamLead(teams))
                            {
                                agentAssigned = true;
                            }
                        }
                    }
                }
                if (!agentAssigned)
                {
                    agentAssigned = await AssignOverflowTeamAgent(teams);
                }
                return agentAssigned;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assign an overflow team agent
        /// </summary>
        /// <param name="teamNames"></param>
        /// <returns></returns>
        private async Task<bool> AssignOverflowTeamAgent(List<TeamDto> teams)
        {
            return await AssignJuniorAgent(teams, true);
        }

        #endregion

        #region Assign agents

        /// <summary>
        /// Assign junior level agent to chat
        /// </summary>
        /// <param name="teams"></param>
        /// <param name="overflow"></param>
        /// <returns></returns>
        private async Task<bool> AssignJuniorAgent(List<TeamDto> teams, bool overflow)
        {
            try
            {
                if (overflow)
                {
                    var teamIds = teams.Where(o => o.Name.Equals("OverflowTeam")).Select(o => o.Id).ToList();
                    if (teamIds.Any())
                    {
                        var agents = await _agentRepository.GetAgentsByTeamIds(teamIds);
                        if (agents.Any() && agents.Any(o => o.CurrentChatCount < o.SeniorityFactor * 10))
                        {
                            var assigningAgent = agents.FirstOrDefault(o => o.CurrentChatCount < o.SeniorityFactor * 10);
                            await UpdateAgentChatCount(assigningAgent.Id);
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    if (await AssignATeamAgent(teams, 4))
                    {
                        return true;
                    }
                    else
                    {
                        return await AssignBTeamAgent(teams, 4);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Process assigning mid senior level agents
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private async Task<bool> AssignMidLevelAgent(List<TeamDto> teams)
        {
            if (await AssignATeamAgent(teams, 6))
            {
                return true;
            }
            else
            {
                return await AssignBTeamAgent(teams, 6);
            }
        }

        /// <summary>
        /// Process assigning senior level agents
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private async Task<bool> AssignSeniorAgent(List<TeamDto> teams)
        {
            return await AssignBTeamAgent(teams, 8);
        }

        /// <summary>
        /// Process assigning team lead level agents
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private async Task<bool> AssignTeamLead(List<TeamDto> teams)
        {
            return await AssignATeamAgent(teams, 5);
        }

        /// <summary>
        /// Assing agent from Team A based on the seniority
        /// </summary>
        /// <param name="teams"></param>
        /// <param name="seniorityFactor10"></param>
        /// <returns></returns>
        private async Task<bool> AssignATeamAgent(List<TeamDto> teams, int seniorityFactor10)
        {
            try
            {
                var teamIds = teams.Where(o => o.Name.Equals("Team_A")).Select(o => o.Id).ToList();
                if (teamIds.Any())
                {
                    var agents = await _agentRepository.GetAgentsByTeamIds(teamIds);
                    if (agents.Any() && agents.Any(o => o.CurrentChatCount < seniorityFactor10))
                    {
                        var assigningAgent = agents.FirstOrDefault(o => o.CurrentChatCount < seniorityFactor10);
                        if (assigningAgent != null)
                        {
                            await UpdateAgentChatCount(assigningAgent.Id);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assing agent from Team B based on the seniority
        /// </summary>
        /// <param name="teams"></param>
        /// <param name="seniorityFactor10"></param>
        /// <returns></returns>
        private async Task<bool> AssignBTeamAgent(List<TeamDto> teams, int seniorityFactor10)
        {
            try
            {
                var teamIds = teams.Where(o => o.Name.Equals("Team_B")).Select(o => o.Id).ToList();
                if (teamIds.Any())
                {
                    var agents = await _agentRepository.GetAgentsByTeamIds(teamIds);
                    if (agents.Any() && agents.Any(o => o.CurrentChatCount < seniorityFactor10))
                    {
                        var assigningAgent = agents.FirstOrDefault(o => o.CurrentChatCount < seniorityFactor10);
                        if (assigningAgent != null)
                        {
                            await UpdateAgentChatCount(assigningAgent.Id);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update agent with current chat count
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        private async Task<bool> UpdateAgentChatCount(Guid agentId)
        {
            try
            {
                return await _agentRepository.UpdateAgentChatCount(agentId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
