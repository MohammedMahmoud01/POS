using POSDemo.Screens.Users;
using POSDemo.Screens.Products;
using POSDemo.Screens.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSDemo.Screens.Supplier;
using POSDemo.Screens.SalesBills;
using POSDemo.Screens.PurchaseBills;

namespace POSDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
