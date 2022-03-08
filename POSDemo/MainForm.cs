using POSDemo.Screens.CategoryScreen;
using POSDemo.Screens.Customer;
using POSDemo.Screens.Products;
using POSDemo.Screens.Supplier;
using POSDemo.Screens.Users;
using POSDemo.Screens.SalesBills;
using POSDemo.Screens.PurchaseBills;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            txtUser.Text = Login.UserName;
        }

        private void AddNewUserMenuItem(object sender, EventArgs e)
        {
            NewUser fUser = new NewUser();
            fUser.Show();
        }

        private void NavToSalesBillsForm(object sender, EventArgs e)
        {
            SalesBillsForm s = new SalesBillsForm();
            s.Show();
        }

        private void NavToPurchaseBillsForm(object sender, EventArgs e)
        {
            PurchaseBillsForm p = new PurchaseBillsForm();
            p.Show();
        }
        private void NavToCustomer(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.Show();
        }

        private void CloseMenuItem(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddProductMenuItem(object sender, EventArgs e)
        {
            Product p = new Product();
            p.Show();
        }

        private void NavToProduct(object sender, EventArgs e)
        {
            Product p = new Product();
            p.Show();
        }

        private void ListProductMenuItem(object sender, EventArgs e)
        {
            ProductList productList = new ProductList();
            productList.Show();
        }

        private void EditProductMenuItem(object sender, EventArgs e)
        {
            ProductList pr = new ProductList();
            pr.Show();
        }

        private void CustomerMenuItem(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.Show();
        }

        private void SupplierMenuItem(object sender, EventArgs e)
        {
            Supplier supplier = new Supplier();
            supplier.Show();
        }

        private void NavToSupplier(object sender, EventArgs e)
        {
            Supplier supplier = new Supplier();
            supplier.Show();
        }

        private void AddCategoryMenuItem(object sender, EventArgs e)
        {
            Category cat = new Category();
            cat.Show();
        }

        private void PurchaseBillsMenuItem(object sender, EventArgs e)
        {
            PurchaseBillsForm p = new PurchaseBillsForm();
            p.Show();
        }

        private void SalesBillsMenuItem(object sender, EventArgs e)
        {
            SalesBillsForm s = new SalesBillsForm();
            s.Show();
        }

    }
}
