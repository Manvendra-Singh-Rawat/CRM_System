using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Application.DTO
{
    public class CreateInvoiceDTO
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public int WorkId { get; set; }

        [Required]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        public string ClientPhone { get; set; } = string.Empty;
        [Required]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        public string ProjectName { get; set; } = string.Empty;
        [Required]
        public double ProjectCost {  get; set; }
    }
}
