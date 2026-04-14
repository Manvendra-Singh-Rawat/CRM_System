namespace ClientManagement.Application.DTO
{
    public class GetWorkDTO
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDesc { get; set; } = string.Empty;
        public double Cost { get; set; }
        public int ClientId { get; set; }
    }
}
