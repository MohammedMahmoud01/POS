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

namespace POSDemo.Screens.Customer
{
    public partial class Customer : Form
    {
        POSEntities db = new POSEntities();
        int index = 0;
        TbCustomer customer;
        string ImagePath = "";
        public Customer()
        {
            InitializeComponent();

            dataGridView1.DataSource = db.TbCustomers.ToList();
        }


        private void Search(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchName.Text))
                dataGridView1.DataSource = db.TbCustomers.Where(x => x.Phone == txtSearchPh.Text).ToList();
            else
                dataGridView1.DataSource = db.TbCustomers.Where(x => x.Phone == txtSearchPh.Text
             || x.CustomerName.Contains(txtSearchName.Text)).ToList();
        }

        private void Refresh(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TbCustomers.ToList();
        }

        private void AddCustomer(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد اضافة عميل؟", "تأكيد اضافة عميل جديد", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPhone.Text))
                {
                    MessageBox.Show("رجاء أكمال البيانات المطلوبة أولا");
                }
                else if (db.TbCustomers.Where(x => x.Phone == txtPhone.Text).Count() > 0)
                {
                    MessageBox.Show("هذا العميل موجود مسبقا ");
                }
                else
                {
                    TbCustomer cCustomer = new TbCustomer();
                    cCustomer.CustomerName = txtName.Text;

                    cCustomer.Phone = txtPhone.Text;

                    cCustomer.Notes = txtNotes.Text;

                    cCustomer.Address = txtAdd.Text;

                    cCustomer.Email = txtEmail.Text;

                    cCustomer.Company = txtCom.Text;

                    db.TbCustomers.Add(cCustomer);

                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(ImagePath))
                    {
                        string imagePath = Environment.CurrentDirectory + "\\Images\\Customer\\"
                            + cCustomer.CustomerId + ".jpg";

                        File.Copy(ImagePath, imagePath);

                        cCustomer.Image = imagePath;

                        db.SaveChanges();
                    }
                    MessageBox.Show("تم الحفظ");

                    txtPhone.Text = "";
                    txtName.Text = "";
                    txtNotes.Text = "";
                    txtCom.Text = "";
                    txtAdd.Text = "";
                    txtEmail.Text = "";
                    dataGridView1.DataSource = db.TbCustomers.ToList();
                }
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

        private void dataGridSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                index = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                customer = db.TbCustomers.FirstOrDefault(x => x.CustomerId == index);

                if (customer != null)
                {
                    txtName.Text = customer.CustomerName;

                    txtEmail.Text = customer.Email;

                    txtAdd.Text = customer.Address;

                    txtPhone.Text = customer.Phone;

                    txtNotes.Text = customer.Notes;

                    txtCom.Text = customer.Company;

                    pictureBox1.ImageLocation = customer.Image;
                }

            }
            catch
            {

            }
        }

        private void EditCustomer(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد تعديل العميل؟", "تأكيد تعديل العميل ", MessageBoxButtons.YesNo);
            if (db.TbCustomers.Where(x => x.Phone == txtPhone.Text && x.CustomerId != index).Count() > 0)
            {
                MessageBox.Show("هذا العميل موجود مسبقا ");
                return;
            }
            if (r == DialogResult.Yes)
            {
                customer.CustomerName = txtName.Text;

                customer.Phone = txtPhone.Text;

                customer.Notes = txtNotes.Text;

                customer.Address = txtAdd.Text;

                customer.Email = txtEmail.Text;

                customer.Company = txtCom.Text;

                db.SaveChanges();

                if (!string.IsNullOrEmpty(ImagePath))
                {
                    string imagePath = Environment.CurrentDirectory + "\\Images\\Customer\\"
                        + customer.CustomerId + ".jpg";

                    File.Copy(ImagePath, imagePath, true);

                    customer.Image = imagePath;

                    db.SaveChanges();
                }
                MessageBox.Show("تم التعديل");

                txtPhone.Text = "";
                txtName.Text = "";
                txtNotes.Text = "";
                txtCom.Text = "";
                txtAdd.Text = "";
                txtEmail.Text = "";
                dataGridView1.DataSource = db.TbCustomers.ToList();
            }          
        }

        private void DeleteCustomer(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد الحذف؟", "تأكيد الحذف", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                var result = db.TbCustomers.Find(index);
                db.TbCustomers.Remove(result);

                db.SaveChanges();

                MessageBox.Show("تم الحذف بنجاح");
                dataGridView1.DataSource = db.TbCustomers.ToList();
            }
        }

    }
}
