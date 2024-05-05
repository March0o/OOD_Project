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
using System.Windows.Shapes;

namespace WH_App
{
    /// <summary>
    /// Interaction logic for ProductInfoWindow.xaml
    /// </summary>
    public partial class ProductInfoWindow : Window
    {
        ProductInfo displayedProduct = null;
        WarehouseData db = null;
        public ProductInfoWindow(ProductInfo product, WarehouseData data)
        {
            InitializeComponent();

            db = data;
            displayedProduct = product;

            tblkName.Text = $"{displayedProduct.name} [{displayedProduct.id}]";
            tblkDescription.Text = displayedProduct.description;

            var quantityQuery = from p in db.ProductQuantities
                                where p.product_id == displayedProduct.id
                                select p.quantity;
            int quantity = quantityQuery.ToList().Sum();
            tblkQuantity.Text = "Total: " + $"{quantity}";
        }
    }
}
