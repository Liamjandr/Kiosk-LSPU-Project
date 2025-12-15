using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kiosk
{
    public partial class AddInventory : UserControl
    {
        InventoryItem item;
        public AddInventory(InventoryItem item)
        {
            InitializeComponent();
            this.item = item;
            ItemID.Text = item.ID;
            Type.Text = item.Type;
            Description.Text = item.Description;
            Stock.Text = item.Stock.ToString();
            Cost.Text = item.Price.ToString("₱#,##0.00");
            if (item.isEnable)
            {
                isEnable.Text = "Enabled";
                isEnable.FillColor = Color.Green;
            }
            else
            {
                isEnable.Text = "Disabled";
                isEnable.FillColor = Color.Red;
            }
        }

        private void isEnable_Click(object sender, EventArgs e)
        {
            item.isEnable = !item.isEnable;

            isEnable.Text = item.isEnable ? "Enabled" : "Disabled";
            isEnable.FillColor = item.isEnable ? Color.Green : Color.Red;

            UpdateIsEnabledInDB(Convert.ToInt32(item.ID), item.isEnable);
        }
        private void UpdateIsEnabledInDB(int itemId, bool isEnabled)
        {
            string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                string query = "UPDATE tbitems SET isEnabled = @enabled WHERE ItemID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@enabled", isEnabled ? 1 : 0); // ✅ FIX
                    cmd.Parameters.AddWithValue("@id", itemId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var main = this.FindForm() as Main;
            if (main == null) return;
            main.showInventoryModal(Convert.ToInt32(item.ID), item.Description ,item.Type, Convert.ToDecimal(item.Price), item.Stock,item.ImagePath);
        }

       
        public static void DeleteInventoryItem(int itemId)
        {

            string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();

                string query = "DELETE FROM tbitems WHERE ItemID = @ItemID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.ExecuteNonQuery();
                }
            }

           

        }
        
        private void Delete_Click(object sender, EventArgs e)
        {
            var main = this.FindForm() as Main;
            if (main == null) return;

            DeleteInventoryItem(Convert.ToInt32(item.ID));

            main.refreshAdminInven();

        }
    }

    public class InventoryItem
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public string Size { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }
        public bool isEnable { get; set; }
    }
}
