using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WH_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WarehouseData db = new WarehouseData("WarehouseDB_v3");
        public MainWindow()
        {
            InitializeComponent();

            // Get Areas
            var areaQuery = from a in db.Areas
                            select a;
            List<Area> areaList = areaQuery.ToList();
            //  Populate lbxAreas
            lbxAreas.ItemsSource = null;
            lbxAreas.ItemsSource = areaList;
            //  Get Products
            var productsQuery = from p in db.ProductInfos
                                select p;
            List<ProductInfo> productsList = productsQuery.ToList();
            //  Populate lbxProducts
            lbxProducts.ItemsSource = null;
            lbxProducts.ItemsSource = productsList;
            //  Populate cbx
            string[] searchOptions = { "ID", "Name" };
            cbxSearch.ItemsSource = searchOptions;
            cbxSearch.SelectedIndex = 0;
        }

        private void lbxAreas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxAreas.SelectedItem != null)
            {
                Area selectedArea = lbxAreas.SelectedItem as Area;
                AreaWindow window = new AreaWindow(db, selectedArea.id);
                this.Close();
                window.Show();
            }
        }

        private void lbxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxProducts.SelectedValue != null)
            {
                //  Pass product to new window
                ProductInfo product = lbxProducts.SelectedValue as ProductInfo;
                ProductInfoWindow infoWindow = new ProductInfoWindow(product, db);
                infoWindow.Show();
                
            }
        }

        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = tbxSearch.Text;
            if ((string)cbxSearch.SelectedValue == "ID")
            {
                var idQuery = from p in db.ProductInfos
                              where p.id.ToString().Contains(input)
                              select p;
                lbxProducts.ItemsSource = idQuery.ToList();
            }
            else if ((string)cbxSearch.SelectedValue == "Name")
            {
                var nameQuery = from p in db.ProductInfos
                                where p.name.Contains(input)
                                select p;
                lbxProducts.ItemsSource = nameQuery.ToList();
            }
                           
        }

        private void tbxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxSearch.Text = "";
        }
    }
}
