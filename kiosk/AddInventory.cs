using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Cost.Text = item.Price.ToString("C2");
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
    }
}
