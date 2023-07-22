using CCTest.Database.Common;

namespace CCTest.Database.Entities
{
    public class Team : BaseEntity
    {
        public string? Name { get; set; }
        public int ChatCapacity { get; set; }
        public int QueueSize { get; set; }
    }
}
