//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POSDemo.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class TbItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TbItem()
        {
            this.TbPurchaseInvoiceItems = new HashSet<TbPurchaseInvoiceItem>();
            this.TbSalesInvoiceItems = new HashSet<TbSalesInvoiceItem>();
        }
    
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal SalesPrice { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public decimal PurchasePrice { get; set; }
    
        public virtual TbCategory TbCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TbPurchaseInvoiceItem> TbPurchaseInvoiceItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TbSalesInvoiceItem> TbSalesInvoiceItems { get; set; }
    }
}
