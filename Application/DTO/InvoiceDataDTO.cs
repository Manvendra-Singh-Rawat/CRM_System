namespace ClientManagement.Application.DTO
{
    public class InvoiceDataDTO
    {
        public byte[] InvoiceBytes;
        public string ProjectName { set; get; } = string.Empty;
        public string Email { set; get; } = string.Empty;
    }
}
