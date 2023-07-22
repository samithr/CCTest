using CCTest.Service.Contracts;
using System;
using System.Threading.Tasks;

namespace CCTest.UnitTest.Services
{
    public class SessionServiceTest : ISessionService
    {
        public Task<bool> CheckOverflowAgents(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckSessionAvailability(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DequeueSession()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetSessionQueueCount()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSessionQueueDayShift(string userId, bool addSession)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSessionQueueNightShift(string userId, bool addSession)
        {
            throw new NotImplementedException();
        }
    }
}
