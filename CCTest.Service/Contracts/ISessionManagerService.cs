namespace CCTest.Service.Contracts
{
    public interface ISessionManagerService
    {
        Task MonitorAndProcessChatQueue();
    }
}
