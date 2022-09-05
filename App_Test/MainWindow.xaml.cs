using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Path = System.IO.Path;

namespace App_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> category = new List<string>()
            {
                "----Select Category------",
                "Electronic",
                "Homedecor",
                "Outdoor"
            };
            List<string> color = new List<string>()
            {
                "---Select Color----",
                "Black",
                "White",
                "Silver"
            };
            cmbCategory.ItemsSource = category;
            cmbCategory.Text = "---Select Category-----";
            cmbColor.ItemsSource = color;
            cmbColor.Text = "----Select Color------";
        }

        private void btnImageUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                itemPic.Source = new BitmapImage(new Uri(ofd.FileName));
                //var tmpFileName = DateTime.Now.Minute + "-" + DateTime.Now.Second + Path.GetExtension(ofd.FileName);
                var tmpFileName = Guid.NewGuid() + Path.GetExtension(ofd.FileName);
                txtFilePath.Text = tmpFileName;
                var imagePath = System.IO.Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../..Assets\Images\") + tmpFileName);
                //File.Copy(ofd.FileName, imagePath);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Items i = new Models.Items();
                i.Id = Convert.ToInt32(txtId.Text);
                i.Name = txtName.Text;
                i.Price = Convert.ToDecimal(txtPrice.Text);
                i.Quantity = Convert.ToInt32(txtQuantity.Text);
                i.Color = cmbColor.Text;
                i.Category = cmbCategory.Text;
                i.Image = txtFilePath.Text;
                i.Description = txtDescription.Text;
                if (rdInStock.IsChecked == true)
                {
                    i.IsStock = "In Stock";
                }
                if (rdOutOfStock.IsChecked == true)
                {
                    i.IsStock = "Out of Stock";
                }
                var newItem = "{'Id':'"+i.Id+"','Name':'"+i.Name+"','Price':'"+i.Price+"','Quantity':'"+i.Quantity+"','Color':'"+i.Color+"','Category':'"+i.Category+"','Image':'"+i.Image+"','InStock':'"+i.IsStock+"','Description':'"+i.Description+"'}";

                var itemJson = File.ReadAllText(@"products.json");
                var jsonObject = JObject.Parse(itemJson);
                var itemArr = jsonObject.GetValue("Products") as JArray;
                var product = JObject.Parse(newItem);
                itemArr.Add(product);

                jsonObject["Products"] = itemArr;
                string jsonResult = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                File.WriteAllText(@"products.json", jsonResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message.ToString());
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            ProductAll pa = new ProductAll();
            pa.Show();
            this.Close();
        }
    }
}
