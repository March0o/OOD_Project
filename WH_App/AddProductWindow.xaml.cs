using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WH_App
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        WarehouseData db;
        public AddProductWindow(WarehouseData data)
        {
            db = data;

            InitializeComponent();
        }

        private void tblkName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxName.Text = "";
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = tbxName.Text;
            string productDescription = tbxDescription.Text;

            ProductInfo newProduct = new ProductInfo() { name = productName, description = productDescription, image="Images/Borger.png"};
            db.ProductInfos.AddOrUpdate(newProduct);
            db.SaveChanges();

            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }
    }
}
