using CCTest.Database.Common;

namespace CCTest.Database.Entities
{
    public class Agent : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SeniorityLevel{ get; set; }
        public double SeniorityFactor { get; set; }
        public int CurrentChatCount { get; set; }
        public Guid TeamId { get; set; }

        public Team? Team { get; set; }
    }
}
