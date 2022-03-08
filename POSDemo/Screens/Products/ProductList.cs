using POSDemo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSDemo.Screens.Products
{
    public partial class ProductList : Form
    {
        POSEntities db = new POSEntities();
        int index = 0;
        TbItem item;
        string ImagePath = "";
        public ProductList()
        {
            InitializeComponent();

            //dataGridView1.DataSource = db.TbItems.ToList();
            comboBox2.DataSource = db.TbCategories.ToList();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pOSDataSet4.TbCategory' table. You can move, or remove it, as needed.
            this.tbCategoryTableAdapter.Fill(this.pOSDataSet4.TbCategory);
            // TODO: This line of code loads data into the 'pOSDataSet.TbItem' table. You can move, or remove it, as needed.
            this.tbItemTableAdapter.Fill(this.pOSDataSet.TbItem);

        }

        private void Search(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text))
                dataGridView1.DataSource = db.TbItems.Where(x => x.ItemCode == txtCode.Text).ToList();
            else
                dataGridView1.DataSource = db.TbItems.Where(x => x.ItemCode == txtCode.Text
             || x.ItemName.Contains(txtName.Text)).ToList();
        }

        private void Refresh(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TbItems.ToList();
        }

        private void dataGridSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                index = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                item = db.TbItems.FirstOrDefault(x => x.ItemId == index);

                if(item != null)
                {
                    txtFormCode.Text = item.ItemCode;

                    txtFormName.Text = item.ItemName;

                    txtPrice.Text = item.SalesPrice.ToString();

                    txtPurchase.Text = item.PurchasePrice.ToString();

                    txtQty.Text = item.Quantity.ToString();

                    txtNotes.Text = item.Notes.ToString();

                    pictureBox1.ImageLocation = item.Image;

                    comboBox1.SelectedValue = item.CategoryId;
                }
              
            }
            catch
            {

            }
            
        }

        private void EditProduct(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد التعديل؟", "تأكيد التعديل", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                item.ItemName = txtFormName.Text;

                item.CategoryId = Convert.ToInt32(comboBox1.SelectedValue.ToString());

                decimal purchasePrice;
                bool isPueConverted = decimal.TryParse(txtPurchase.Text, out purchasePrice);
                if (isPueConverted)
                {
                    item.PurchasePrice = purchasePrice;
                }
                else
                {
                    MessageBox.Show("رجاءأدخال سعر الشراء بطريقة صحيحة");
                    return;
                }

                decimal salesPrice;
                bool isDecConverted = decimal.TryParse(txtPrice.Text, out salesPrice);

                int qty;
                bool isIntConverted = int.TryParse(txtQty.Text, out qty);
                if (isDecConverted)
                {
                    item.SalesPrice = salesPrice;
                }
                else
                {
                    MessageBox.Show("رجاءأدخال سعر البيع بطريقة صحيحة");
                    return;
                }
                if (isIntConverted)
                {
                    item.Quantity = qty;
                }
                else
                {
                    MessageBox.Show("رجاءأدخال الكمية بطريقة صحيحة");
                    return;

                }


                item.Notes = txtNotes.Text;

                item.ItemCode = txtFormCode.Text;

                db.SaveChanges();

                if (!string.IsNullOrEmpty(ImagePath))
                {
                    string imagePath = Environment.CurrentDirectory + "\\Images\\Product\\" + item.ItemId + ".jpg";

                    File.Copy(ImagePath, imagePath, true);

                    item.Image = imagePath;

                    db.SaveChanges();
                }
                MessageBox.Show("تم التعديل");

                dataGridView1.DataSource = db.TbItems.ToList();
            }
      
        }

        private void ImageBox(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ImagePath = dialog.FileName;
                pictureBox1.ImageLocation = dialog.FileName;
            }
        }

        private void RemoveProduct(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد الحذف؟", "تأكيد الحذف", MessageBoxButtons.YesNo);
            if(r == DialogResult.Yes)
            {
                var result = db.TbItems.Find(index);
                db.TbItems.Remove(result);

                db.SaveChanges();

                MessageBox.Show("تم الحذف بنجاح");           
            }
    
        }

        private void NavToProductScreen(object sender, EventArgs e)
        {
            Product p = new Product();
            p.Show();
        }

        private void Filters(object sender, EventArgs e)
        {
            int idCat = Convert.ToInt32(comboBox2.SelectedValue.ToString());
            dataGridView1.DataSource = db.TbItems.Where(x => x.CategoryId == idCat).ToList();
        }

    }
}
