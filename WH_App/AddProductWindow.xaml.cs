using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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
        string imageName;
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
            //  Get Input Values
            string productName = tbxName.Text;
            string productDescription = tbxDescription.Text;
            //  Add Products to Database
            ProductInfo newProduct = new ProductInfo() { name = productName, description = productDescription, image="/Images/" + imageName};
            db.ProductInfos.AddOrUpdate(newProduct);
            db.SaveChanges();
            //  Change Windows
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

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            //  Open File Dialop
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a PNG file to download";
            openFileDialog.Filter = "PNG Files (*.png)|*.png";

            if (openFileDialog.ShowDialog() == true) // if ok
            {
                //  Get Directories
                string selectedFileName = openFileDialog.FileName;
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string solutionDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory).FullName).FullName).FullName;
                string destinationFolder = System.IO.Path.Combine(solutionDirectory, "Images");
                string destinationFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(selectedFileName));
                //  Move to Images Folder
                Directory.CreateDirectory(destinationFolder);
                File.Copy(selectedFileName, destinationFilePath, true);
                string fullPath = System.IO.Path.GetFullPath(destinationFilePath);
                //  Change Display Image & Variable
                imageName = System.IO.Path.GetFileName(selectedFileName);
                imgDisplay.Source = new BitmapImage(new Uri("/Images/" + imageName,UriKind.Relative));
                MessageBox.Show($"Download of {imageName} Complete, Remember to Include in Folder");
            }
            else
            {
                MessageBox.Show("No file selected.", "Download Cancelled");
            }
        }
    }
}
