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
        WarehouseData db = new WarehouseData("WarehouseDB_v1");
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
                var query = from sp in db.Stockpiles
                            where sp.section_id == selectedSection.id
                            select sp.id;

                if (query.Count() == 0) // If No Stockpile
                { tblkStockpileId.Text = "No Stockpile Assigned"; }
                else // If Stockpile Assigned
                {
                    var stockpileId = query.FirstOrDefault();
                    tblkStockpileId.Text = stockpileId.ToString();
                    var productQuery = from sp in db.Stockpiles
                                       where sp.id == stockpileId
                                       select sp.products;

                    if (productQuery.Count() == 0)
                    {
                        tblkProducts.Text = "No Products in Stockpile";
                    }
                    else
                    {
                        tblkProducts.Text = productQuery.ToList().ToString();
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
            if (lbxSections.SelectedItem != null)
            {
                Section selectedSection = lbxSections.SelectedItem as Section;
                Stockpile newStockpile = new Stockpile();
                newStockpile.section_id = selectedSection.id;
                db.Stockpiles.Add(newStockpile);
                db.SaveChanges();
                UpdateOccupiedSections();
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSections.SelectedItem != null)
            {
                Section selectedSection = lbxSections.SelectedItem as Section;

                var query = from sp in db.Stockpiles
                            where sp.section_id == selectedSection.id
                            select sp;

                Stockpile spToDelete = query.FirstOrDefault();
                db.Stockpiles.Remove(spToDelete);
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
    }
}
