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
        int areaId = 0;
        WarehouseData db = null;
        List<ProductQuantity> productsList = new List<ProductQuantity>();

        public AddStockpileWindow(int sectionId, WarehouseData data, int areaId)
        {
            InitializeComponent();

            //  Assign program-wide variables
            this.sectionId = sectionId;
            this.db = data;
            this.areaId = areaId;

            //  Get Products
            var productQuery = from pi in db.ProductInfos
                               select pi;

            //  Assign to product combobox
            List<ProductInfo> productList = productQuery.ToList();
            cbxProductToAdd.ItemsSource = null;
            cbxProductToAdd.ItemsSource = productList;
            cbxProductToAdd.SelectedIndex = 0;
            this.areaId = areaId;
        }

        private void btnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            //  Get input Values
            int quantity;
            if (int.TryParse(tbxProductQty.Text, out quantity))
            {
                int productId = (int)cbxProductToAdd.SelectedValue;
                DateTime dateTime;
                if (DateTime.TryParse(dpExpiryDate.SelectedDate.ToString(), out dateTime))
                {
                    //  Create Product & add to list
                    ProductQuantity product = new ProductQuantity() { product_id = productId, quantity = quantity, expiry_date = dateTime };
                    productsList.Add(product);
                    RefreshList();
                }
                else
                {
                    MessageBox.Show("Please Select a Date");
                }
            }
            else
            {
                MessageBox.Show("Quantity Input Invalid");
            }

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
            if (productsList.Count() == 0)
            {
                // Insert placeholder object for id
                Stockpile placeholderStockpile = new Stockpile();
                db.Stockpiles.AddOrUpdate(placeholderStockpile);
                db.SaveChanges();
                int newId = placeholderStockpile.id;

                //  Assign Id to product list items
                foreach (ProductQuantity product in productsList)
                {
                    product.owning_stockpile = newId;
                }
                //  Create full Stockpile
                Stockpile updatedStockpile = new Stockpile() { id = newId, section_id = sectionId };
                updatedStockpile.products = productsList;

                //  Update Database
                db.Stockpiles.AddOrUpdate(updatedStockpile);
                db.ProductQuantities.AddRange(productsList);
                db.SaveChanges();

                //  Close Current Window & Open new one
                AreaWindow window = new AreaWindow(db, areaId);
                this.Close();
                window.Show();
            }
           else
            {
                MessageBox.Show("List is Empty");
            }
        }

        private void tbxProductQty_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxProductQty.Text = "";
        }
    }
}
