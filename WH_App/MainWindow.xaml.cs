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

        private void lbxSections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Section selectedSection = lbxSections.SelectedItem as Section;
            if (selectedSection != null) 
            {
                var query = from sp in db.Stockpiles
                            where sp.section_id == selectedSection.id
                            select sp.id;

                if (query.Count() == 0) 
                { tblkStockpileId.Text = "No Stockpile Assigned"; }
                else
                {
                  tblkStockpileId.Text = query.FirstOrDefault().ToString();
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
            }
        }
    }
}
