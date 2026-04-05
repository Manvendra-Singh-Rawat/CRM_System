namespace ClientManagement.Domain.Entity
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;

        public User User { get; set; }
    }
}
