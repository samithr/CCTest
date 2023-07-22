namespace CCTest.Service.Contracts
{
    public interface ISessionService
    {
        Task<bool> CheckSessionAvailability(string userId);
        Task<bool> CheckOverflowAgents(string userId);
        Task<bool> UpdateSessionQueueDayShift(string userId, bool addSession);
        Task<bool> UpdateSessionQueueNightShift(string userId, bool addSession);
        Task<int> GetSessionQueueCount();
        Task<string> DequeueSession();
    }
}
