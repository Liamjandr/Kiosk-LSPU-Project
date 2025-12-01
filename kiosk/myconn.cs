using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace kiosk
{
    internal class myconn
    {
        public MySqlConnection con;
        public MySqlDataReader dr;
        public MySqlCommand cmd;
        public DataTable dt;

        public void connect()
        {
            con = new MySqlConnection("datasource=localhost;Database=dbkiosk;username=root");
            con.Open();
        }

        public void Disconnect()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
            con.Dispose();
        }

        //private static string mycon = "datasource=localhost;Database=dbkiosk;username=root";
        //private static string queryItems = "SELECT * FROM tbitems ORDER BY itemId ASC";
        //int id = reader.GetInt32("itemId");
        //string name = reader.GetString("itemName");
        //int stock = reader.GetInt32("itemStock");
        //string type = reader.GetString("itemType").ToLower(); // shirt, pants, fabric, other
        //string imgFileName = reader.GetString("IMAGE_PATH");
        //string fullPath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics", imgFileName);

        //public static async Task loadInventory()
        //{
        //    using (MySqlConnection conn = new MySqlConnection(mycon))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            MySqlCommand cmd = new MySqlCommand(queryItems, conn);
        //            using (MySqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    // Load item info
        //                    //ac.prodName.Text = reader["itemName"].ToString();
        //                    //ac.subName.Text = reader["itemType"].ToString();
        //                    //ac.price.Text = reader["itemPrice"].ToString();
        //                    //ac.stock.Text = reader["itemStock"].ToString();

        //                    // to set unavailable img


        //                    // Load and process sizes
        //                    //string sizeData = reader["itemSize"].ToString();
        //                    //List<string> availableSizes = sizeData.Split(',').ToList();



        //                    // Enable/disable radio buttons
        //                    //ac.sBtn.Enabled = availableSizes.Contains("S");
        //                    //ac.mBtn.Enabled = availableSizes.Contains("M");
        //                    //ac.lBtn.Enabled = availableSizes.Contains("L");
        //                    //ac.xBtn.Enabled = availableSizes.Contains("XL");
        //                    //ac.xxBtn.Enabled = availableSizes.Contains("2XL");


        //                    //ac.ShowDialog();
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Item not found in the database.");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Database error: " + ex.Message);
        //        }
        //    }
        //}

        //---------------------------------------------------
    }
}
