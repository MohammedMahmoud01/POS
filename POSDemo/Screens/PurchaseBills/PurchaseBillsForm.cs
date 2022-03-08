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
    public partial class PurchaseBillsForm : Form
    {
        POSEntities db = new POSEntities();

        int index, index2;

        TbItem item;
        TbPurshaseInvoice ss;
        public PurchaseBillsForm()
        {
            InitializeComponent();
            boxSupplier.DataSource = db.TbSuppliers.ToList();
            boxSupplier.ValueMember = "SupplierId";
            boxSupplier.DisplayMember = "SupplierName";

            ItemsData.DataSource = GetItems();

            BillsData.DataSource = db.TbPurshaseInvoices.ToList();
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
                            Price = item.PurchasePrice
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
            TbPurshaseInvoice purchaseInvoice = new TbPurshaseInvoice();
            TbPurchaseInvoiceItem purchaseInvoiceItem = new TbPurchaseInvoiceItem();
            var r = MessageBox.Show("هل تريد أضافة فاتورة جديدة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                try
                {
                    if (boxSupplier.SelectedValue == null)
                    {
                        MessageBox.Show("رجاءأدخال الصنف بطريقة صحيحة");
                        return;
                    }
                    purchaseInvoice.SupplierId = Convert.ToInt32(boxSupplier.SelectedValue);

                    purchaseInvoice.InvoiceDate = dateTimePicker1.Value;

                    purchaseInvoice.Notes = txtNotes.Text;

                    purchaseInvoice.UserId = Login.UserId;

                    db.TbPurshaseInvoices.Add(purchaseInvoice);

                    db.SaveChanges();

                    MessageBox.Show("تم أضافة فاتورة جديدة بنجاح");

                    purchaseInvoiceItem.InvoiceId = purchaseInvoice.InvoiceId;

                    purchaseInvoiceItem.ItemId = item.ItemId;

                    purchaseInvoiceItem.Qty = 1;

                    purchaseInvoiceItem.PurchasePrice = item.PurchasePrice;

                    purchaseInvoiceItem.Notes = purchaseInvoice.Notes;

                    db.TbPurchaseInvoiceItems.Add(purchaseInvoiceItem);

                    item.Quantity++;

                    db.SaveChanges();

                    BillsData.DataSource = db.TbPurshaseInvoices.ToList();
                }
                catch { }
            }


        }

        private void BillsDoubleClick(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد تعديل الفاتورة؟", "تأكيد الطلب", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    ss.SupplierId = Convert.ToInt32(boxSupplier.SelectedValue);

                    ss.InvoiceDate = dateTimePicker1.Value;

                    ss.Notes = txtNotes.Text;

                    ss.UserId = Login.UserId;

                    db.SaveChanges();

                    MessageBox.Show("تم تعديل الفاتورة بنجاح");

                    BillsData.DataSource = db.TbPurshaseInvoices.ToList();
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
                    db.TbPurshaseInvoices.Remove(ss);

                    db.SaveChanges();

                    MessageBox.Show("تم الحذف بنجاح");
                    BillsData.DataSource = db.TbPurshaseInvoices.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية البحث لتعارض في قاعدة البيانات");
                return;
            }
        }

        private void Filters(object sender, EventArgs e)
        {
            try
            {
                int idCat = Convert.ToInt32(boxSupplier.SelectedValue);
                BillsData.DataSource = db.TbPurshaseInvoices.Where(x => x.SupplierId == idCat).ToList();
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PurchaseBillsEdit s = new PurchaseBillsEdit();
            s.Show();
        }


        private void Refresh(object sender, EventArgs e)
        {
            BillsData.DataSource = db.TbPurshaseInvoices.ToList();
        }

        private void BillsSelection(object sender, EventArgs e)
        {
            try
            {
                index2 = int.Parse(BillsData.CurrentRow.Cells[0].Value.ToString());

                if (index2 <= 0)
                    return;

                ss = db.TbPurshaseInvoices.FirstOrDefault(x => x.InvoiceId == index2);

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
