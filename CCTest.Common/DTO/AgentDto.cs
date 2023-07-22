namespace CCTest.Common.DTO
{
    public class AgentDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SeniorityLevel { get; set; }
        public double SeniorityFactor { get; set; }
        public int CurrentChatCount { get; set; }
    }
}
