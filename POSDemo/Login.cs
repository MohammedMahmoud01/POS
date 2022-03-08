using POSDemo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSDemo
{
    public partial class Login : Form
    {
        //m@gmail.com
        //1
        POSEntities db = new POSEntities();
        public static int UserId;
        public static string UserName;
       
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn(object sender, EventArgs e)
        {     
            var result = db.TbUsers.FirstOrDefault(x => x.Email == textEmail.Text &&
            x.Password == txtPassword.Text);
            


            if (result == null)
            {
                MessageBox.Show("خطأ في البيانات رجاء معاودة المحاولة");
            }
            else
            {
                UserId = result.UserId;
                UserName = result.UserName;
                this.Close();
                Thread th = new Thread(OpenMainForm);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
        }
        void OpenMainForm()
        {
            Application.Run(new MainForm());
        }

    }
}
