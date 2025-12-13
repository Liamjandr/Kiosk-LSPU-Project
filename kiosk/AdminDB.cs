using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kiosk
{



    //adminDB
    //imteTable
    //historyTable
    //ItemUpdate
    //HistoryItem

    public abstract class AdminDB 
    {
        public string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";
        
        abstract public void Table(FlowLayoutPanel panel);
        abstract public void TableSort(Guna2TabControl admin_tabControl, TabPage CurrentPage, Guna2ComboBox adminSort, FlowLayoutPanel CurrentFlowPanel);
    }

    public class InventoryDB : AdminDB
    {

        private List<AddInventory> Inventory = new List<AddInventory>();
        public override void Table(FlowLayoutPanel panel)
        {
            panel.Controls.Clear();
            Inventory.Clear();
            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                try
                {
                    conn.Open();
                    //Inventory Table
                    string itemQuery = "SELECT * FROM tbitems ORDER BY itemId ASC";
                    MySqlCommand itemCmd = new MySqlCommand(itemQuery, conn);
                    using (MySqlDataReader reader = itemCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Inventory.Add(
                                new AddInventory(
                                    new InventoryItem
                                    {
                                        ID = reader.GetInt32("itemId").ToString(),
                                        Type = reader.GetString("itemType"),
                                        Description = reader.GetString("itemName"),
                                        Price = reader.GetInt32("itemPrice"),
                                        Stock = reader.GetInt32("itemStock"),
                                        ImagePath = reader.GetString("IMAGE_PATH")
                                    }
                                )
                            );
                            //Image img = File.Exists(fullPath) ? Image.FromFile(fullPath) : null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }

            foreach (AddInventory item in Inventory)
            {
                panel.Controls.Add(item);
            }
        }

        public override void TableSort(Guna2TabControl admin_tabControl, TabPage CurrentPage, Guna2ComboBox adminSort, FlowLayoutPanel CurrentFlowPanel)
        {
            List<AddInventory> inventoryByDescription = Inventory.OrderBy(i => i.Description.Text).ToList();
            List<AddInventory> inventoryByCost = Inventory.OrderBy(i => i.Cost.Text).ToList();
            List<AddInventory> inventoryByStock = Inventory.OrderBy(i => Convert.ToInt32(i.Stock.Text)).ToList();

            if (admin_tabControl.SelectedTab == CurrentPage)
                switch (adminSort.SelectedItem.ToString())
                {
                    case "Default":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (AddInventory item in Inventory) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "Alphabetical (A-Z)":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (AddInventory item in inventoryByDescription) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "Alphabetical (Z-A)":
                        CurrentFlowPanel.Controls.Clear();
                        inventoryByDescription.Reverse();
                        foreach (AddInventory item in inventoryByDescription) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "By Cost (Highest)":
                        CurrentFlowPanel.Controls.Clear();
                        inventoryByCost.Reverse();
                        foreach (AddInventory item in inventoryByCost) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "By Cost (Lowest)":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (AddInventory item in inventoryByCost) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "By Stock (Highest)":
                        CurrentFlowPanel.Controls.Clear();
                        inventoryByStock.Reverse();
                        foreach (AddInventory item in inventoryByStock) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "By Stock (Lowest)":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (AddInventory item in inventoryByStock) CurrentFlowPanel.Controls.Add(item);
                        break;
                    //case "":
                    //    break;
                    default:
                        break;
                }
        }

        public int getInventory()
        {
            int totalQTY = 0;
            foreach (AddInventory inven in Inventory) totalQTY += Convert.ToInt32(inven.Stock.Text);

            return totalQTY;
        }
    }

    public class ReceiptDB : AdminDB
    {
        //List<HistoryDB> historyDB = new List<HistoryDB>();
        private List<addPurchase> receiptHistory = new List<addPurchase>();
        Dictionary<string, ReceiptGroup> receiptGroup = new Dictionary<string, ReceiptGroup>();

        public override void Table(FlowLayoutPanel panel)
        {
            panel.Controls.Clear();
            receiptHistory.Clear();

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                try
                {
                    conn.Open();
                    //Purchase History Table
                    string historyQuery = "SELECT * FROM tbhistory ORDER BY ReceiptID ASC";
                    MySqlCommand historyCmd = new MySqlCommand(historyQuery, conn);

                    using (MySqlDataReader readHistory = historyCmd.ExecuteReader())
                    {
                        while (readHistory.Read())
                        {
                            string receiptID = readHistory.GetString("ReceiptID");

                            if (!receiptGroup.ContainsKey(receiptID))
                            {
                                receiptGroup[receiptID] = new ReceiptGroup
                                {
                                    ReceiptID = receiptID,
                                    ReceiptDate = readHistory.GetDateTime("DateTime"),
                                    Transaction = readHistory.GetString("Transaction"),
                                    Items = new List<HistoryDB>()
                                };
                            }
                            receiptGroup[receiptID].Items.Add(
                                new HistoryDB
                                {
                                    ItemID = readHistory.GetInt32("ItemID"),
                                    ImgPath = readHistory.GetString("itemImage"),
                                    Name = readHistory.GetString("itemName"),
                                    Type = readHistory.GetString("itemType"),
                                    Size = readHistory.GetString("itemSize"),
                                    Quantity = readHistory.GetInt32("itemQTY"),
                                    Price = readHistory.GetDecimal("itemPrice"),
                                    TotalAmount = readHistory.GetDecimal("Total"),
                                    Cash = readHistory.GetDecimal("Cash"),
                                    Change = readHistory.GetDecimal("Change")
                                }
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }



            foreach (var receipt in receiptGroup.Values.OrderBy(g => g.ReceiptID))
            {
                addPurchase control = new addPurchase(receipt);
                receiptHistory.Add(control);
                panel.Controls.Add(control);
            }
            //foreach (HistoryDB history in historyDB) Console.WriteLine(history.receiptID);

        }

        public override void TableSort(Guna2TabControl admin_tabControl, TabPage CurrentPage, Guna2ComboBox adminSort, FlowLayoutPanel CurrentFlowPanel)
        {
            List<addPurchase> HistoryByDescription = receiptHistory.OrderBy(i => i.Description.Text).ToList();
            List<addPurchase> HistoryByCost = receiptHistory.OrderBy(i => i.Cost.Text).ToList();

            if (admin_tabControl.SelectedTab == CurrentPage)
                switch (adminSort.SelectedItem.ToString())
                {
                    case "Default":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (addPurchase item in receiptHistory) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "Alphabetical (A-Z)":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (addPurchase item in HistoryByDescription) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "Alphabetical (Z-A)":
                        CurrentFlowPanel.Controls.Clear();
                        HistoryByDescription.Reverse();
                        foreach (addPurchase item in HistoryByDescription) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "By Cost (Highest)":
                        CurrentFlowPanel.Controls.Clear();
                        HistoryByCost.Reverse();
                        foreach (addPurchase item in HistoryByCost) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "By Cost (Lowest)":
                        CurrentFlowPanel.Controls.Clear();
                        foreach (addPurchase item in HistoryByCost) CurrentFlowPanel.Controls.Add(item);
                        break;
                    case "":
                        break;
                    default:
                        break;
                }
        }


        public decimal totalSale()
        {
            decimal total = 0;
            foreach (var receipt in receiptGroup.Values)
            {
                foreach (var item in receipt.Items)
                {
                    total += item.Price * item.Quantity;
                }
            }
            return total;
        }

        public int Paid()
        {
            int total = 0;
            foreach (var receipt in receiptGroup.Values)
            {
                foreach (var item in receipt.Items)
                {
                    if (item.isPaid) total++;
                }
            }
            return total;
        }

        public int NotPaid()
        {
            int total = 0;
            foreach (var receipt in receiptGroup.Values)
            {
                foreach (var item in receipt.Items)
                {
                    if (!item.isPaid) total++;
                }
            }
            return total;
        }

        public int Claimed()
        {
            int total = 0;
            foreach (var receipt in receiptGroup.Values)
            {
                foreach (var item in receipt.Items)
                {
                    if (item.isClaimed) total++;
                }
            }
            return total;
        }

        public int NotClaimed()
        {
            int total = 0;
            foreach (var receipt in receiptGroup.Values)
            {
                foreach (var item in receipt.Items)
                {
                    if (!item.isClaimed) total++;
                }
            }
            return total;
        }
    }

    public class ItemDB 
    { 
        
    }

    public class ReceiptGroup
    {
        public string ReceiptID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Transaction { get; set; }
        public List<HistoryDB> Items { get; set; }
    }

    public class HistoryDB 
    {
        //Item Info
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string ImgPath { get; set; }

        //Transaction Info
        public decimal TotalAmount { get; set; }
        public decimal Cash { get; set; }
        public decimal Change { get; set; }

        public Boolean isPaid = false;
        public Boolean isClaimed = false;
    }
}
