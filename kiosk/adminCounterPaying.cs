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
    public partial class adminCounterPaying : UserControl
    {
        private string receiptID;
        private decimal totalAmount;
        private string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";


        private int FormRadius = 20;
        public event Action PaymentConfirmed; // callback to parent

        public adminCounterPaying(string receiptID)
        {
            InitializeComponent();
            this.receiptID = receiptID;

            // Fetch total from database
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();
                    string query = "SELECT Total FROM tbhistory WHERE ReceiptID = @receiptID AND Transaction = 'CASH/COUNTER' LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptID", receiptID);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            totalAmount = Convert.ToDecimal(result);
                            totalCost.Text = "₱" + totalAmount.ToString("F2");
                        }
                        else
                        {
                            MessageBox.Show("Failed to fetch total from database.");
                            totalAmount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
                totalAmount = 0;
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
        private void adminCounterPaying_Load(object sender, EventArgs e)
        {
            this.Region = RoundRegion(20); // Rounded corners
            Cash.Focus();

            Cash.TextChanged += txtCash_TextChanged;
            Cash.KeyPress += txtCash_KeyPress;

            // Enter key submits payment
            Cash.KeyPress += (s, ev) =>
            {
                if (ev.KeyChar == (char)Keys.Enter)
                {
                    paid_Click(s, ev);
                    ev.Handled = true;
                }
            };
        }

        // Live update of change
        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            decimal cash;
            if (decimal.TryParse(Cash.Text, out cash))
            {
                // Update change textbox
                if (cash >= totalAmount)
                {
                    decimal change = cash - totalAmount;
                    Change.Text = "₱" + change.ToString("F2");
                }
                else
                {
                    Change.Text = "₱0.00";
                }

            
            }
            else
            {
                Change.Text = "₱0.00"; // invalid input
            }
        }

        // Only allow numbers and one decimal point
        private void txtCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return; // allow backspace
            if (char.IsDigit(e.KeyChar)) return;   // allow digits
            if (e.KeyChar == '.' && !Cash.Text.Contains(".")) return; // allow one decimal
            e.Handled = true; // block others
        }

        private void paid_Click(object sender, EventArgs e)
        {
            decimal cash;
            if (!decimal.TryParse(Cash.Text, out cash))
            {
                MessageBox.Show("Invalid cash input.");
                return;
            }

            if (cash < totalAmount)
            {
                MessageBox.Show("Cash is less than the total. Please enter sufficient amount.");
                return;
            }

            decimal change = cash - totalAmount;
            Change.Text = "₱" + change.ToString("F2");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();
                    string query = @"
                        UPDATE tbhistory
                        SET isPaid = 'true', Cash = @cash, `Change` = @change
                        WHERE ReceiptID = @receiptID
                        AND Transaction = 'CASH/COUNTER'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptID", receiptID);
                        cmd.Parameters.AddWithValue("@cash", cash);
                        cmd.Parameters.AddWithValue("@change", change);
                        cmd.ExecuteNonQuery();
                    }
                }

                PaymentConfirmed?.Invoke(); // notify parent
                MessageBox.Show("Payment successful!");

                // Close modal by removing from parent
                this.Parent?.Controls.Remove(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Payment failed: " + ex.Message);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            // Close modal when clicking the picture (X)
            this.Parent?.Controls.Remove(this);
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
