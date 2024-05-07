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
    /// Interaction logic for AreaWindow.xaml
    /// </summary>
    public partial class AreaWindow : Window
    {
        WarehouseData db = null;
        int AreaId = 0;
        public AreaWindow(WarehouseData data, int areaId)
        {
            db = data;
            AreaId = areaId;
            InitializeComponent();

            // Get Area Info
            var areaQuery = from a in db.Areas
                            where a.id == AreaId
                            select a;
            Area area = areaQuery.FirstOrDefault();
            tblkArea.Text = $"{area.name} [{area.id}]";

            // Get Sections
            var sectionQuery = from s in db.Sections
                               where s.area_id == AreaId
                               select s;

            //  Populate lbxSection
            lbxSections.ItemsSource = null;
            lbxSections.ItemsSource = sectionQuery.ToList();

            UpdateOccupiedSections();
        }
        private void lbxSections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxSections.SelectedItem != null)
            {
                //  Get Section Stockpile info
                Section selectedSection = lbxSections.SelectedItem as Section;
                var stockpileQuery = from sp in db.Stockpiles
                                     where sp.section_id == selectedSection.id
                                     select sp;


                if (stockpileQuery.Count() == 0) // If No Stockpile
                { tblkStockpileId.Text = "No Stockpile Assigned"; lbxProducts.ItemsSource = null; }
                else // If Stockpile Assigned
                {
                    //  Assign textblock info
                    Stockpile currentStockpile = stockpileQuery.FirstOrDefault() as Stockpile;
                    tblkStockpileId.Text = "Stockpile ID: " + $"{currentStockpile.id.ToString()}";

                    //  Get Products that are located in stockpile
                    var productQuery = from p in db.ProductQuantities
                                       where p.owning_stockpile == currentStockpile.id
                                       select p;

                    List<ProductQuantity> productList = productQuery.ToList();

                    //  Assign listbox info
                    if (productList.Count == 0)
                    {
                        lbxProducts.ItemsSource = null;
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
                //  Get internal values
                Section selectedSection = lbxSections.SelectedItem as Section;
                if (cbxMoveTo.SelectedItem != null)
                {
                    int newId = (int)cbxMoveTo.SelectedItem;
                    //  get current stockpile
                    var stockpileQuery = from sp in db.Stockpiles
                                         where sp.section_id == selectedSection.id
                                         select sp;
                    Stockpile stockpile = stockpileQuery.FirstOrDefault();
                    if (stockpile != null)
                    {
                        //  assign stockpile to new section
                        stockpile.section_id = newId;
                        db.SaveChanges();
                        UpdateOccupiedSections();
                        RefreshListBox();
                    }
                    else
                    {
                        MessageBox.Show("Select a Valid Section");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Section to transfer to");
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //  Get section id's of sections with stockpiles
            var OccupiedQuery = from sp in db.Stockpiles
                                select sp.section_id;
            var OccupiedList = OccupiedQuery.ToList();
            //  Get section id's of all sections
            var SectionIdQuery = from s in db.Sections
                                 select s.id;
            var SectionIdList = SectionIdQuery.ToList();
            //  Add to list if id not in occupied list
            List<int> emptySections = new List<int>();
            foreach (var id in SectionIdList)
            {
                if (OccupiedList.Contains(id))
                { }
                else { emptySections.Add(id); }
            }

            if (lbxSections.SelectedItem != null)
            {
                //  make sure id used is in empty section
                Section selectedSection = lbxSections.SelectedItem as Section;
                if (emptySections.Contains(selectedSection.id))
                {
                    AddStockpileWindow window = new AddStockpileWindow(selectedSection.id, db,AreaId);
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
                //  Get stockpile to delete
                var stockpileQuery = from sp in db.Stockpiles
                                     where sp.section_id == selectedSection.id
                                     select sp;
                Stockpile spToDelete = stockpileQuery.FirstOrDefault();
                if (spToDelete != null)
                {
                    //  Get products to delete
                    var productsQuery = from p in db.ProductQuantities
                                        where p.owning_stockpile == spToDelete.id
                                        select p;
                    //  Start changes on database
                    db.Stockpiles.Remove(spToDelete);
                    db.ProductQuantities.RemoveRange(productsQuery);
                    db.SaveChanges();
                    UpdateOccupiedSections();
                    RefreshListBox();
                }
                else
                {
                    MessageBox.Show("Nothing to Delete");
                }
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
                //  Get selected product
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
                    //  pass product to new window
                    ProductInfo product = productQuery.FirstOrDefault() as ProductInfo;
                    ProductInfoWindow infoWindow = new ProductInfoWindow(product, db);
                    infoWindow.Show();
                }
            }
        }

        public void RefreshListBox()
        {
            // Assuming lbxSections is your ListBox
            // Store the currently selected item
            var selectedItem = lbxSections.SelectedItem;

            // Temporarily set the selected item to null
            lbxSections.SelectedItem = null;

            // Set the selected item back to the original value
            lbxSections.SelectedItem = selectedItem;

            // Create a new SelectionChangedEventArgs object
            var args = new SelectionChangedEventArgs(
                ListBox.SelectionChangedEvent,
                new List<object>() { selectedItem }, // Newly selected items
                new List<object>()); // Items being unselected

            // Raise the SelectionChanged event with the new arguments
            lbxSections.RaiseEvent(args);
        }
    }
}
