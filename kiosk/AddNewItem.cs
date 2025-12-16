using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace kiosk
{
    public partial class AddNewItem : Form
    {
        public string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";
        private readonly string itemImagePath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics");

        private FlowLayoutPanel panel; // optional, if you want to remove modal from parent
        private int FormRadius = 20;
        private string imagePath; // store selected image filename

        public AddNewItem()
        {
            InitializeComponent();
            InitializeForm();
        }

        public AddNewItem(FlowLayoutPanel panel)
        {
            InitializeComponent();
            this.panel = panel;
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Rounded corners
            this.Region = RoundRegion(FormRadius);

            // Initialize CheckedListBox
            clbUpdateSizes.Items.Clear();
            clbUpdateSizes.Items.AddRange(new object[] { "S", "M", "L", "XL", "2XL" });

            // Populate item types
            LoadItemTypes();

            // Clear textboxes
            updateItemName.Text = "";
            modaltext.Text = "0"; // default stock
            updateItemPrice.Text = "0.00";
            updateItemImage.Text = "";

            // Hook PictureBox click event for image selection
            updateItemImage.Click += UpdateItemImage_Click;
        }

        // ===================== Rounded corners + border =====================
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = RoundRegion(FormRadius);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderWidth = 2;
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
                    pen.Alignment = PenAlignment.Inset;
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

        // ===================== Load item types into ComboBox =====================
        private void LoadItemTypes()
        {
            updateItemTypeCB.Items.Clear();
            string[] itemTypes = { "SHIRT", "PANTS", "SHORT", "CLOTH/FABRIC" };
            foreach (string type in itemTypes) updateItemTypeCB.Items.Add(type);
            //using (MySqlConnection conn = new MySqlConnection(mycon))
            //{
            //    conn.Open();
            //    string query = "SELECT DISTINCT itemType FROM tbitems"; // or a separate table for types
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

        // ===================== Add item to DB =====================
        private void updateModal_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(updateItemName.Text))
            {
                MessageBox.Show("Please enter item name.");
                return;
            }

            if (clbUpdateSizes.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one size.");
                return;
            }

            try
            {
                string name = updateItemName.Text.Trim();
                string type = updateItemTypeCB.Text.Trim();
                int stock = int.Parse(modaltext.Text);
                decimal price = decimal.Parse(updateItemPrice.Text);
                string sizes = string.Join(",", clbUpdateSizes.CheckedItems.Cast<string>());
                string imageFileName = this.imagePath ?? ""; // selected filename

                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO tbitems (itemName, itemType, itemSize, itemPrice, itemStock, IMAGE_PATH)
                        VALUES (@name, @type, @size, @price, @stock, @image)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@size", sizes);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@stock", stock);
                        cmd.Parameters.AddWithValue("@image", imageFileName);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item added successfully!", "Notification");
                CloseModal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add item: " + ex.Message, "Error");
            }
        }

        // ===================== PictureBox image selection =====================
        private void UpdateItemImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select Item Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = ofd.FileName;
                    string fileName = Path.GetFileName(sourcePath);
                    string destPath = Path.Combine(itemImagePath, fileName);

                    // Copy image if not already exists
                    if (!File.Exists(destPath))
                    {
                        Directory.CreateDirectory(itemImagePath); // ensure folder exists
                        File.Copy(sourcePath, destPath);
                    }

                    this.imagePath = fileName;

                    // Display image in PictureBox
                    updateItemImage.Image = Image.FromFile(destPath);
                    updateItemImage.SizeMode = PictureBoxSizeMode.Zoom;

                    // Optional: show filename in textbox
                    updateItemImage.Text = fileName;
                }
            }
        }

        // ===================== Close modal =====================
        private void CloseModal()
        {
            if (panel != null)
            {
                panel.Controls.Remove(this);
            }
            else
            {
                this.Close();
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
