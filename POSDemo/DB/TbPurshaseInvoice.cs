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
    
    public partial class TbPurshaseInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TbPurshaseInvoice()
        {
            this.TbPurchaseInvoiceItems = new HashSet<TbPurchaseInvoiceItem>();
        }
    
        public int InvoiceId { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public string Notes { get; set; }
        public int UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TbPurchaseInvoiceItem> TbPurchaseInvoiceItems { get; set; }
        public virtual TbSupplier TbSupplier { get; set; }
        public virtual TbUser TbUser { get; set; }
    }
}
