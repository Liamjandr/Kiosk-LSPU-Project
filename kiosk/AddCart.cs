using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using static kiosk.randomData;

namespace kiosk
{

    public partial class AddCart : Form
    {

        string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";

        // -----------------------------
        // DWM API for shadow
        // -----------------------------
        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }
        private Main mainForm;
        public AddCart(Main m)
        {
            InitializeComponent();
            mainForm = m;
            // Apply shadow effect
            this.FormBorderStyle = FormBorderStyle.None;  // Remove standard border
            this.BackColor = Color.White;
            this.Padding = new Padding(1); // small padding for visible shadow


        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Apply DWM shadow
            int v = 2; // DWMWA_NCRENDERING_POLICY
            DwmSetWindowAttribute(this.Handle, 2, ref v, sizeof(int));

            MARGINS margins = new MARGINS()
            {
                Left = 20,   // bigger values = bigger shadow
                Right = 20,
                Top = 20,
                Bottom = 20
            };
            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }

        // Allow dragging for borderless form
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;

            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
            {
                m.Result = (IntPtr)HTCAPTION;
            }


            const int WM_NCLBUTTONDBLCLK = 0xA3; // Non-client area double-click (title bar)

            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                // Prevent maximize toggle on double-click
                return; // swallow the message
            }

            base.WndProc(ref m);
        }


       


        private void itemload()
        {









        }


        private void AddCart_Load(object sender, EventArgs e)
        {
            itemload();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();

            // Reset radio buttons
            sBtn.Checked = false;
            mBtn.Checked = false;
            lBtn.Checked = false;
            xBtn.Checked = false;
            xxBtn.Checked = false;

            qty.Text = "1";
            this.Refresh();
        }


        private void incr_Click(object sender, EventArgs e)
        {
         
            int quantity = 0;
            int.TryParse(qty.Text, out quantity);

            quantity++;          
            qty.Text = quantity.ToString();
        }

        private void decr_Click(object sender, EventArgs e)
        {
          
            int quantity = 0;
            int.TryParse(qty.Text, out quantity);

            if (quantity > 0)
            {
                quantity--;     
            }

            qty.Text = quantity.ToString();
        }

        ///  ------------------------------------------------------------------------------- TO SUBTRACT THE CHOSEN QUANTITY FROM THE PRODUCT STOCK IN DB
        private void SubtractStock(int itemId, int quantityPurchased)
        {
            string query = "UPDATE tbitems SET itemStock = itemStock - @qty WHERE itemId = @id";

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@qty", quantityPurchased);
                    cmd.Parameters.AddWithValue("@id", itemId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

      


        public int cartCounter = 0;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // =============================
            // 1. GATHER DATA
            // =============================
            string ProductID = prodID.Text;
            string ProductName = prodName.Text;
            string SubProductName = subName.Text;

            string SelectedSize = "";
            if (sBtn.Checked) SelectedSize = "S";
            else if (mBtn.Checked) SelectedSize = "M";
            else if (lBtn.Checked) SelectedSize = "L";
            else if (xBtn.Checked) SelectedSize = "XL";
            else if (xxBtn.Checked) SelectedSize = "2XL";

            if (SelectedSize == "")
            {
                MessageBox.Show("Please select a size.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(qty.Text, out int Quantity) || Quantity <= 0)
            {
                MessageBox.Show("Invalid quantity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(stock.Text, out int Stock))
            {
                MessageBox.Show("Invalid stock.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Quantity > Stock)
            {
                MessageBox.Show("Quantity exceeds stock.", "Warning");
                return;
            }

            double Price = double.Parse(price.Text);

            // =============================
            // 2. SUBTRACT STOCK IN DATABASE
            // =============================
            SubtractStock(int.Parse(ProductID), Quantity);


            // =============================
            // 3. UPDATE UI SLOTS IN MAIN FORM
            // =============================
            Label[] productID = { mainForm.firstID, mainForm.secondID, mainForm.thirdID, mainForm.fourthID, mainForm.fifthID };
            PictureBox[] cartPics = { mainForm.firstPic, mainForm.secondPic, mainForm.thirdPic, mainForm.fourthPic, mainForm.fifthPic };
            Panel[] picsPanel = { mainForm.firstItem, mainForm.secondItem, mainForm.thirdItem, mainForm.fourthItem, mainForm.fifthItem };
            Label[] productNameLbl = { mainForm.firstItemName, mainForm.secondItemName, mainForm.thirdItemName, mainForm.fourthItemName, mainForm.fifthItemName };
            Label[] prodType = { mainForm.firstItemType, mainForm.secondItemType, mainForm.thirdItemType, mainForm.fourthItemType, mainForm.fifthItemType };
            Label[] prodQty = { mainForm.firstItemQty, mainForm.secondItemQty, mainForm.thirdItemQty, mainForm.fourthItemQty, mainForm.fifthItemQty };
            Label[] prodSize = { mainForm.firstItemSize, mainForm.secondItemSize, mainForm.thirdItemSize, mainForm.fourthItemSize, mainForm.fifthItemSize };
            Label[] prodPrice = { mainForm.firstItemPrice, mainForm.secondItemPrice, mainForm.thirdItemPrice, mainForm.fourthItemPrice, mainForm.fifthItemPrice };

            if (mainForm.cartCounter < cartPics.Length)
            {
                int i = mainForm.cartCounter;

                productID[i].Text = ProductID.ToString();
                picsPanel[i].Visible = true;
                cartPics[i].Image = prodIMG.Image;
                productNameLbl[i].Text = ProductName;
                prodType[i].Text = SubProductName;
                prodQty[i].Text = Quantity.ToString();
                prodSize[i].Text = SelectedSize;
                prodPrice[i].Text = Price.ToString();

                mainForm.ttext.Visible = false;
                mainForm.confirmBtn.Enabled = true;

                // =============================
                // 4. ADD TO CART LIST (for receipt + reset)
                // =============================
                mainForm.cartItems.Add(new CartItem
                {
                    ItemID = int.Parse(ProductID),
                    Name = ProductName,
                    Type = SubProductName,
                    Size = SelectedSize,
                    Quantity = Quantity,
                    Price = (decimal)Price
                });

                mainForm.cartCounter++;
            }
            else
            {
                MessageBox.Show("Cart is full!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Reset UI
            sBtn.Checked = mBtn.Checked = lBtn.Checked = xBtn.Checked = xxBtn.Checked = false;
            qty.Text = "1";

            // Update main form
            mainForm.LoadAllItemsPage(mainForm.currentPage);
            mainForm.LoadStockStatus();

            // Properly remove AddCart
            this.Close();
        }


    }
}

