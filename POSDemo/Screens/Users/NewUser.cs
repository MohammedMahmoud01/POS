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

namespace POSDemo.Screens.Users
{
    public partial class NewUser : Form
    {
        POSEntities db = new POSEntities();
        string ImagePath="";
        public NewUser()
        {
            InitializeComponent();
        }

        private void AddNewUser(object sender, EventArgs e)
        {
            //TbUser user = new TbUser();
            //user.UserName = txtUser.Text;
            //user.Password = txtPassword.Text;

            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtEmail.Text)
                || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("رجاء أكمال البيانات المطلوبة أولا");                
            }
            else
            {
                TbUser user = new TbUser
                {
                    UserName = txtUser.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text
                };
                db.TbUsers.Add(user);
                db.SaveChanges();


                if(!string.IsNullOrEmpty(ImagePath))
                {
                    string imagePath = Environment.CurrentDirectory + "\\Images\\Users\\" + user.UserId + ".jpg";

                    File.Copy(ImagePath, imagePath);

                    user.Image = imagePath;

                    db.SaveChanges();
                }          

                MessageBox.Show("تم الحفظ");
            }

        }
        private void ImageBox(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                ImagePath = dialog.FileName;
                pictureBox1.ImageLocation = dialog.FileName;
            }
        }
    }
}
