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

namespace POSDemo.Screens.CategoryScreen
{
    public partial class Category : Form
    {
        POSEntities db = new POSEntities();
        int index;
        TbCategory cat;
        public Category()
        {
            InitializeComponent();

            dataGridView1.DataSource = db.TbCategories.ToList();
        }

        private void AddCategory(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCat.Text))
            {
                MessageBox.Show("عذرا يجب أدخال القسم");
            }
            else
            {
                var r = MessageBox.Show("هل تريد الحفظ؟", "تأكيد الحفظ", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    TbCategory cat = new TbCategory
                    {
                        CategoryName = txtCat.Text,
                    };
                    db.TbCategories.Add(cat);

                    db.SaveChanges();

                    MessageBox.Show("تم الحفظ");
                }
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show("هل تريد التعديل؟", "تأكيد التعديل", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    cat.CategoryName = txtCat.Text;

                    db.SaveChanges();
                }

            }
            catch { }
        }

        private void CatSelection(object sender, EventArgs e)
        {
            index = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

            cat = db.TbCategories.FirstOrDefault(x => x.CategoryId == index);

            if (cat != null)
            {
                txtCat.Text = cat.CategoryName;
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TbCategories.ToList();
        }
    }
}
