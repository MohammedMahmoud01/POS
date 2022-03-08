using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSDemo.Models
{
    public class InvoiceModel
    {
        public int InvoiceItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int Qty { get; set; }
        public decimal Total { get; set; }
        public int ItemId { get; set; }

    }
}
