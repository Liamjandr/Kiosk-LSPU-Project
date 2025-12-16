using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Xml.Linq;


namespace kiosk
{

    public partial class InventoryModal : UserControl
    {



        int itemId;
        string name;
        string type;
        string size;
        decimal price;
        int stock;
        string imagePath;
        private FlowLayoutPanel panel; // store the container panel

      
        private int FormRadius = 20;
        InventoryDB invenDb;
        public string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";
        string[] itemTypes = { "SHIRT", "PANTS", "SHORT", "CLOTH/FABRIC" };
        private readonly string itemImagePath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics");


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            using (GraphicsPath path = new GraphicsPath())
            {
                int r = FormRadius;

                path.StartFigure();
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(Width - r, 0, r, r, 270, 90);
                path.AddArc(Width - r, Height - r, r, r, 0, 90);
                path.AddArc(0, Height - r, r, r, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderWidth = 2; // Thickness of the border
            Color borderColor = Color.Black;

            using (GraphicsPath path = new GraphicsPath())
            {
                int r = FormRadius;

                path.StartFigure();
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(Width - r - 1, 0, r, r, 270, 90);
                path.AddArc(Width - r - 1, Height - r - 1, r, r, 0, 90);
                path.AddArc(0, Height - r - 1, r, r, 90, 90);
                path.CloseFigure();

                using (Pen pen = new Pen(borderColor, borderWidth))
                {
                    pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; // ensures border is inside
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
        private Region RoundRegion(int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return new Region(path);
        }


        public InventoryModal(int itemId, string name, string type,
                         decimal price, int stock, string imageFileName,
                         FlowLayoutPanel panell)
        {
            InitializeComponent();

            this.itemId = itemId;
            this.name = name;
            this.type = type;
            this.price = price;
            this.stock = stock;
            this.imagePath = imageFileName; // filename only
            this.panel = panell;

            // Initialize CheckedListBox
            clbUpdateSizes.Items.Clear();
            clbUpdateSizes.Items.AddRange(new object[] { "S", "M", "L", "XL", "2XL" });

            // Populate ComboBox
            LoadItemTypes();
            updateItemTypeCB.SelectedItem = type;

            // Display values in TextBoxes
            updateItemName.Text = name;
            modaltext.Text = stock.ToString();
            updateItemPrice.Text = price.ToString();
            updateItemImage.Text = imageFileName; // TextBox for filename

            // Load sizes from DB
            string dbSizes = GetSizesFromDB(itemId);
            DisplaySizesToCheckedListBox(dbSizes);

            // Load image into PictureBox
            DisplayItemImage(imageFileName);
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void modaltext_TextChanged(object sender, EventArgs e)
        {
        }

        private void updateModal_Click(object sender, EventArgs e)
        {
            if (clbUpdateSizes.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one size.");
                return;
            }

            try
            {
                int stockValue;
                if (!int.TryParse(modaltext.Text.Trim(), out stockValue))
                {
                    MessageBox.Show("Enter a valid stock value.");
                    return;
                }

                decimal priceValue;
                if (!decimal.TryParse(updateItemPrice.Text.Trim(), out priceValue))
                {
                    MessageBox.Show("Enter a valid decimal price.");
                    return;
                }

                string selectedSizes = GetSelectedSizes();

                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();
                    string query = @"
                UPDATE tbitems
                SET itemName=@name, itemType=@type, itemSize=@size,
                    itemPrice=@price, itemStock=@stock, IMAGE_PATH=@image
                WHERE itemId=@id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", updateItemName.Text.Trim());
                        cmd.Parameters.AddWithValue("@type", updateItemTypeCB.Text.Trim());
                        cmd.Parameters.AddWithValue("@size", selectedSizes);
                        cmd.Parameters.Add("@price", MySqlDbType.Decimal).Value = priceValue;
                        cmd.Parameters.AddWithValue("@stock", stockValue);
                        cmd.Parameters.AddWithValue("@image", updateItemImage.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", itemId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("UPDATED!", "Notification");
                this.Parent.Controls.Remove(this);
                CloseModal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message, "Warning");
            }
        }



        private void aaa_TextChanged(object sender, EventArgs e)
        {

        }

        //loading sizes to checkedlistbox (for updating) --------------------------------------
        private void LoadSizes(string dbSizes)
        {
            // Clear previous checks
            for (int i = 0; i < clbUpdateSizes.Items.Count; i++)
                clbUpdateSizes.SetItemChecked(i, false);

            if (string.IsNullOrWhiteSpace(dbSizes)) return;

            string[] sizes = dbSizes.Split(',');

            foreach (string s in sizes)
            {
                for (int i = 0; i < clbUpdateSizes.Items.Count; i++)
                {
                    if (clbUpdateSizes.Items[i].ToString().Equals(s.Trim()))
                    {
                        clbUpdateSizes.SetItemChecked(i, true);
                        break;
                    }
                }
            }
        }


        // Get currently selected sizes for update
        private string GetSelectedSizes()
        {
            return string.Join(",",
                clbUpdateSizes.CheckedItems.Cast<string>().Select(s => s.Trim())
            );
        }

        private void CloseModal()
        {
            if (panel != null)
                panel.Controls.Remove(this);
        }
        private string SortSizes(string sizes)
        {
            string[] order = { "S", "M", "L", "XL", "2XL" };

            return string.Join(",",
                sizes.Split(',')
                     .OrderBy(s => Array.IndexOf(order, s.Trim()))
            );
        }

        // Helper to check boxes in CheckedListBox based on DB value
        private void DisplaySizesToCheckedListBox(string size)
        {
            if (string.IsNullOrWhiteSpace(size)) return;

            string[] sizes = size.Split(',');

            for (int i = 0; i < clbUpdateSizes.Items.Count; i++)
                clbUpdateSizes.SetItemChecked(i, false); // clear previous checks

            foreach (string s in sizes)
            {
                for (int i = 0; i < clbUpdateSizes.Items.Count; i++)
                {
                    if (clbUpdateSizes.Items[i].ToString().Equals(s.Trim()))
                    {
                        clbUpdateSizes.SetItemChecked(i, true);
                        break;
                    }
                }
            }
        }

        private string GetSizesFromDB(int itemId)
        {
            string sizes = "";
            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                string query = "SELECT itemSize FROM tbitems WHERE itemId=@id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", itemId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        sizes = result.ToString();
                }
            }
            return sizes;
        }

        
        //loading item type to combobox (for updating) --------------------------------------
        private void LoadItemTypes()
        {
            updateItemTypeCB.Items.Clear();
            foreach(string type in itemTypes) updateItemTypeCB.Items.Add(type);

            //using (MySqlConnection conn = new MySqlConnection(mycon))
            //{
            //    conn.Open();
            //    string query = "SELECT DISTINCT itemType FROM tbitems"; // or from a separate types table
            //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
            //    {
            //        using (MySqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                updateItemTypeCB.Items.Add(reader.GetString("itemType"));
            //            }
            //        }
            //    }
            //}
        }

        //loading item image to pictuerbox (for updating) --------------------------------------


        private void DisplayItemImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string fullPath = Path.Combine(itemImagePath, fileName);
                if (File.Exists(fullPath))
                {
                    updateItemImage.Image = Image.FromFile(fullPath); // PictureBox
                    updateItemImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    updateItemImage.Image = null;
                }
            }
            else
            {
                updateItemImage.Image = null;
            }
        }

        private void updateItemImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select Item Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = ofd.FileName; // full path of selected file
                    string fileName = Path.GetFileName(sourcePath); // get filename only
                    string destPath = Path.Combine(itemImagePath, fileName); // destination in itemPics

                    // Copy image to itemPics if it doesn't already exist
                    if (!File.Exists(destPath))
                    {
                        File.Copy(sourcePath, destPath);
                    }

                    // Store only the filename in your class variable (to update DB later)
                    this.imagePath = fileName;

                    // Update PictureBox
                    updateItemImage.Image = Image.FromFile(destPath);
                    updateItemImage.SizeMode = PictureBoxSizeMode.Zoom;

                    // Optionally update the TextBox if you have one
                    updateItemImage.Text = fileName;
                }
            }
        }
    }
}
