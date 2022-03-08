using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSDemo.DB;
using POSDemo.Models;

namespace POSDemo.Screens.PurchaseBills
{
    public partial class PurchaseBillsEdit : Form
    {
        POSEntities db = new POSEntities();
        int index, index2;

        TbItem item;
        TbPurchaseInvoiceItem PurchaseInvoiceItem;

        public PurchaseBillsEdit()
        {
            InitializeComponent();

            ItemsData.DataSource = GetItems();

            BillsData.DataSource = GetInvoices();
        }

        private void ItemsSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                index = int.Parse(ItemsData.CurrentRow.Cells[0].Value.ToString());

                item = db.TbItems.FirstOrDefault(x => x.ItemId == index);

                if (item != null)
                {
                    //txtNotes.Text = item.Notes.ToString();

                    pictureBox1.ImageLocation = item.Image;
                }

            }
            catch
            {

            }
        }

        private void ItemsDoubleClick(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد أضافة منتج جديد لنفس الفاتورة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                try
                {
                    InvoiceModel model = GetInvoiceModel(index2);

                    if (model.ItemId == item.ItemId)
                    {
                        PurchaseInvoiceItem = db.TbPurchaseInvoiceItems.FirstOrDefault(x => x.InvoiceItemId == index2);

                        PurchaseInvoiceItem.Qty++;
                        item.Quantity++;
                        model.Total = PurchaseInvoiceItem.Qty * PurchaseInvoiceItem.PurchasePrice;

                        db.SaveChanges();

                        MessageBox.Show("تم زيادة عدد المنتج");

                        ItemsData.DataSource = GetItems();

                        BillsData.DataSource = GetInvoices();
                        
                    }
                    else
                    {
                        PurchaseInvoiceItem = new TbPurchaseInvoiceItem();

                        PurchaseInvoiceItem.InvoiceId = Convert.ToInt32(txtBills.Text);

                        PurchaseInvoiceItem.ItemId = item.ItemId;

                        PurchaseInvoiceItem.Notes = txtNotes.Text;

                        PurchaseInvoiceItem.Qty = 1;
                            
                        PurchaseInvoiceItem.PurchasePrice = item.SalesPrice;

                        db.TbPurchaseInvoiceItems.Add(PurchaseInvoiceItem);

                        db.SaveChanges();

                        MessageBox.Show("تم أضافة منتج جديد");

                        ItemsData.DataSource = GetItems();

                        BillsData.DataSource = GetInvoices();
                        
                    }

                }
                catch
                {

                }
            }
            else
            {

            }


        }

        private void BillsDoubleClick(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد تعديل الفاتورة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    InvoiceModel model = GetInvoiceModel(index2);

                    PurchaseInvoiceItem.Notes = txtNotes.Text;

                    PurchaseInvoiceItem.InvoiceId = Convert.ToInt32(txtBills.Text);

                    PurchaseInvoiceItem.ItemId = Convert.ToInt32(txtItemId.Text);

                    PurchaseInvoiceItem.Qty++;

                    TbItem temp = db.TbItems.FirstOrDefault(x => x.ItemId == PurchaseInvoiceItem.ItemId);
                    temp.Quantity++;

                    model.Total = PurchaseInvoiceItem.Qty * PurchaseInvoiceItem.PurchasePrice;

                    db.SaveChanges();

                    MessageBox.Show("تم تعديل الفاتورة بنجاح");

                    BillsData.DataSource = GetInvoices();

                    ItemsData.DataSource = GetItems();
                }
            }
            catch { }
        }

        private void BillsSelection(object sender, EventArgs e)
        {
            try
            {
                index2 = int.Parse(BillsData.CurrentRow.Cells[0].Value.ToString());

                InvoiceModel model = GetInvoiceModel(index2);

                if (index2 <= 0)
                    return;

                PurchaseInvoiceItem = db.TbPurchaseInvoiceItems.FirstOrDefault(x => x.InvoiceItemId == index2);

                if (PurchaseInvoiceItem == null)
                    return;
                txtBills.Text = PurchaseInvoiceItem.InvoiceId.ToString();

                txtNotes.Text = PurchaseInvoiceItem.Notes;

                txtQty.Text = PurchaseInvoiceItem.Qty.ToString();

                txtItemId.Text = PurchaseInvoiceItem.ItemId.ToString();

                TotalPrice.Text = GetTotalPrice(PurchaseInvoiceItem.InvoiceId).ToString();

                txtBillNo.Text = PurchaseInvoiceItem.InvoiceId.ToString();
            }
            catch { }
        }

        private List<ItemModel> GetItems()
        {
            var query = from item in db.TbItems
                        join cat in db.TbCategories
                        on item.CategoryId equals cat.CategoryId
                        select new ItemModel
                        {
                            ItemId = item.ItemId,
                            ItemName = item.ItemName,
                            CategoryName = cat.CategoryName,
                            ItemCode = item.ItemCode,
                            Qty = item.Quantity,
                            Price = item.PurchasePrice
                        };
            return query.ToList();
        }

        private decimal GetTotalPrice(int index)
        {
            List<InvoiceModel> lstInvoices = TotalInvoice(index);

            decimal total = lstInvoices.Sum(x => x.Total);

            return total;
        }

        private List<InvoiceModel> GetInvoices()
        {
            var query = from item in db.TbItems
                        join invoice in db.TbPurchaseInvoiceItems
                        on item.ItemId equals invoice.ItemId
                        join cat in db.TbCategories
                        on item.CategoryId equals cat.CategoryId
                        select new InvoiceModel
                        {
                            InvoiceItemId = invoice.InvoiceItemId,
                            ItemName = item.ItemName,
                            CategoryName = cat.CategoryName,
                            ItemCode = item.ItemCode,
                            Qty = invoice.Qty,
                            Price = item.PurchasePrice,
                            Total = invoice.Qty * invoice.PurchasePrice
                        };

            return query.ToList();
        }

        private void EditBill(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد تعديل الفاتورة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    InvoiceModel model = GetInvoiceModel(index2);
                    PurchaseInvoiceItem.Notes = txtNotes.Text;

                    PurchaseInvoiceItem.InvoiceId = Convert.ToInt32(txtBills.Text);

                    PurchaseInvoiceItem.ItemId = Convert.ToInt32(txtItemId.Text);

                    var x = Convert.ToInt32(txtQty.Text);
                    var y = 0;

                    if (x == PurchaseInvoiceItem.Qty)
                    {
                        PurchaseInvoiceItem.Qty = x;
                        db.SaveChanges();

                        BillsData.DataSource = GetInvoices();

                        ItemsData.DataSource = GetItems();
                    }
                    else
                    {
                        y = (Convert.ToInt32(txtQty.Text) - PurchaseInvoiceItem.Qty);

                        if (y > 0)
                        {
                            PurchaseInvoiceItem.Qty = x;

                            TbItem temp = db.TbItems.FirstOrDefault(z => z.ItemId == PurchaseInvoiceItem.ItemId);
                            temp.Quantity = temp.Quantity + y;

                            model.Total = PurchaseInvoiceItem.Qty * PurchaseInvoiceItem.PurchasePrice;

                            db.SaveChanges();

                            MessageBox.Show("تم تعديل الفاتورة بنجاح");

                            BillsData.DataSource = GetInvoices();
                            ItemsData.DataSource = GetItems();
                        }
                        else
                        {
                            MessageBox.Show("لا يمكن تقليل الكمية");
                            return;
                        }

                    }

                }

                
            }
            catch { }
        }

        private InvoiceModel GetInvoiceModel(int index2)
        {
            var query = from item in db.TbItems
                        join invoice in db.TbPurchaseInvoiceItems
                        on item.ItemId equals invoice.ItemId
                        join cat in db.TbCategories
                        on item.CategoryId equals cat.CategoryId
                        where invoice.InvoiceItemId == index2
                        select new InvoiceModel
                        {
                            InvoiceItemId = invoice.InvoiceItemId,
                            ItemId = invoice.ItemId,
                            ItemName = item.ItemName,
                            CategoryName = cat.CategoryName,
                            ItemCode = item.ItemCode,
                            Qty = invoice.Qty,
                            Price = item.PurchasePrice,
                            Total = invoice.Qty * invoice.PurchasePrice
                        };

            return query.FirstOrDefault();
        }

        private void Delete(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد حذف الفاتوة ؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    var qty = PurchaseInvoiceItem.Qty;

                    TbItem temp = db.TbItems.FirstOrDefault(x => x.ItemId == PurchaseInvoiceItem.ItemId);

                    temp.Quantity = temp.Quantity - qty;

                    db.TbPurchaseInvoiceItems.Remove(PurchaseInvoiceItem);

                    db.SaveChanges();

                    MessageBox.Show("تم الحذف بنجاح");

                    BillsData.DataSource = GetInvoices();
                }
            }
            catch { }
        }

        private void Refresh(object sender, EventArgs e)
        {
            BillsData.DataSource = GetInvoices();
        }

        private List<InvoiceModel> TotalInvoice(int index2)
        {
            var query = from item in db.TbItems
                        join invoice in db.TbPurchaseInvoiceItems
                        on item.ItemId equals invoice.ItemId
                        join cat in db.TbCategories
                        on item.CategoryId equals cat.CategoryId
                        where invoice.InvoiceId == index2
                        select new InvoiceModel
                        {
                            InvoiceItemId = invoice.InvoiceItemId,
                            ItemId = invoice.ItemId,
                            ItemName = item.ItemName,
                            CategoryName = cat.CategoryName,
                            ItemCode = item.ItemCode,
                            Qty = invoice.Qty,
                            Price = item.PurchasePrice,
                            Total = invoice.Qty * invoice.PurchasePrice
                        };

            return query.ToList();
        }
    }
}
