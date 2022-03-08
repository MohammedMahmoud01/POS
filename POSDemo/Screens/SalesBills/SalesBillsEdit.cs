using POSDemo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSDemo.Models;

namespace POSDemo.Screens.SalesBills
{
    public partial class SalesBillsEdit : Form
    {
        POSEntities db = new POSEntities();
        int index, index2;

        TbItem item;
        TbSalesInvoiceItem salesInvoiceItem;
        public SalesBillsEdit()
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
                        if (item.Quantity <= 0)
                        {
                            MessageBox.Show("للاسف تم نفاذ المنتج");
                            return;
                        }
                        else
                        {
                            salesInvoiceItem = db.TbSalesInvoiceItems.FirstOrDefault(x => x.InvoiceItemId == index2);

                            salesInvoiceItem.Qty++;
                            item.Quantity--;
                            model.Total = salesInvoiceItem.Qty * salesInvoiceItem.SalesPrice;

                            db.SaveChanges();

                            MessageBox.Show("تم زيادة عدد المنتج");

                            ItemsData.DataSource = GetItems();

                            BillsData.DataSource = GetInvoices();
                        }                       
                    }
                    else
                    {
                        if (item.Quantity <= 0)
                        {
                            MessageBox.Show("للاسف تم نفاذ المنتج");
                            return;
                        }
                        else
                        {
                            salesInvoiceItem = new TbSalesInvoiceItem();

                            salesInvoiceItem.InvoiceId = Convert.ToInt32(txtBills.Text);

                            salesInvoiceItem.ItemId = item.ItemId;

                            salesInvoiceItem.Notes = txtNotes.Text;

                            salesInvoiceItem.Qty = 1;

                            salesInvoiceItem.SalesPrice = item.SalesPrice;

                            db.TbSalesInvoiceItems.Add(salesInvoiceItem);

                            db.SaveChanges();

                            MessageBox.Show("تم أضافة منتج جديد");

                            ItemsData.DataSource = GetItems();

                            BillsData.DataSource = GetInvoices();
                        }
                    }
                 
                }
                catch(Exception ex)
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

                    salesInvoiceItem.Notes = txtNotes.Text;

                    salesInvoiceItem.InvoiceId = Convert.ToInt32(txtBills.Text);

                    salesInvoiceItem.ItemId = Convert.ToInt32(txtItemId.Text);

                    if (item.Quantity < 1)
                    {
                        MessageBox.Show("للاسف المنتج غير موجود");
                        return;
                    }
                    else
                    {
                        salesInvoiceItem.Qty++;

                        TbItem temp = db.TbItems.FirstOrDefault(x => x.ItemId == salesInvoiceItem.ItemId);
                        temp.Quantity--;

                        model.Total = salesInvoiceItem.Qty * salesInvoiceItem.SalesPrice;

                        db.SaveChanges();

                        MessageBox.Show("تم تعديل الفاتورة بنجاح");

                        BillsData.DataSource = GetInvoices();

                        ItemsData.DataSource = GetItems();
                    }

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

                salesInvoiceItem = db.TbSalesInvoiceItems.FirstOrDefault(x => x.InvoiceItemId == index2);

                if (salesInvoiceItem == null)
                    return;
                txtBills.Text = salesInvoiceItem.InvoiceId.ToString();

                txtNotes.Text = salesInvoiceItem.Notes;

                txtQty.Text = salesInvoiceItem.Qty.ToString();

                txtItemId.Text = salesInvoiceItem.ItemId.ToString();



                TotalPrice.Text = GetTotalPrice(salesInvoiceItem.InvoiceId).ToString();

                txtBillNo.Text = salesInvoiceItem.InvoiceId.ToString();
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
                            Price = item.SalesPrice
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
                        join invoice in db.TbSalesInvoiceItems
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
                            Price = item.SalesPrice,
                            Total = invoice.Qty * invoice.SalesPrice
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
                    salesInvoiceItem.Notes = txtNotes.Text;

                    salesInvoiceItem.InvoiceId = Convert.ToInt32(txtBills.Text);

                    salesInvoiceItem.ItemId = Convert.ToInt32(txtItemId.Text);

                    var x = Convert.ToInt32(txtQty.Text);
                    var y = 0;
                    if (x > item.Quantity)
                    {
                        MessageBox.Show("للاسف تخطيت عدد المنتجات الموجود لدينا");
                        return;
                    }
                    else
                    {
                        if(x == salesInvoiceItem.Qty)
                        {
                            salesInvoiceItem.Qty = x;
                            db.SaveChanges();

                            BillsData.DataSource = GetInvoices();

                            ItemsData.DataSource = GetItems();
                        }
                        else
                        {
                            y = (Convert.ToInt32(txtQty.Text) - salesInvoiceItem.Qty);

                            if(y > 0)
                            {
                                salesInvoiceItem.Qty = x;

                                TbItem temp = db.TbItems.FirstOrDefault(z => z.ItemId == salesInvoiceItem.ItemId);
                                temp.Quantity = temp.Quantity - y;

                                model.Total = salesInvoiceItem.Qty * salesInvoiceItem.SalesPrice;

                                db.SaveChanges();

                                MessageBox.Show("تم تعديل الفاتورة بنجاح");

                                BillsData.DataSource = GetInvoices();
                                ItemsData.DataSource = GetItems();
                            }
                            else
                            {
                                y = y * -1;

                                salesInvoiceItem.Qty = x;

                                TbItem temp = db.TbItems.FirstOrDefault(z => z.ItemId == salesInvoiceItem.ItemId);
                                temp.Quantity = temp.Quantity + y;

                                model.Total = salesInvoiceItem.Qty * salesInvoiceItem.SalesPrice;

                                db.SaveChanges();

                                MessageBox.Show("تم تعديل الفاتورة بنجاح");

                                BillsData.DataSource = GetInvoices();
                                ItemsData.DataSource = GetItems();
                            }

                        }
                     
                    }

                }
            }
            catch { }
        }

        private InvoiceModel GetInvoiceModel(int index2)
        {
            var query = from item in db.TbItems
                        join invoice in db.TbSalesInvoiceItems
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
                            Price = item.SalesPrice,
                            Total = invoice.Qty * invoice.SalesPrice
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
                    var qty = salesInvoiceItem.Qty;

                    TbItem temp = db.TbItems.FirstOrDefault(x => x.ItemId == salesInvoiceItem.ItemId);

                    temp.Quantity = temp.Quantity - qty;

                    db.TbSalesInvoiceItems.Remove(salesInvoiceItem);

                    db.SaveChanges();

                    MessageBox.Show("تم الحذف بنجاح");

                    BillsData.DataSource = GetInvoices();
                }
            }
            catch { }
        }

        private List<InvoiceModel> TotalInvoice(int index2)
        {
            var query = from item in db.TbItems
                        join invoice in db.TbSalesInvoiceItems
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
                            Price = item.SalesPrice,
                            Total = invoice.Qty * invoice.SalesPrice
                        };

            return query.ToList();
        }

        private void Refresh(object sender, EventArgs e)
        {
            BillsData.DataSource = GetInvoices();
        }
    }
}
