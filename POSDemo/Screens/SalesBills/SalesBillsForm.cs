using POSDemo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSDemo.Models;

namespace POSDemo.Screens.SalesBills
{
    public partial class SalesBillsForm : Form
    {
        POSEntities db = new POSEntities();

        int index , index2;

        TbItem item;
        TbSalesInvoice ss;
        public SalesBillsForm()
        {
            InitializeComponent();
            boxCustomer.DataSource = db.TbCustomers.ToList();
            boxCustomer.ValueMember = "CustomerId";
            boxCustomer.DisplayMember = "CustomerName";

            ItemsData.DataSource = GetItems();

            BillsData.DataSource = db.TbSalesInvoices.ToList();
        }

        public List<ItemModel> GetItems()
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

        private void ItemsSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                index = int.Parse(ItemsData.CurrentRow.Cells[0].Value.ToString());

                item = db.TbItems.FirstOrDefault(x => x.ItemId == index);

                if (item != null)
                {
                    txtNotes.Text = item.Notes.ToString();

                    pictureBox1.ImageLocation = item.Image;
                }

            }
            catch
            {

            }
        }

        private void ItemsDoubleClick(object sender, EventArgs e)
        {
            TbSalesInvoice salesInvoice = new TbSalesInvoice();
            TbSalesInvoiceItem salesInvoiceItem = new TbSalesInvoiceItem();
            var r = MessageBox.Show("هل تريد أضافة فاتورة جديدة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                try
                {                 
                    if (boxCustomer.SelectedValue == null)
                    {
                        MessageBox.Show("رجاءأدخال الصنف بطريقة صحيحة");
                        return;
                    }
                    salesInvoice.CustomerId = Convert.ToInt32(boxCustomer.SelectedValue);

                    salesInvoice.InvoiceDate = dateTimePicker1.Value;

                    salesInvoice.Notes = txtNotes.Text;

                    salesInvoice.UserId = Login.UserId;

                    db.TbSalesInvoices.Add(salesInvoice);

                    db.SaveChanges();

                    MessageBox.Show("تم أضافة فاتورة جديدة بنجاح");

                    salesInvoiceItem.InvoiceId = salesInvoice.InvoiceId;

                    salesInvoiceItem.ItemId = item.ItemId;

                    salesInvoiceItem.Qty = 1;

                    salesInvoiceItem.SalesPrice = item.SalesPrice;

                    salesInvoiceItem.Notes = salesInvoice.Notes;

                    db.TbSalesInvoiceItems.Add(salesInvoiceItem);

                    item.Quantity--;

                    db.SaveChanges();

                    BillsData.DataSource = db.TbSalesInvoices.ToList();
                }
                catch{ }
            }
          
       
        }

        private void BillsDoubleClick(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد تعديل الفاتورة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    ss.CustomerId = Convert.ToInt32(boxCustomer.SelectedValue);

                    ss.InvoiceDate = dateTimePicker1.Value;

                    ss.Notes = txtNotes.Text;

                    ss.UserId = Login.UserId;

                    db.SaveChanges();

                    MessageBox.Show("تم تعديل الفاتورة بنجاح");

                    BillsData.DataSource = db.TbSalesInvoices.ToList();
                }
            }
            catch { }
        }

        private void Delete(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد حذف الفاتوة ؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    db.TbSalesInvoices.Remove(ss);

                    db.SaveChanges();

                    MessageBox.Show("تم الحذف بنجاح");
                    BillsData.DataSource = db.TbSalesInvoices.ToList();
                }
            }
            catch
            {
                MessageBox.Show("فشلت عملية البحث لتعارض في قاعدة البيانات");
                return;
            }
        }

        private void Filters(object sender, EventArgs e)
        {
            try
            {
                int idCat = Convert.ToInt32(boxCustomer.SelectedValue);
                BillsData.DataSource = db.TbSalesInvoices.Where(x => x.CustomerId == idCat).ToList();
            }
            catch
            {

            }    
        }

        private void NavToSalesBillsEdit(object sender, EventArgs e)
        {
            SalesBillsEdit s = new SalesBillsEdit();
            s.Show();
        }

        private void Refresh(object sender, EventArgs e)
        {
            BillsData.DataSource = db.TbSalesInvoices.ToList();
        }

        private void BillsSelection(object sender, EventArgs e)
        {
            try
            {
                index2 = int.Parse(BillsData.CurrentRow.Cells[0].Value.ToString());

                if (index2 <= 0)
                    return;

                ss = db.TbSalesInvoices.FirstOrDefault(x => x.InvoiceId == index2);

                if (ss == null)
                    return;
                txtBills.Text = ss.InvoiceId.ToString();

                txtNotes.Text = ss.Notes;

                dateTimePicker1.Value = ss.InvoiceDate;
            }
            catch { }
        }
    }
}
