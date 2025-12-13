using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace kiosk
{
    public partial class InventoryModal : UserControl
    {
        int stock;
        int itemId;
        private int FormRadius = 20;
        InventoryDB invenDb;
        FlowLayoutPanel panel;
        public string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";
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

        public InventoryModal(int stock, int itemId, FlowLayoutPanel panell)
        {
            InitializeComponent();
            this.stock = stock;
            this.itemId = itemId;
            this.panel = panell;
            this.DoubleBuffered = true;
            int radius = 40;
            this.Region = RoundRegion(radius);
            modaltext.Text = stock.ToString();
            //label1.Text = itemId.ToString();
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
            try
            {
                int stock = int.Parse(modaltext.Text);
                //int ahhaha = int.Parse(label1.Text);
                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();

                    string query = @"
                        UPDATE tbitems
                        SET itemStock = @stock
                        WHERE itemId = @itemId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@stock", stock);
                        cmd.Parameters.AddWithValue("@itemId", itemId);

                        int rows = cmd.ExecuteNonQuery();
                        //MessageBox.Show("Updated rows: " + rows);
                    }
                }
                invenDb = new InventoryDB();
                invenDb.Table(panel);
                this.Parent.Controls.Remove(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Input: " + ex.Message);
                return;
            }
        }

        private void aaa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
