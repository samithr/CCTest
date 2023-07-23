namespace CCTest.Common.DTO
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int ChatCapacity { get; set; }
        public int QueueSize { get; set; }
        public bool OverFlow { get; set; }
        public bool NightShift { get; set; }
    }
}
