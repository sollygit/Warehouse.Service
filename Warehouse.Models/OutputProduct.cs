using System;

namespace Warehouse.Models
{
    public class OutputProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CodeType { get; set; }
        public string Code { get; set; }
        public DateTime DateReceived { get; set; }
    }
}
