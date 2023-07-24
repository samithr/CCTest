using CCTest.Common.Util;
using CCTest.Service.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CCTest.Service.Services
{
    public class SessionService : ISessionService
    {
        private readonly IMemoryCache _sessionQueueMemory;
        private readonly IConfiguration _configuration;

        #region Properties
        /// <summary>
        /// These values calculated from the assessment description and store as hard coded values
        /// In order to make more flexible system, these values can store in database tables and use accordingly at run time
        /// </summary>
        private readonly short officeHoursQueueSize;
        private readonly short overflowTeamQueueSize;
        private readonly short nightShiftQueueSize;
        private readonly short officeStartHour;
        private readonly short officeEndHour;
        private Queue<string> sessionQueue = new();
        #endregion

        #region Constructor
        public SessionService(IMemoryCache sessionQueueMemory,
                              IConfiguration configuration)
        {
            _sessionQueueMemory = sessionQueueMemory;
            _configuration = configuration;

            if (!_sessionQueueMemory.TryGetValue("SessionQueue", out Queue<short> _))
            {
                _sessionQueueMemory.Set("SessionQueue", sessionQueueMemory);
            }
            sessionQueue = _sessionQueueMemory.Get("SessionQueue") as Queue<string>;

            short.TryParse(_configuration["OfficeHoursQueueSize"], out officeHoursQueueSize);
            short.TryParse(_configuration["OverflowTeamQueueSize"],out overflowTeamQueueSize);
            short.TryParse(_configuration["NightShiftQueueSize"], out nightShiftQueueSize);
            short.TryParse(_configuration["OfficeStartHour"], out officeStartHour);
            short.TryParse(_configuration["OfficeEndHour"], out officeEndHour);
        }
        #endregion


        #region Public Methods

        /// <summary>
        /// Check if session can hold more data
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckSessionAvailability(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }
                var dayTime = CommonCalculations.IsDayShift(officeStartHour, officeEndHour);
                if (sessionQueue != null && !sessionQueue.Contains(userId.ToString()) && ((sessionQueue.Count < officeHoursQueueSize) || sessionQueue.Count < (officeHoursQueueSize + overflowTeamQueueSize)))
                {
                    if (dayTime && (sessionQueue.Count < officeHoursQueueSize))
                    {
                        return await Task.FromResult(true);
                    }
                    else if (sessionQueue.Count < nightShiftQueueSize)
                    {
                        return await Task.FromResult(true);
                    }
                }
                return await Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check overflow team available for day shift
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckOverflowAgents(string userId)
        {
            try
            {
                var dayTime = (DateTime.UtcNow.Hour < 16 && DateTime.UtcNow.Hour > 8);
                if (dayTime && (sessionQueue.Count < officeHoursQueueSize + overflowTeamQueueSize))
                {
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// check if session can be added on day time
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addSession"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSessionQueueDayShift(string userId, bool addSession)
        {
            try
            {
                if (addSession && ((sessionQueue.Count < officeHoursQueueSize) || sessionQueue.Count < (officeHoursQueueSize + overflowTeamQueueSize)))
                {
                    sessionQueue.Enqueue(userId);
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// check if session can be added on night time
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addSession"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSessionQueueNightShift(string userId, bool addSession)
        {
            try
            {
                if (addSession && (sessionQueue.Count < nightShiftQueueSize))
                {
                    sessionQueue.Enqueue(userId);
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get session queue count
        /// </summary>
        /// <returns> session queue count</returns>
        public async Task<int> GetSessionQueueCount()
        {
            return await Task.FromResult(sessionQueue != null ? sessionQueue.Count : 0);
        }

        /// <summary>
        /// Dequeue session for processing
        /// </summary>
        /// <returns>User id in the queue</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> DequeueSession()
        {
            try
            {
                var sessionCount = sessionQueue.Count;
                if (sessionCount > 0)
                {
                    return await Task.FromResult(sessionQueue.Dequeue());
                }
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
