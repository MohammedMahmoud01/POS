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

namespace POSDemo.Screens.Supplier
{
    public partial class Supplier : Form
    {
        POSEntities db = new POSEntities();
        int index = 0;
        TbSupplier supplier;
        string ImagePath = "";
        public Supplier()
        {
            InitializeComponent();

            dataGridView1.DataSource = db.TbSuppliers.ToList();
        }

        private void Search(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchName.Text))
                dataGridView1.DataSource = db.TbSuppliers.Where(x => x.Phone == txtSearchPh.Text).ToList();
            else
                dataGridView1.DataSource = db.TbSuppliers.Where(x => x.Phone == txtSearchPh.Text
             || x.SupplierName.Contains(txtSearchName.Text)).ToList();
        }

        private void Refresh(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TbSuppliers.ToList();
        }

        private void AddSupplier(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد اضافة عميل؟", "تأكيد اضافة عميل جديد", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPhone.Text))
                {
                    MessageBox.Show("رجاء أكمال البيانات المطلوبة أولا");
                }else if (db.TbSuppliers.Where(x=>x.Phone == txtPhone.Text).Count() > 0)
                {
                    MessageBox.Show("هذا المورد موجود مسبقا ");
                }
                else
                {
                    TbSupplier sSupplier = new TbSupplier();
                    sSupplier.SupplierName = txtName.Text;

                    sSupplier.Phone = txtPhone.Text;

                    sSupplier.Notes = txtNotes.Text;

                    sSupplier.Address = txtAdd.Text;

                    sSupplier.Email = txtEmail.Text;

                    sSupplier.Company = txtCom.Text;

                    db.TbSuppliers.Add(sSupplier);

                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(ImagePath))
                    {
                        string imagePath = Environment.CurrentDirectory + "\\Images\\Supplier\\"
                            + sSupplier.SupplierId + ".jpg";

                        File.Copy(ImagePath, imagePath);

                        sSupplier.Image = imagePath;

                        db.SaveChanges();
                    }
                    MessageBox.Show("تم الحفظ");

                    txtPhone.Text = "";
                    txtName.Text = "";
                    txtNotes.Text = "";
                    txtCom.Text = "";
                    txtAdd.Text = "";
                    txtEmail.Text = "";
                    dataGridView1.DataSource = db.TbSuppliers.ToList();
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

                supplier = db.TbSuppliers.SingleOrDefault(x => x.SupplierId == index);

                if (supplier != null)
                {
                    txtName.Text = supplier.SupplierName;

                    txtEmail.Text = supplier.Email;

                    txtAdd.Text = supplier.Address;

                    txtPhone.Text = supplier.Phone;

                    txtNotes.Text = supplier.Notes;

                    txtCom.Text = supplier.Company;

                    pictureBox1.ImageLocation = supplier.Image;
                }

            }
            catch
            {

            }
        }

        private void Edit(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد تعديل العميل؟", "تأكيد تعديل العميل ", MessageBoxButtons.YesNo);
            if (db.TbSuppliers.Where(x => x.Phone == txtPhone.Text && x.SupplierId != index).Count() > 0)
            {
                MessageBox.Show("هذا المورد موجود مسبقا ");
                return;
            }
            if (r == DialogResult.Yes)
            {
                supplier.SupplierName = txtName.Text;

                supplier.Phone = txtPhone.Text;

                supplier.Notes = txtNotes.Text;

                supplier.Address = txtAdd.Text;

                supplier.Email = txtEmail.Text;

                supplier.Company = txtCom.Text;

                db.SaveChanges();

                if (!string.IsNullOrEmpty(ImagePath))
                {
                    string imagePath = Environment.CurrentDirectory + "\\Images\\Supplier\\"
                        + supplier.SupplierId + ".jpg";

                    File.Copy(ImagePath, imagePath, true);

                    supplier.Image = imagePath;

                    db.SaveChanges();
                }
                MessageBox.Show("تم التعديل");

                txtPhone.Text = "";
                txtName.Text = "";
                txtNotes.Text = "";
                txtCom.Text = "";
                txtAdd.Text = "";
                txtEmail.Text = "";
                dataGridView1.DataSource = db.TbSuppliers.ToList();
            }
        }

        private void Remove(object sender, EventArgs e)
        {
            var r = MessageBox.Show("هل تريد الحذف؟", "تأكيد الحذف", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                var result = db.TbSuppliers.Find(index);
                db.TbSuppliers.Remove(result);

                db.SaveChanges();

                MessageBox.Show("تم الحذف بنجاح");
                dataGridView1.DataSource = db.TbSuppliers.ToList();
            }
        }
    }
}
