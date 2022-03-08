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
    public partial class Product : Form
    {
        POSEntities db = new POSEntities();
        string ImagePath = "";
        public Product()
        {
            InitializeComponent();
        }

        private void SaveItem(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPrice.Text)
               || string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtQty.Text)
               || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("رجاء أكمال البيانات المطلوبة أولا");            
            }
            else
            {
                TbItem item = new TbItem();
                item.ItemName = txtName.Text;


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
                bool isDecConverted = decimal.TryParse(txtPrice.Text , out salesPrice);

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

                item.ItemCode = txtCode.Text;

                if(comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("رجاءأدخال الصنف بطريقة صحيحة");
                    return;
                }
                else
                {
                    item.CategoryId = int.Parse(comboBox1.SelectedValue.ToString());
                }
                db.TbItems.Add(item);

                db.SaveChanges();

                if(!string.IsNullOrEmpty(ImagePath))
                {
                    string imagePath = Environment.CurrentDirectory + "\\Images\\Product\\" + item.ItemId + ".jpg";

                    File.Copy(ImagePath, imagePath);

                    item.Image = imagePath;

                    db.SaveChanges();
                }
                MessageBox.Show("تم الحفظ");

                txtCode.Text = "";
                txtName.Text = "";
                txtNotes.Text = "";
                txtPrice.Text = "";
                txtPurchase.Text = "";
                txtQty.Text = "";
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

        private void ProductsScreen(object sender, EventArgs e)
        {
            ProductList pr = new ProductList();
            pr.Show();
        }
    
        private void Product_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pOSDataSet3.TbCategory' table. You can move, or remove it, as needed.
            this.tbCategoryTableAdapter.Fill(this.pOSDataSet3.TbCategory);

        }   
    }
}
