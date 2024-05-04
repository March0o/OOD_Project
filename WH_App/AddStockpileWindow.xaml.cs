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
    /// Interaction logic for AddStockpileWindow.xaml
    /// </summary>
    public partial class AddStockpileWindow : Window
    {
        int sectionId = 0;
        WarehouseData db = null;
        List<ProductQuantity> productsList = new List<ProductQuantity>();

        public AddStockpileWindow(int sectionId, WarehouseData data)
        {
            InitializeComponent();

            this.sectionId = sectionId;
            this.db = data;

            var productQuery = from pi in db.ProductInfos
                               select pi;

            List<ProductInfo> productList = productQuery.ToList();
            cbxProductToAdd.ItemsSource = null;
            cbxProductToAdd.ItemsSource = productList;
        }

        private void btnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            int quantity = int.Parse(tbxProductQty.Text);
            int productId = (int)cbxProductToAdd.SelectedValue;

            ProductQuantity product = new ProductQuantity() { product_id = productId, quantity = quantity };
            productsList.Add(product);
            RefreshList();
        }

        public void RefreshList()
        {
            lbxProducts.ItemsSource = null;
            lbxProducts.ItemsSource = productsList;
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (lbxProducts.SelectedItem != null)
            {
                ProductQuantity productToDelete = (ProductQuantity)lbxProducts.SelectedItem;
                productsList.Remove(productToDelete);
                RefreshList();
            }
            else
            {
                MessageBox.Show("Nothing Selected");
            }
        }

        private void btnAddStockpile_Click(object sender, RoutedEventArgs e)
        {
            // Create a placeholder record with minimal data to reserve an ID
            Stockpile placeholderStockpile = new Stockpile();
            db.Stockpiles.AddOrUpdate(placeholderStockpile);
            db.SaveChanges();

            // Retrieve the ID of the reserved placeholder record
            int newId = placeholderStockpile.id;

            foreach (ProductQuantity product in productsList)
            {
                product.owning_stockpile = newId;
            }

            Stockpile updatedStockpile = new Stockpile() {id = newId, section_id = sectionId};
            updatedStockpile.products = productsList;
            db.Stockpiles.AddOrUpdate(updatedStockpile);
            db.ProductQuantities.AddRange(productsList);
            db.SaveChanges();

            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }
    }
}
