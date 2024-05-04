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
        WarehouseData db = new WarehouseData("WarehouseDB_v2");
        public MainWindow()
        {
            InitializeComponent();

            var SectionQuery = from s in db.Sections
                               select s;

            //  Populate lbxSection
            lbxSections.ItemsSource = null;
            lbxSections.ItemsSource = SectionQuery.ToList();

            UpdateOccupiedSections();
        }

        private void lbxSections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxSections.SelectedItem != null)
            {
                Section selectedSection = lbxSections.SelectedItem as Section;
                var stockpileQuery = from sp in db.Stockpiles
                            where sp.section_id == selectedSection.id
                            select sp;


                if (stockpileQuery.Count() == 0) // If No Stockpile
                { tblkStockpileId.Text = "No Stockpile Assigned"; lbxProducts.ItemsSource= new List<String>() { "N/A" }; }
                else // If Stockpile Assigned
                {
                    Stockpile currentStockpile = stockpileQuery.FirstOrDefault() as Stockpile;
                    tblkStockpileId.Text = "Stockpile ID: " + $"{currentStockpile.id.ToString()}";

                    var productQuery = from p in db.ProductQuantities
                                       where p.owning_stockpile == currentStockpile.id
                                       select p;

                    List<ProductQuantity> productList = productQuery.ToList();

                    if (productList.Count == 0)
                    {
                        lbxProducts.ItemsSource = new List<String>() {"No Products Found in List"};
                    }
                    else
                    {
                        lbxProducts.ItemsSource = productList;
                    }
                }
            }                 
        }

        private void BtnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSections.SelectedItem != null)
            {
                Section selectedSection = lbxSections.SelectedItem as Section;
                int newId = (int)cbxMoveTo.SelectedItem;

                var stockpileQuery = from sp in db.Stockpiles
                                where sp.section_id == selectedSection.id
                                select sp;

                Stockpile stockpile = stockpileQuery.FirstOrDefault();
                stockpile.section_id = newId;
                db.SaveChanges();

                UpdateOccupiedSections();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var OccupiedQuery = from sp in db.Stockpiles
                                select sp.section_id;

            var OccupiedList = OccupiedQuery.ToList();

            var SectionIdQuery = from s in db.Sections
                                 select s.id;

            var SectionIdList = SectionIdQuery.ToList();
            List<int> emptySections = new List<int>();

            foreach (var id in SectionIdList)
            {
                if (OccupiedList.Contains(id))
                { }
                else { emptySections.Add(id); }
            }

            if (lbxSections.SelectedItem != null)
            {
                Section selectedSection = lbxSections.SelectedItem as Section;
                if (emptySections.Contains(selectedSection.id))
                {
                    AddStockpileWindow window = new AddStockpileWindow(selectedSection.id, db);
                    this.Close();
                    window.Show();
                }
                else
                {
                    MessageBox.Show("Section is not Empty");
                }
                
            }
            else
            {
                MessageBox.Show("No Section Selected");
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSections.SelectedItem != null)
            {
                Section selectedSection = lbxSections.SelectedItem as Section;

                var stockpileQuery = from sp in db.Stockpiles
                            where sp.section_id == selectedSection.id
                            select sp;
                Stockpile spToDelete = stockpileQuery.FirstOrDefault();

                var productsQuery = from p in db.ProductQuantities
                                    where p.owning_stockpile == spToDelete.id
                                    select p;

                db.Stockpiles.Remove(spToDelete);
                db.ProductQuantities.RemoveRange(productsQuery);
                db.SaveChanges();
                UpdateOccupiedSections();
            }
        }

        public void UpdateOccupiedSections()
        {
            var OccupiedQuery = from sp in db.Stockpiles
                                select sp.section_id;

            var OccupiedList = OccupiedQuery.ToList();

            var SectionIdQuery = from s in db.Sections
                                 select s.id;

            var SectionIdList = SectionIdQuery.ToList();
            List<int> emptySections = new List<int>();

            foreach (var id in SectionIdList)
            {
                if (OccupiedList.Contains(id))
                { }
                else { emptySections.Add(id); }
            }
            cbxMoveTo.ItemsSource = emptySections;
        }

        private void lbxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxProducts.SelectedValue != null)
            {
                int productId = (int)lbxProducts.SelectedValue;

                var productQuery = from pi in db.ProductInfos
                                   where pi.id == productId
                                   select pi;

                if (productQuery.ToList().Count() == 0)
                {
                    MessageBox.Show("Product Doesn't Exist!");
                }
                else
                {
                    ProductInfo product = productQuery.FirstOrDefault() as ProductInfo;

                    ProductInfoWindow infoWindow = new ProductInfoWindow(product, db);
                    infoWindow.Show();
                }
            }
        }
    }
}
