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
            string productName = tbxName.Text;
            string productDescription = tbxDescription.Text;

            ProductInfo newProduct = new ProductInfo() { name = productName, description = productDescription, image="/Images/" + imageName};
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

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set properties of the OpenFileDialog
            openFileDialog.Title = "Select a PNG file to download";
            openFileDialog.Filter = "PNG Files (*.png)|*.png"; // Allow only PNG files

            // Show the OpenFileDialog and check if the user clicked OK
            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file name and path
                string selectedFileName = openFileDialog.FileName;

                // Get the base directory of the application
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Navigate two levels up from the base directory to get the solution directory
                string solutionDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory).FullName).FullName).FullName;

                // Define the destination folder relative to the solution directory
                string destinationFolder = System.IO.Path.Combine(solutionDirectory, "Images"); // Change "Images" to your desired relative destination folder

                // Construct the destination file path
                string destinationFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(selectedFileName));

                // Create the destination folder if it doesn't exist
                Directory.CreateDirectory(destinationFolder);

                // Copy the selected file to the destination folder
                File.Copy(selectedFileName, destinationFilePath, true); // "true" to overwrite if the file already exists

                // Get the full path of the downloaded file
                string fullPath = System.IO.Path.GetFullPath(destinationFilePath);

                // Show a message to indicate that the file was downloaded
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
