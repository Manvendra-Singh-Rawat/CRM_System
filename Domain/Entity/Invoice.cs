namespace ClientManagement.Domain.Entity
{
    public class Invoice
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string InvoiceURL { get; set; }
    }
}
