using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace App_Test
{
    /// <summary>
    /// Interaction logic for ProductAll.xaml
    /// </summary>
    public partial class ProductAll : Window
    {
        public ProductAll()
        {
            InitializeComponent();
        }
        public static int id;
        public static string name = "";
        public static decimal price = decimal.Zero;
        public static int quantity = 0;
        public static string color = "";
        public static string category = "";
        public static string isstock = "";
        public static string image = "";
        public static string desc = "";

        private void GetItemList()
        {
            var json = File.ReadAllText(@"products.json");
            var jObject = JObject.Parse(json);
            if (jObject != null)
            {
                JArray products = (JArray)jObject["Products"];
                if (products != null)
                {
                    List<Models.Items> pro = new List<Models.Items>();
                    foreach (var item in products)
                    {
                        pro.Add(new Models.Items() { Id = Convert.ToInt32(item["Id"]), Name = item["Name"].ToString(), Price = Convert.ToDecimal(item["Price"]), Quantity = Convert.ToInt32(item["Quantity"]), Color = item["Color"].ToString(), Category = item["Category"].ToString(), Image = item["Image"].ToString(), Description = item["Description"].ToString() });

                    }
                    lvProducts.ItemsSource = pro;
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetItemList();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var itemsJson = File.ReadAllText(@"products.json");
            try
            {
                var jObject = JObject.Parse(itemsJson);
                JArray itemArr = (JArray)jObject["Products"];
                Button button = sender as Button;
                Models.Items itemId = button.CommandParameter as Models.Items;
                int iId = itemId.Id;
                var itemToDelete = itemArr.FirstOrDefault(obj => obj["Id"].Value<int>() == iId);

                MessageBox.Show("Are you sure to delete this product?");
                itemArr.Remove(itemToDelete);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(@"products.json", output);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            finally
            {
                MessageBox.Show("Data deleted successfully!!!");
                GetItemList();
            }
        }
    }
}
