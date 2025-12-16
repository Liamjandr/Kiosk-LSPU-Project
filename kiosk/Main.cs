using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;
using static kiosk.randomData;
using static QRCoder.PayloadGenerator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace kiosk 
{

    public partial class Main : Form
    {

        //for running admin only
        public void OpenAdminTab()
        {
            tabControl1.SelectedTab = Admin;
        }

        private readonly Timer _inactivityTimer = new Timer();
        private const int InactivityMs = 5_000; //5sec

        private TabPage _homeTab;     // e.g., tabPageHome
        private TabPage _targetTab;   // e.g., tabPage2


        public string paymentMethod = "";  

        private readonly string itemImagePath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics" );

        string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";

        public List<CartItem> cartItems = new List<CartItem>();
        //public studentInfo studentInfoObject;

        // ALL ITEMS
        List<Guna2PictureBox> all_itemPics = new List<Guna2PictureBox>();
        List<Guna2PictureBox> all_overlays = new List<Guna2PictureBox>();
        List<Guna2ShadowPanel> all_itemPanels = new List<Guna2ShadowPanel>();
        List<Label> all_itemLabel = new List<Label>();

        // TOP — shirts
        List<Guna2PictureBox> top_itemPics = new List<Guna2PictureBox>();
        List<Label> top_itemLabel = new List<Label>();
        List<Guna2PictureBox> top_overlays = new List<Guna2PictureBox>();
        List<Guna2ShadowPanel> top_itemPanels = new List<Guna2ShadowPanel>();

        // BOTTOM — pants
        List<Guna2PictureBox> bot_itemPics = new List<Guna2PictureBox>();
        List<Label> bot_itemLabel = new List<Label>();
        List<Guna2PictureBox> bot_overlays = new List<Guna2PictureBox>();
        List<Guna2ShadowPanel> bot_itemPanels = new List<Guna2ShadowPanel>();

        // FABRIC
        List<Guna2PictureBox> fab_itemPics = new List<Guna2PictureBox>();
        List<Label> fab_itemLabel = new List<Label>();
        List<Guna2PictureBox> fab_overlays = new List<Guna2PictureBox>();
        List<Guna2ShadowPanel> fab_itemPanels = new List<Guna2ShadowPanel>();

        // OTHERS
        List<Guna2PictureBox> other_itemPics = new List<Guna2PictureBox>();
        List<Label> other_itemLabel = new List<Label>();
        List<Guna2PictureBox> other_overlays = new List<Guna2PictureBox>();
        List<Guna2ShadowPanel> other_itemPanels = new List<Guna2ShadowPanel>();

        //all items 2nd
        List<Guna2PictureBox> all_itemPics2nd = new List<Guna2PictureBox>();
        List<Guna2PictureBox> all_overlays2nd = new List<Guna2PictureBox>();
        List<Guna2ShadowPanel> all_itemPanels2nd = new List<Guna2ShadowPanel>();
        List<Label> all_itemLabel2nd = new List<Label>();


        private List<Guna2PictureBox> allPicsCombined;
        private List<Label> allLabelsCombined;
        private List<Guna2PictureBox> allOverlaysCombined;
        private List<Guna2ShadowPanel> allPanelsCombined;

        AddCart ac;

        //--------------------------------- Admin Section ---------------------------------
        InventoryDB inventoryDB = new InventoryDB();
        ReceiptDB receiptDB = new ReceiptDB();

        //Admin Inventory
        List<AddInventory> Inventory  = new List<AddInventory>();
        //Admin Receipts
        receiptTemplate receiptData = new receiptTemplate();
        createPDF createPDF = new createPDF();
        ReceiptModal receiptModal;
        InventoryModal inventoryModal;
        //
        WebView2 webPanel;

        public int cartCounter = 0;
        public Main()
        {
            InitializeComponent();
            adminIntialize();

       

            allSeventh_Overlay.Parent = all_itemPicSeventh;    
            allSeventh_Overlay.BackColor = Color.Transparent;
            allSeventh_Overlay.BringToFront();






            _homeTab = tabPage1;
            _targetTab = tabPage2;

            _inactivityTimer.Interval = InactivityMs;
            _inactivityTimer.Tick += (s, e) => GoHome();

            tabControl1.SelectedIndexChanged += (s, e) =>
            {
                if (tabControl1.SelectedTab == _targetTab)
                {
                    _inactivityTimer.Stop();
                    _inactivityTimer.Start(); // only start when TabPage2 is shown
                }
                else
                {
                    _inactivityTimer.Stop();  // stop on any other tab
                }
            };

            // Reset timer only on TabPage2 interactions (WinForms controls)
            HookPage(_targetTab);


            //Avoid pixelate loading
            //this.DoubleBuffered = true;
            //SetStyle(ControlStyles.AllPaintingInWmPaint |
            //         ControlStyles.UserPaint |
            //         ControlStyles.OptimizedDoubleBuffer, true);
            //UpdateStyles();


        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}
        //public class SmoothPanel : Panel
        //{
        //    public SmoothPanel()
        //    {
        //        this.DoubleBuffered = true;
        //        this.ResizeRedraw = true;
        //    }
        //}



        //---------- part of inactive timer
        private void HookPage(Control page)
        {
            page.MouseMove += (_, __) => ResetIfOnTarget();
            page.MouseDown += (_, __) => ResetIfOnTarget();
            page.KeyDown += (_, __) => ResetIfOnTarget();

            foreach (Control child in page.Controls)
            {
                child.MouseMove += (_, __) => ResetIfOnTarget();
                child.MouseDown += (_, __) => ResetIfOnTarget();
                child.KeyDown += (_, __) => ResetIfOnTarget();
            }
        }

        private void ResetIfOnTarget()
        {
            if (tabControl1.SelectedTab == _targetTab)
            {
                _inactivityTimer.Stop();
                _inactivityTimer.Start();
            }
        }

        private void GoHome()
        {
            _inactivityTimer.Stop();
            tabControl1.SelectedTab = _homeTab;
        }
   
    //-------------------------------


private void Main_Load(object sender, EventArgs e)
{
    ac = new AddCart(this);

    // =======================
    // SETUP
    // =======================
    SetupItemLists();
    ResetCart();

            LoadTotalItemCount();
            LoadAllItemsPage(currentPage);
            // =======================
            // LOAD CATEGORY TABS
            // =======================
            LoadItemsByType("shirt", top_itemPics, top_itemLabel, top_overlays, top_itemPanels);
    LoadItemsByType("short", bot_itemPics, bot_itemLabel, bot_overlays, bot_itemPanels);
    LoadItemsByType("pants", bot_itemPics, bot_itemLabel, bot_overlays, bot_itemPanels);
    LoadItemsByType("fabric", fab_itemPics, fab_itemLabel, fab_overlays, fab_itemPanels);
    LoadItemsByType("other", other_itemPics, other_itemLabel, other_overlays, other_itemPanels);

            // =======================
            // MERGE ALL TAB (PAGE 1 + 2)
            // =======================
            //allPicsCombined = all_itemPics.Concat(all_itemPics2nd).ToList();
            //allLabelsCombined = all_itemLabel.Concat(all_itemLabel2nd).ToList();
            //allOverlaysCombined = all_overlays.Concat(all_overlays2nd).ToList();
            //allPanelsCombined = all_itemPanels.Concat(all_itemPanels2nd).ToList();

            // =======================
            // LOAD ALL ITEMS (32 SLOTS)
            // =======================
            LoadAllItemsPage(currentPage);

            // =======================
            // APPLY STOCK STATUS AFTER LOAD
            // =======================
            LoadStockStatus();

    // =======================
    // PAGE VISIBILITY
    // =======================
    ToggleSecondPage();

    // =======================
    // REMOVE TAB HEADERS
    // =======================
    tabControl1.SizeMode = TabSizeMode.Fixed;
    tabControl1.ItemSize = new Size(0, 1);
    tabControl1.Padding = new Point(0, 0);
    tabControl1.Dock = DockStyle.Fill;
}



        //tab control buttons
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;

        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            fab_overlay.SelectedTab = tabPage4;
            LoadStockStatus();


            ///------------- FOR COUNTER/CASH PAYMENT ---------------------------------

            paymentMethod = "CASH/COUNTER";
            paymentIdtfy.Text = paymentMethod;
            buybutton.Visible = false;
            Printbutton.Visible = true;

            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();



        }



        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            fab_overlay.SelectedTab = tabPage4;
            LoadStockStatus();


            ///------------- FOR QR/CASHLESS PAYMENT ---------------------------------

            paymentMethod = "CASHLESS/QR";
            paymentIdtfy.Text = paymentMethod;
            buybutton.Visible = true;
            Printbutton.Visible = false;

            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();


        }

        private void guna2adminback_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        //buttons
        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            ResetCart();
 

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            ResetCart();
           

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            ResetCart();
           

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            ResetCart();
        

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            ResetCart();
      
        }

        /// --------------------------------- clicking the image products

        public void LoadTbItems(int itemId)
        {
            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM tbitems WHERE itemId = @itemId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemId", itemId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Load item info
                            ac.prodID.Text = reader["itemId"].ToString();
                            ac.prodName.Text = reader["itemName"].ToString();
                            ac.subName.Text = reader["itemType"].ToString();
                            ac.price.Text = reader["itemPrice"].ToString();
                            ac.stock.Text = reader["itemStock"].ToString();

                            // to set unavailable img
                         

                            // Load and process sizes
                            string sizeData = reader["itemSize"].ToString();
                            List<string> availableSizes = sizeData.Split(',').ToList();



                            // Enable/disable radio buttons
                            ac.sBtn.Enabled = availableSizes.Contains("S");
                            ac.mBtn.Enabled = availableSizes.Contains("M");
                            ac.lBtn.Enabled = availableSizes.Contains("L");
                            ac.xBtn.Enabled = availableSizes.Contains("XL");
                            ac.xxBtn.Enabled = availableSizes.Contains("2XL");


                            ac.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Item not found in the database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }

         private void LoadItemsByType(

            string typeFilter,
            List<Guna2PictureBox> pics,
            List<Label> labels,
            List<Guna2PictureBox> overlays,
            List<Guna2ShadowPanel> panels)
                {
                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();

                        string query = @"
                    SELECT itemId, itemName, itemStock, IMAGE_PATH, isEnabled 
                    FROM tbitems
                    WHERE (@type = 'all' OR itemType = @type)
                    ORDER BY itemId ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@type", typeFilter);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int slotIndex = 0;

                        while (reader.Read() && slotIndex < pics.Count)
                        {
                            string itemName = reader.GetString("itemName");
                            string img = reader.GetString("IMAGE_PATH");
                        int stock = reader.GetInt32("itemStock");
                            int id = reader.GetInt32("itemId");
                        bool isEnabled = reader.GetInt32("isEnabled") == 1;


                        string fullPath = Path.Combine(
                                Application.StartupPath,
                                "images_rsrcs", "itemPics", img
                            );

                            // Load image
                            if (File.Exists(fullPath))
                                pics[slotIndex].Image = Image.FromFile(fullPath);
                            else
                                pics[slotIndex].Image = null;

                            labels[slotIndex].Text = itemName;


                        bool notAvail = stock == 0 || !isEnabled;

                        overlays[slotIndex].Visible = notAvail;
                        overlays[slotIndex].BringToFront();
                        pics[slotIndex].Enabled = !notAvail;
                        pics[slotIndex].Visible = true;
                            labels[slotIndex].Visible = true;
                            panels[slotIndex].Visible = true;

                            pics[slotIndex].Tag = id;
                            labels[slotIndex].Tag = id;

                            slotIndex++;
                        }
                        
                        // Hide remaining slots
                        for (int i = slotIndex; i < pics.Count; i++)
                        {
                            pics[i].Visible = false;
                            labels[i].Visible = false;
                            overlays[i].Visible = false;
                            panels[i].Visible = false;
                        }
                    }
                }
            }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (fab_overlay.SelectedIndex)
            {
                case 0:
                    LoadItemsByType("all", all_itemPics, all_itemLabel, all_overlays, all_itemPanels);
                    break;
                case 1:
                    LoadItemsByType("shirt", top_itemPics, top_itemLabel, top_overlays, top_itemPanels);
                    break;
                case 2:
                    LoadItemsByType("pants", bot_itemPics, bot_itemLabel, bot_overlays, bot_itemPanels);
                    break;
                case 3:
                    LoadItemsByType("fabric", fab_itemPics, fab_itemLabel, fab_overlays, fab_itemPanels);
                    break;
                case 4:
                    LoadItemsByType("other", other_itemPics, other_itemLabel, other_overlays, other_itemPanels);
                    break;
            }
        }

        private void BindOverlay(Guna2PictureBox overlay, Guna2PictureBox basePic)
        {
            overlay.Parent = basePic;         // stick overlay on top of picture
            overlay.BackColor = Color.Transparent;
            overlay.BringToFront();           // ALWAYS show overlay in front
            overlay.Visible = false;
        }




        private void ItemPic_Click(object sender, EventArgs e)
        {
            if (sender is Guna2PictureBox pic && pic.Tag != null)
            {
                int itemId = Convert.ToInt32(pic.Tag);
                ac.prodIMG.Image = pic.Image;
                LoadTbItems(itemId);
            }
        }

        public void SetupItemLists()
        {
           
    // =======================
    // ALL TAB
    // =======================
    all_itemPics.AddRange(new Guna2PictureBox[] {
        all_itemPicFirst, all_itemPicSecond, all_itemPicThird, all_itemPicFourth,
        all_itemPicFifth, all_itemPicSixth, all_itemPicSeventh, all_itemPicEighth,
        all_itemPicNinth, all_itemPicTenth, all_itemPicEleventh, all_itemPicTwelfth,
        all_itemPicThirteenth, all_itemPicFourteenth, all_itemPicFifteenth, all_itemPicSixteenth
    });

    all_itemLabel.AddRange(new Label[] {
        all_itemLblFirst,  all_itemLblSecond,  all_itemLblThird,  all_itemLblFourth,
         all_itemLblFifth,  all_itemLblSixth,  all_itemLblSeventh,  all_itemLblEighth,
         all_itemLblNinth,  all_itemLblTenth,  all_itemLblEleventh,  all_itemLblTwelfth,
         all_itemLblThirteenth,  all_itemLblFourteenth,  all_itemLblFifteenth,  all_itemLblSixteenth
    });

    all_overlays.AddRange(new Guna2PictureBox[] {
        allFirst_Overlay, allSecond_Overlay, allThird_Overlay, allFourth_Overlay,
        allFifth_Overlay, allSixth_Overlay, allSeventh_Overlay, allEighth_Overlay,
        allNinth_Overlay, allTenth_Overlay, allEleventh_Overlay, allTwelfth_Overlay,
        allThirteenth_Overlay, allFourteenth_Overlay, allFifteenth_Overlay, allSixteenth_Overlay
    });

    all_itemPanels.AddRange(new Guna2ShadowPanel[] {
        all_itemPanelFirst, all_itemPanelSecond, all_itemPanelThird, all_itemPanelFourth, all_itemPanelFifth, all_itemPanelSixth,
        all_itemPanelSeventh, all_itemPanelEighth, all_itemPanelNinth, all_itemPanelTenth, all_itemPanelEleventh, all_itemPanelTwelfth,
        all_itemPanelThirteenth, all_itemPanelFourteenth, all_itemPanelFifteenth, all_itemPanelSixteenth
    });


    // =======================
    // TOP (SHIRTS)
    // =======================
    top_itemPics.AddRange(new Guna2PictureBox[] {
        top_itemPic1, top_itemPic2, top_itemPic3, top_itemPic4,
        top_itemPic5, top_itemPic6, top_itemPic7, top_itemPic8,
        top_itemPic9, top_itemPic10, top_itemPic11, top_itemPic12,
        top_itemPic13, top_itemPic14, top_itemPic15, top_itemPic16
    });

    top_itemLabel.AddRange(new Label[] {
        top_lbl1, top_lbl2, top_lbl3, top_lbl4,
        top_lbl5, top_lbl6, top_lbl7, top_lbl8,
        top_lbl9, top_lbl10, top_lbl11, top_lbl12,
        top_lbl13, top_lbl14, top_lbl15, top_lbl16
    });

    top_overlays.AddRange(new Guna2PictureBox[] {
        Top_Overlay1, Top_Overlay2, Top_Overlay3, Top_Overlay4,
        Top_Overlay5, Top_Overlay6, Top_Overlay7, Top_Overlay8,
        Top_Overlay9, Top_Overlay10, Top_Overlay11, Top_Overlay12,
        Top_Overlay13, Top_Overlay14, Top_Overlay15, Top_Overlay16
    });

    top_itemPanels.AddRange(new Guna2ShadowPanel[] {
        top_Panel1, top_Panel2, top_Panel3, top_Panel4,
        top_Panel5, top_Panel6, top_Panel7, top_Panel8,
        top_Panel9, top_Panel10, top_Panel11, top_Panel12,
        top_Panel13, top_Panel14, top_Panel15, top_Panel16
    });


    // =======================
    // BOTTOM (PANTS)
    // =======================
    bot_itemPics.AddRange(new Guna2PictureBox[] {
        bot_itemPic1, bot_itemPic2, bot_itemPic3, bot_itemPic4,
        bot_itemPic5, bot_itemPic6, bot_itemPic7, bot_itemPic8,
        bot_itemPic9, bot_itemPic10, bot_itemPic11, bot_itemPic12,
        bot_itemPic13, bot_itemPic14, bot_itemPic15, bot_itemPic16
    });

    bot_itemLabel.AddRange(new Label[] {
        bot_lbl1, bot_lbl2, bot_lbl3, bot_lbl4,
        bot_lbl5, bot_lbl6, bot_lbl7, bot_lbl8,
        bot_lbl9, bot_lbl10, bot_lbl11, bot_lbl12,
        bot_lbl13, bot_lbl14, bot_lbl15, bot_lbl16
    });

    bot_overlays.AddRange(new Guna2PictureBox[] {
        bot_overlay1, bot_overlay2, bot_overlay3, bot_overlay4,
        bot_overlay5, bot_overlay6, bot_overlay7, bot_overlay8,
        bot_overlay9, bot_overlay10, bot_overlay11, bot_overlay12,
        bot_overlay13, bot_overlay14, bot_overlay15, bot_overlay16
    });

    bot_itemPanels.AddRange(new Guna2ShadowPanel[] {
        bot_Panel1, bot_Panel2, bot_Panel3, bot_Panel4,
        bot_Panel5, bot_Panel6, bot_Panel7, bot_Panel8,
        bot_Panel9, bot_Panel10, bot_Panel11, bot_Panel12,
        bot_Panel13, bot_Panel14, bot_Panel15, bot_Panel16
    });


             //=======================
             //FABRIC
             //=======================
            fab_itemPics.AddRange(new Guna2PictureBox[] {
                fab_itemPic1, fab_itemPic2, fab_itemPic3, fab_itemPic4,
                fab_itemPic5, fab_itemPic6, fab_itemPic7, fab_itemPic8,
                fab_itemPic9, fab_itemPic10, fab_itemPic11, fab_itemPic12,
                fab_itemPic13, fab_itemPic14, fab_itemPic15, fab_itemPic16
            });

            fab_itemLabel.AddRange(new Label[] {
                fab_lbl1, fab_lbl2, fab_lbl3, fab_lbl4,
                fab_lbl5, fab_lbl6, fab_lbl7, fab_lbl8,
                fab_lbl9, fab_lbl10, fab_lbl11, fab_lbl12,
                fab_lbl13, fab_lbl14, fab_lbl15, fab_lbl16
            });

            fab_overlays.AddRange(new Guna2PictureBox[] {
                fab_overlay1, fab_overlay2, fab_overlay3, fab_overlay4,
                fab_overlay5, fab_overlay6, fab_overlay7, fab_overlay8,
                fab_overlay9, fab_overlay10, fab_overlay11, fab_overlay12,
                fab_overlay13, fab_overlay14, fab_overlay15, fab_overlay16
            });

            fab_itemPanels.AddRange(new Guna2ShadowPanel[] {
                fab_Panel1, fab_Panel2, fab_Panel3, fab_Panel4,
                fab_Panel5, fab_Panel6, fab_Panel7, fab_Panel8,
                fab_Panel9, fab_Panel10, fab_Panel11, fab_Panel12,
                fab_Panel13, fab_Panel14, fab_Panel15, fab_Panel16
            });


            // =======================
            // OTHER
            // =======================
            other_itemPics.AddRange(new Guna2PictureBox[] {
                other_itemPic1, other_itemPic2, other_itemPic3, other_itemPic4,
                other_itemPic5, other_itemPic6, other_itemPic7, other_itemPic8,
                other_itemPic9, other_itemPic10, other_itemPic11, other_itemPic12,
                other_itemPic13, other_itemPic14, other_itemPic15, other_itemPic16
            });

            other_itemLabel.AddRange(new Label[] {
                other_lbl1, other_lbl2, other_lbl3, other_lbl4,
                other_lbl5, other_lbl6, other_lbl7, other_lbl8,
                other_lbl9, other_lbl10, other_lbl11, other_lbl12,
                other_lbl13, other_lbl14, other_lbl15, other_lbl16
            });

            other_overlays.AddRange(new Guna2PictureBox[] {
                other_overlay1, other_overlay2, other_overlay3, other_overlay4,
                other_overlay5, other_overlay6, other_overlay7, other_overlay8,
                other_overlay9, other_overlay10, other_overlay11, other_overlay12,
                other_overlay13, other_overlay14, other_overlay15, other_overlay16
            });

            other_itemPanels.AddRange(new Guna2ShadowPanel[] {
                other_Panel1, other_Panel2, other_Panel3, other_Panel4,
                other_Panel5, other_Panel6, other_Panel7, other_Panel8,
                other_Panel9, other_Panel10, other_Panel11, other_Panel12,
                other_Panel13, other_Panel14, other_Panel15, other_Panel16
            });

             //=======================
             //ALL TAB SECOND(EXTENDED)
             //=======================

             //Item Pictures
            all_itemPics2nd.AddRange(new Guna2PictureBox[] {
    all_itemPicSeventeenth, all_itemPicEighteenth, all_itemPicNineteenth, all_itemPicTwentieth,
    all_itemPicTwentyFirst, all_itemPicTwentySecond, all_itemPicTwentyThird, all_itemPicTwentyFourth,
    all_itemPicTwentyFifth, all_itemPicTwentySixth, all_itemPicTwentySeventh, all_itemPicTwentyEighth,
    all_itemPicTwentyNinth, all_itemPicThirtieth, all_itemPicThirtyFirst, all_itemPicThirtySecond
});

            // Item Labels
            all_itemLabel2nd.AddRange(new Label[] {
    all_itemLblSeventeenth, all_itemLblEighteenth, all_itemLblNineteenth, all_itemLblTwentieth,
    all_itemLblTwentyFirst, all_itemLblTwentySecond, all_itemLblTwentyThird, all_itemLblTwentyFourth,
    all_itemLblTwentyFifth, all_itemLblTwentySixth, all_itemLblTwentySeventh, all_itemLblTwentyEighth,
    all_itemLblTwentyNinth, all_itemLblThirtieth, all_itemLblThirtyFirst, all_itemLblThirtySecond
});

            // Overlays
            all_overlays2nd.AddRange(new Guna2PictureBox[] {
    allSeventeenth_Overlay, alleighteenth_Overlay, allNineteenth_Overlay, allTwentieth_Overlay,
    allTwentyFirst_Overlay, allTwentySecond_Overlay, allTwentyThird_Overlay, allTwentyFourth_Overlay,
    allTwentyFifth_Overlay, allTwentySixth_Overlay, allTwentySeventh_Overlay, allTwentyEighth_Overlay,
    allTwentyNinth_Overlay, allThirtieth_Overlay, allThirtyFirst_Overlay, allThirtySecond_Overlay
});

            // Item Panels
            all_itemPanels2nd.AddRange(new Guna2ShadowPanel[] {
    all_itemPanelSeventeenth, all_itemPanelEighteenth, all_itemPanelNineteenth, all_itemPanelTwentieth,
    all_itemPanelTwentyFirst, all_itemPanelTwentySecond, all_itemPanelTwentyThird, all_itemPanelTwentyFourth,
    all_itemPanelTwentyFifth, all_itemPanelTwentySixth, all_itemPanelTwentySeventh, all_itemPanelTwentyEighth,
    all_itemPanelTwentyNinth, all_itemPanelThirtieth, all_itemPanelThirtyFirst, all_itemPanelThirtySecond
});





            for (int i = 0; i < all_overlays.Count; i++)
                BindOverlay(all_overlays[i], all_itemPics[i]);

            for (int i = 0; i < top_overlays.Count; i++)
                BindOverlay(top_overlays[i], top_itemPics[i]);

            for (int i = 0; i < bot_overlays.Count; i++)
                BindOverlay(bot_overlays[i], bot_itemPics[i]);

            for (int i = 0; i < fab_overlays.Count; i++)
                BindOverlay(fab_overlays[i], fab_itemPics[i]);

            for (int i = 0; i < other_overlays.Count; i++)
                BindOverlay(other_overlays[i], other_itemPics[i]);

            //for (int i = 0; i < all_overlays2nd.Count; i++)
            //    BindOverlay(all_overlays2nd[i], all_itemPics2nd[i]);

            foreach (var pic in all_itemPics)
                pic.Click += ItemPic_Click;

            foreach (var pic in top_itemPics)
                pic.Click += ItemPic_Click;

            foreach (var pic in bot_itemPics)
                pic.Click += ItemPic_Click;

            foreach (var pic in fab_itemPics)
                pic.Click += ItemPic_Click;

            foreach (var pic in other_itemPics)
                pic.Click += ItemPic_Click;

            //foreach (var pic in all_itemPics2nd)
            //    pic.Click += ItemPic_Click;

        }



        //private void UpdateOverlayForItem(int itemId, int stock, string imgPath)
        //{
        //    int index = itemId - 1;

        //    // FIX: Ignore invalid IDs
        //    if (index < 0 || index >= all_overlays.Count)
        //        return;

        //    bool notAvail = (stock == 0);

        //    all_overlays[index].Visible = notAvail;
        //    all_itemPics[index].Enabled = !notAvail;

        //    if (File.Exists(imgPath))
        //        all_itemPics[index].Image = Image.FromFile(imgPath);
        //}

        public void LoadStockStatus()
        {
            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                string query = "SELECT itemId, itemName, itemStock, itemType, IMAGE_PATH, isEnabled FROM tbitems ORDER BY itemId ASC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // separate slot index for each tab
                    int allIndex = 0, topIndex = 0, botIndex = 0, fabIndex = 0, otherIndex = 0;

                    while (reader.Read())
                    {
                        int id = reader.GetInt32("itemId");
                        string name = reader.GetString("itemName");
                        int stock = reader.GetInt32("itemStock");
                        string type = reader.GetString("itemType").ToLower(); // shirt, pants, fabric, other
                        string imgFileName = reader.GetString("IMAGE_PATH");
                        bool isEnabled = reader.GetInt32("isEnabled") == 1;

                        string fullPath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics", imgFileName);

                        Image img = File.Exists(fullPath) ? Image.FromFile(fullPath) : null;

                        // ALL TAB
                        if (allIndex < all_itemPics.Count)
                        {
                            all_itemPics[allIndex].Image = img;
                            all_itemLabel[allIndex].Text = name;
                            all_itemPics[allIndex].Image = img;
                            all_itemLabel[allIndex].Text = name;

                            bool notAvail = stock == 0 || !isEnabled;

                            all_itemPics[allIndex].Enabled = !notAvail;
                            all_overlays[allIndex].Visible = notAvail;
                            all_overlays[allIndex].BringToFront();


                            all_itemPics[allIndex].Visible = true;
                            all_itemPanels[allIndex].Visible = true;
                            all_itemLabel[allIndex].Visible = true;
                            all_itemPics[allIndex].Tag = id;
                            all_itemLabel[allIndex].Tag = id;

                            allIndex++;
                        }

                          
                        //if (allIndex2nd < allPicsCombined.Count)
                        //{
                        //    allPicsCombined[allIndex2nd].Image = img;
                        //    allLabelsCombined[allIndex2nd].Text = name;
                        //    allPicsCombined[allIndex2nd].Image = img;
                        //    allLabelsCombined[allIndex2nd].Text = name;

                        //    bool notAvail = stock == 0 || !isEnabled;

                        //    allPicsCombined[allIndex2nd].Enabled = !notAvail;
                        //    allOverlaysCombined[allIndex2nd].Visible = notAvail;
                        //    allOverlaysCombined[allIndex2nd].BringToFront();


                        //    allPicsCombined[allIndex2nd].Visible = true;
                        //    allPanelsCombined[allIndex2nd].Visible = true;
                        //    allLabelsCombined[allIndex2nd].Visible = true;
                        //    allPicsCombined[allIndex2nd].Tag = id;
                        //    allLabelsCombined[allIndex2nd].Tag = id;

                        //    allIndex2nd++;
                        //}



                        // Type-specific tab
                        switch (type)
                        {
                            case "shirt":
                                if (topIndex < top_itemPics.Count)
                                {
                                    top_itemPics[topIndex].Image = img;
                                    top_itemLabel[topIndex].Text = name;

                                    bool notAvail = stock == 0 || !isEnabled;

                                    top_itemPics[topIndex].Enabled = !notAvail;
                                    top_overlays[topIndex].Visible = notAvail;
                                    top_overlays[topIndex].BringToFront();

                                    top_itemPics[topIndex].Visible = true;
                                    top_itemPanels[topIndex].Visible = true;
                                    top_itemLabel[topIndex].Visible = true;
                                    top_itemPics[topIndex].Tag = id;
                                    top_itemLabel[topIndex].Tag = id;
                                    topIndex++;
                                }
                                break;
                            case "short":
                            case "pants":
                                if (botIndex < bot_itemPics.Count)
                                {
                                    bot_itemPics[botIndex].Image = img;
                                    bot_itemLabel[botIndex].Text = name;

                                    bool notAvail = stock == 0 || !isEnabled;

                                    bot_itemPics[botIndex].Enabled = !notAvail;
                                    bot_overlays[botIndex].Visible = notAvail;
                                    bot_overlays[botIndex].BringToFront();

                                    bot_itemPics[botIndex].Visible = true;
                                    bot_itemPanels[botIndex].Visible = true;
                                    bot_itemLabel[botIndex].Visible = true;
                                    bot_itemPics[botIndex].Tag = id;
                                    bot_itemLabel[botIndex].Tag = id;
                                    botIndex++;
                                }
                                break;
                            case "cloth/fabric":
                                if (fabIndex < fab_itemPics.Count)
                                {
                                    fab_itemPics[fabIndex].Image = img;
                                    fab_itemLabel[fabIndex].Text = name;

                                    bool notAvail = stock == 0 || !isEnabled;

                                    fab_itemPics[fabIndex].Enabled = !notAvail;
                                    fab_overlays[fabIndex].Visible = notAvail;
                                    fab_overlays[fabIndex].BringToFront();

                                    fab_itemPics[fabIndex].Visible = true;
                                    fab_itemPanels[fabIndex].Visible = true;
                                    fab_itemLabel[fabIndex].Visible = true;
                                    fab_itemPics[fabIndex].Tag = id;
                                    fab_itemLabel[fabIndex].Tag = id;
                                    fabIndex++;
                                }
                                break;
                            case "other":
                                if (otherIndex < other_itemPics.Count)
                                {
                                    other_itemPics[otherIndex].Image = img;
                                    other_itemLabel[otherIndex].Text = name;

                                    bool notAvail = stock == 0 || !isEnabled;

                                    other_itemPics[otherIndex].Enabled = !notAvail;
                                    other_overlays[otherIndex].Visible = notAvail;
                                    other_overlays[otherIndex].BringToFront();

                                    other_itemPics[otherIndex].Visible = true;
                                    other_itemPanels[otherIndex].Visible = true;
                                    other_itemLabel[otherIndex].Visible = true;
                                    other_itemPics[otherIndex].Tag = id;
                                    other_itemLabel[otherIndex].Tag = id;
                                    otherIndex++;
                                }
                                break;
                        }
                    }

                    // Hide remaining slots in each tab
                    HideUnusedSlots(allIndex, all_itemPics, all_itemPanels, all_itemLabel, all_overlays);
                    HideUnusedSlots(topIndex, top_itemPics, top_itemPanels, top_itemLabel, top_overlays);
                    HideUnusedSlots(botIndex, bot_itemPics, bot_itemPanels, bot_itemLabel, bot_overlays);
                    HideUnusedSlots(fabIndex, fab_itemPics, fab_itemPanels, fab_itemLabel, fab_overlays);
                    HideUnusedSlots(otherIndex, other_itemPics, other_itemPanels, other_itemLabel, other_overlays);
                }
            }
        }

        private void HideUnusedSlots(int startIndex, List<Guna2PictureBox> pics, List<Guna2ShadowPanel> panels, List<Label> labels, List<Guna2PictureBox> overlays)
        {
            for (int i = startIndex; i < pics.Count; i++)
            {
                pics[i].Visible = false;
                panels[i].Visible = false;
                labels[i].Visible = false;
                overlays[i].Visible = false;
            }
        }

        private void HideUnusedSlots(int startIndex, List<PictureBox> pics, List<Panel> panels, List<Label> labels, List<PictureBox> overlays)
        {
            for (int i = startIndex; i < pics.Count; i++)
            {
                pics[i].Visible = false;
                panels[i].Visible = false;
                labels[i].Visible = false;
                overlays[i].Visible = false;
            }
        }



        //--------------- for Item image
        private Image LoadItemImage(string fileName)
        {
            string fullPath = Path.Combine(itemImagePath, fileName);

            if (File.Exists(fullPath))
                return Image.FromFile(fullPath);

            return null;  
        }

        private void all_itemPicFirst_Click(object sender, EventArgs e)
        {
           
            

        }

        private void all_itemPicSecond_Click(object sender, EventArgs e)
        {
           
         

        }

        private void all_itemPicThird_Click(object sender, EventArgs e)
        {
          
          
        }

        private void all_itemPicFourth_Click(object sender, EventArgs e)
        { 
         
        }

        private void all_itemPicFifth_Click(object sender, EventArgs e)
        {
           
           
        }

        private void all_itemPicSixth_Click(object sender, EventArgs e)
        {
          
           
        }


        private void all_itemPicSeventh_Click(object sender, EventArgs e)
        {
          
           
        }

        private void all_itemPicEighth_Click(object sender, EventArgs e)
        {
          
           
        }

        private void all_itemPicNinth_Click(object sender, EventArgs e)
        {
          
           
        }

        private void all_itemPicTenth_Click(object sender, EventArgs e)
        {
        
           
        }

        private void all_itemPicEleventh_Click(object sender, EventArgs e)
        {
          
           
        }

        private void all_itemPicTwelfth_Click(object sender, EventArgs e)
        {
            
            
        }

        private void all_itemPicThirteenth_Click(object sender, EventArgs e)
        {
           
           
        }

        private void all_itemPicFourteenth_Click(object sender, EventArgs e)
        {
            
           
        }

        private void all_itemPicFifteenth_Click(object sender, EventArgs e)
        {
            
          
        }

        private void all_itemPicSixteenth_Click(object sender, EventArgs e)
        {
            
            
        }






        //------------------------------------------------------------------------------------------------------------------------
        public void ResetCart()
        {
            // 1. Restore stock for all items in cart
            foreach (var item in cartItems)
            {
                RestoreStock(item.ItemID, item.Quantity);
            }

            // 2. Clear the list
            cartItems.Clear();

            // 3. Reset UI
            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem };

            for (int i = 0; i < cartPics.Length; i++)
            {
                cartPics[i].Image = null;
                picsPanel[i].Visible = false;
            }

            cartCounter = 0;
            ttext.Visible = true;
            confirmBtn.Enabled = false;
        }


        public void RemoveCartItem(int index)
        {
            // Step 1: Restore stock & remove from cartItems list
            if (index >= 0 && index < cartItems.Count)
            {
                CartItem removedItem = cartItems[index];
                RestoreStock(removedItem.ItemID, removedItem.Quantity);
                cartItems.RemoveAt(index);
            }

            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem };
            Label[] productName = { firstItemName, secondItemName, thirdItemName, fourthItemName, fifthItemName };
            Label[] prodType = { firstItemType, secondItemType, thirdItemType, fourthItemType, fifthItemType };
            Label[] prodQty = { firstItemQty, secondItemQty, thirdItemQty, fourthItemQty, fifthItemQty };
            Label[] prodSize = { firstItemSize, secondItemSize, thirdItemSize, fourthItemSize, fifthItemSize };
            Label[] prodPrice = { firstItemPrice, secondItemPrice, thirdItemPrice, fourthItemPrice, fifthItemPrice };

            // Step 2: Shift UI cart items left
            for (int i = index; i < cartPics.Length - 1; i++)
            {
                cartPics[i].Image = cartPics[i + 1].Image;
                picsPanel[i].Visible = picsPanel[i + 1].Visible;

                productName[i].Text = productName[i + 1].Text;
                prodType[i].Text = prodType[i + 1].Text;
                prodQty[i].Text = prodQty[i + 1].Text;
                prodSize[i].Text = prodSize[i + 1].Text;
                prodPrice[i].Text = prodPrice[i + 1].Text;
            }

            // Step 3: Clear last slot
            cartPics[cartPics.Length - 1].Image = null;
            picsPanel[cartPics.Length - 1].Visible = false;

            productName[cartPics.Length - 1].Text = "";
            prodType[cartPics.Length - 1].Text = "";
            prodQty[cartPics.Length - 1].Text = "";
            prodSize[cartPics.Length - 1].Text = "";
            prodPrice[cartPics.Length - 1].Text = "";

            // Step 4: Recompute counter
            cartCounter = cartItems.Count;

            // Step 5: UI updates
            ttext.Visible = (cartCounter == 0);
            confirmBtn.Enabled = (cartCounter != 0);
        }

        public void ClearCartWithoutRestoringStock()
        {
            // Clear the cart items list
            cartItems.Clear();

            // Reset the cart visuals
            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem };

            for (int i = 0; i < cartPics.Length; i++)
            {
                cartPics[i].Image = null;
                picsPanel[i].Visible = false;
            }

            cartCounter = 0;
            ttext.Visible = true; // show "empty cart" text if any
            confirmBtn.Enabled = false;
        }

        ///  ------------------------------------------------------------------------------- TO RESTORE/RE-ADD THE QUANTITY FROM THE PRODUCT STOCK IN DB -- IF THE USER REMOVE/CANCEL THE ITEM FROM CART  
        private void RestoreStock(int itemId, int qty)
        {
            string query = "UPDATE tbitems SET itemStock = itemStock + @qty WHERE itemId = @id";

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@id", itemId);
                cmd.ExecuteNonQuery();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ResetCart();
          
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();
        }

        private void firstCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(0);
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();
            //cartCounter = 0;

        }

        private void secondCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(1);
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();

        }

        private void thirdCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(2);
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();
        }

        private void fourthCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(3);
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();

        }

        private void fifthcancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(4);
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();

        }



        //------------------------------------------------------------------------------------------------------------------------
        //admin section
        // Admin Functionalities
        private void adminIntialize()
        {
            inventoryDB.Table(inventoryTable);
            receiptDB.Table(receiptTable);
            admin_tabControl.SelectedTab = admin_dashboard;
            activeCBox();
            loadDashboardData();
            //await loadSort();
        }

        private void dashboardButton_Click(object sender, EventArgs e)
        {
            admin_tabControl.SelectedTab = admin_dashboard;
            activeCBox();
            closeModal();
        }

        private void inventoryButton_Click(object sender, EventArgs e)
        {
            admin_tabControl.SelectedTab = admin_inventory;
            activeCBox();
            closeModal();
        }
        private void HistoryButton_Click(object sender, EventArgs e)
        {
            admin_tabControl.SelectedTab = admin_purchaseHistory;
            activeCBox();
            closeModal();
        }
        void activeCBox()
        {
            adminSort.SelectedIndex = 0;
            if (admin_tabControl.SelectedTab == admin_dashboard)
            {

                adminSort.Visible = false;
            }
            else if (admin_tabControl.SelectedTab == admin_inventory)
            {
                adminSort.Visible = true;
                if (adminSort.Items.Contains("Date")) adminSort.Items.Remove("Date");
                if (!adminSort.Items.Contains("Stock")) 
                { 
                    adminSort.Items.Add("By Stock (Highest)");
                    adminSort.Items.Add("By Stock (Lowest)");
                }
            }
            else if (admin_tabControl.SelectedTab == admin_purchaseHistory)
            {
                adminSort.Visible = true;
                if (adminSort.Items.Contains("Stock"))
                {
                    adminSort.Items.Remove("By Stock (Highest)");
                    adminSort.Items.Remove("By Stock (Lowest)");
                }
                if (!adminSort.Items.Contains("Date")) adminSort.Items.Add("Date");
            }
        }

      
        //checkout tab
        private async void confirmBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = Checkout;

           
            receiptData = Cart.generatePurchase(cartItems);

            pdfTemplate pdf = new pdfTemplate(receiptData);
            createPDF.generate(pdf);

            webPanel = await Web.viewPDF();



            receiptPanel.Controls.Clear();
            receiptPanel.Controls.Add(webPanel);
            webPanel.Dock = DockStyle.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to discard all items?",
                "Confirmation",
                MessageBoxButtons.OKCancel
            );


            if (result == DialogResult.OK)
            {


                ResetCart();

                // Clear the PDF viewer panel
                receiptPanel.Controls.Clear();

                // Dispose the webPanel if it exists
                if (webPanel != null)
                {
                    webPanel.Dispose();
                    webPanel = null;
                }

                // Reset receipt data object
                receiptData = null;

                // Reset cart items if needed
                cartItems.Clear();

                LoadStockStatus();


                tabControl1.SelectedTab = tabPage3;
                receiptPanel.Controls.Remove(webPanel);
            }
            else
            {
                
            }



           
        }

        private void buybutton_Click(object sender, EventArgs e)
        {
            paymentControl paymentControl = new paymentControl(receiptData,receiptTable);
            paymentControl.TabIndex = 20;
            Checkout.Controls.Add(paymentControl);
            int x = (Checkout.Width - paymentControl.Width) / 2;
            int y = (Checkout.Height - paymentControl.Height) / 2;
            paymentControl.Location = new Point(x, y);
            paymentControl.BringToFront();

   
            //addPurchase receipt = new addPurchase(randomData.generateHistory());
            //receiptTable.Controls.Add(receipt);
        }

        private void selectedSort()
        {
            //adminSort.Items.
        }

        public void showReceiptModal(ReceiptGroup receipt)
        {
            closeModal();
            receiptModal = new ReceiptModal(receipt);
            receiptModal.TabIndex = 20;
            Admin.Controls.Add(receiptModal);
            int x = (Admin.Width - receiptModal.Width) / 2;
            int y = (Admin.Height - receiptModal.Height) / 2;
            receiptModal.Location = new Point(x, y);
            receiptModal.BringToFront();
        }

        public void showInventoryModal(int itemId, string name, string type,
                      decimal price, int stock, string imagePath)
        {
            closeModal();
            inventoryModal = new InventoryModal( itemId, name, type, price, stock, imagePath, inventoryTable);
            Admin.Controls.Add(inventoryModal);
            int x = (Admin.Width - inventoryModal.Width) / 2;
            int y = (Admin.Height - inventoryModal.Height) / 2;
            inventoryModal.Location = new Point(x, y);
            inventoryModal.BringToFront();
        }


        public void closeModal()
        {
            if (receiptModal != null)
            {
                admin_purchaseHistory.Controls.Remove(receiptModal);
                receiptModal.Dispose();
                receiptModal = null;
            }
            if(inventoryModal != null)
            {
                admin_purchaseHistory.Controls.Remove(inventoryModal);
                inventoryModal.Dispose();
                inventoryModal = null;
            }
        }

        private void adminSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            inventoryDB.TableSort(admin_tabControl, admin_inventory, adminSort, inventoryTable);
            receiptDB.TableSort(admin_tabControl, admin_purchaseHistory, adminSort, receiptTable);
            closeModal();
        }   

        public void loadDashboardData()
        {
            InventoryCount.Text = inventoryDB.getInventory().ToString();
            TotalSold.Text = "₱" + receiptDB.totalSale().ToString("F2");
            TotalPaid.Text = receiptDB.Paid().ToString();
            TotalUnpaid.Text = receiptDB.NotPaid().ToString();
            TotalClaimed.Text = receiptDB.Claimed().ToString();
            TotalUnclaimed.Text = receiptDB.NotClaimed().ToString();
        }


        private void inventoryTable_Paint(object sender, PaintEventArgs e)
        {

        }



        ///-------------------------------------------------------------------------------------------------- for saving the dets/info to history...........................
       



        private void Printbutton_Click(object sender, EventArgs e)
        {

            myconn.SaveReceipt(receiptData, paymentMethod, "false");

            DialogResult result = MessageBox.Show(
           "Please present your receipt at the counter \nto complete the payment & claim the item",
            "Information",
                MessageBoxButtons.OK
            );
            button2.Visible = false;
            Printbutton.Visible = false;
            again.Location = new Point(814, 976);
        



            ShowCountdownMessage();
        }

        public void ShowCountdownMessage()
        {
            ClearCartWithoutRestoringStock();

            Form countdownForm = new Form
            {
                Text = "Confirmation",
                Width = 300,
                Height = 150,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lbl = new Label
            {
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Left = 80,
                Top = 70,
                Width = 80
            };

            Button cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Left = 170,
                Top = 70,
                Width = 80
            };

            countdownForm.Controls.Add(lbl);
            countdownForm.Controls.Add(okButton);
            countdownForm.Controls.Add(cancelButton);

            int seconds = 5;
            lbl.Text = $"Switching to Homepage… Cleaning up for the next session.\nAuto-confirm in {seconds}...";
            okButton.Text = $"OK ({seconds})";

            Timer timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                seconds--;
                lbl.Text = $"Switching to Homepage… Cleaning up for the next session.\nAuto-confirm in {seconds}...";
                okButton.Text = $"OK ({seconds})";

                if (seconds == 0)
                {
                    timer.Stop();
                    countdownForm.DialogResult = DialogResult.OK;
                    countdownForm.Close();
                }
            };

            countdownForm.Shown += (s, e) => timer.Start();

            DialogResult result = countdownForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                tabControl1.SelectedTab = tabPage1;
                button2.Visible = true;
                Printbutton.Visible = true;
                again.Location = new Point(957, 976);
            }
        }



        private void guna2PictureBox27_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            closeModal();
        }

        private void adminHeader_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void refreshAdminInven()
        {
            closeModal();
            loadDashboardData();
            inventoryDB.Table(inventoryTable);
            receiptDB.Table(receiptTable);
        }
        private void guna2PictureBox31_Click(object sender, EventArgs e)
        {
            refreshAdminInven();
        }

        private void guna2PictureBox26_Click(object sender, EventArgs e)
        {
            closeModal();
        }

        //Notif and Profile
        private void guna2PictureBox29_Click(object sender, EventArgs e)
        {
            closeModal();
        }

        private void guna2PictureBox28_Click(object sender, EventArgs e)
        {
            closeModal();
        }

        private void guna2PictureBox35_Click(object sender, EventArgs e)
        {

        }

        private void again_Click(object sender, EventArgs e)
        {

            ClearCartWithoutRestoringStock();
            tabControl1.SelectedTab = tabPage1;
            button2.Visible = true;
            Printbutton.Visible = true;
            again.Location = new Point(957, 976);

            tabControl1.SelectedTab = tabPage1;
        }

        private void guna2PictureBox45_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void footer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomGradientPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_overlay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblSixteenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblFifteenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblFourteenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblThirteenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblTwelfth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblEleventh_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblTenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblNinth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblEighth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblSeventh_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblSixth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblFifth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblFourth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblThird_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblFirst_Click(object sender, EventArgs e)
        {

        }

        private void all_itemLblSecond_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void all_itemPanelSixteenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allSixteenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelFifteenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allFifteenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelFourteenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allFourteenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelThirteenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allThirteenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwelfth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwelfth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelEleventh_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allEleventh_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelNinth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allNinth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void all_itemPanelEighth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allEighth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelSeventh_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allSeventh_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelSixth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allSixth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelFifth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allFifth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelFourth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allFourth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelThird_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allThird_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelSecond_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allSecond_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelFirst_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allFirst_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl16_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl15_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl14_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl13_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl12_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl11_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl10_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl9_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl8_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl7_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl6_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl5_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl4_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl3_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl1_Click(object sender, EventArgs e)
        {

        }

        private void top_lbl2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void top_Panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay16_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic16_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay15_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic15_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay14_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic14_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay13_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic13_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay12_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic12_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay11_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic11_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay10_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic10_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay9_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic9_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void top_Panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay8_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic8_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay7_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic7_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay6_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic6_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay5_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic5_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay4_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic4_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay3_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic3_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay2_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic2_Click(object sender, EventArgs e)
        {

        }

        private void top_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Top_Overlay1_Click(object sender, EventArgs e)
        {

        }

        private void top_itemPic1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl16_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl15_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl14_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl13_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl12_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl11_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl10_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl9_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl8_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl7_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl6_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl5_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl4_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl3_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl1_Click(object sender, EventArgs e)
        {

        }

        private void bot_lbl2_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_Panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay16_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic16_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay15_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic15_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay14_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic14_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay13_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic13_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay12_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic12_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay11_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic11_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay10_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic10_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay9_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic9_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_Panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay8_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic8_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay7_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic7_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay6_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic6_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay5_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic5_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay4_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic4_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay3_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic3_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay2_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic2_Click(object sender, EventArgs e)
        {

        }

        private void bot_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_overlay1_Click(object sender, EventArgs e)
        {

        }

        private void bot_itemPic1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl16_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl15_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl14_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl13_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl12_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl11_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl10_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl9_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl8_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl7_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl6_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl5_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl4_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl3_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl1_Click(object sender, EventArgs e)
        {

        }

        private void fab_lbl2_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_Panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic16_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay16_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic15_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay15_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic14_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay14_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic13_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay13_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic12_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay12_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic11_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay11_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic10_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay10_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic9_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay9_Click(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_Panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic8_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay8_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic7_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay7_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic6_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay6_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic5_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay5_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic4_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay4_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic3_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay3_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic2_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay2_Click(object sender, EventArgs e)
        {

        }

        private void fab_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fab_itemPic1_Click(object sender, EventArgs e)
        {

        }

        private void fab_overlay1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl16_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl15_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl14_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl13_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl12_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl11_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl10_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl9_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl8_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl7_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl6_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl5_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl4_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl3_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl1_Click(object sender, EventArgs e)
        {

        }

        private void other_lbl2_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_Panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay15_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic15_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay14_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic14_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay13_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic13_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay12_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic12_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay11_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic11_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay10_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic10_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay9_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic9_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay16_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic16_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_Panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay8_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic8_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay7_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic7_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay6_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic6_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay5_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic5_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_itemPic4_Click(object sender, EventArgs e)
        {

        }

        private void other_overlay4_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay3_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic3_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay2_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic2_Click(object sender, EventArgs e)
        {

        }

        private void other_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void other_overlay1_Click(object sender, EventArgs e)
        {

        }

        private void other_itemPic1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fifthItem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fifthItemSize_Click(object sender, EventArgs e)
        {

        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void fifthItemType_Click(object sender, EventArgs e)
        {

        }

        private void fifthItemName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void fifthItemPrice_Click(object sender, EventArgs e)
        {

        }

        private void label52_Click(object sender, EventArgs e)
        {

        }

        private void fifthItemQty_Click(object sender, EventArgs e)
        {

        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private void fifthPic_Click(object sender, EventArgs e)
        {

        }

        private void fifthID_Click(object sender, EventArgs e)
        {

        }

        private void fourthItem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fourthItemSize_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void fourthItemType_Click(object sender, EventArgs e)
        {

        }

        private void fourthItemName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void fourthItemPrice_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void fourthItemQty_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void fourthPic_Click(object sender, EventArgs e)
        {

        }

        private void fourthID_Click(object sender, EventArgs e)
        {

        }

        private void thirdItem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void thirdItemSize_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void thirdItemType_Click(object sender, EventArgs e)
        {

        }

        private void thirdItemName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void thirdItemPrice_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void thirdItemQty_Click(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void thirdPic_Click(object sender, EventArgs e)
        {

        }

        private void thirdID_Click(object sender, EventArgs e)
        {

        }

        private void secondItem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void secondItemSize_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void secondItemType_Click(object sender, EventArgs e)
        {

        }

        private void secondItemName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void secondItemPrice_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void secondItemQty_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void secondPic_Click(object sender, EventArgs e)
        {

        }

        private void secondID_Click(object sender, EventArgs e)
        {

        }

        private void firstItem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void firstItemSize_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void firstItemType_Click(object sender, EventArgs e)
        {

        }

        private void firstItemName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void firstItemPrice_Click(object sender, EventArgs e)
        {

        }

        private void firstPic_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void firstItemQty_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void firstID_Click(object sender, EventArgs e)
        {

        }

        private void ttext_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomGradientPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomGradientPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox19_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox20_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox21_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox22_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel6_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void Admin_Click(object sender, EventArgs e)
        {

        }

        private void AdminTitlePic_Click(object sender, EventArgs e)
        {

        }

        private void admin_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void admin_dashboard_Click(object sender, EventArgs e)
        {

        }

        private void TotalUnclaimed_Click(object sender, EventArgs e)
        {

        }

        private void TotalClaimed_Click(object sender, EventArgs e)
        {

        }

        private void TotalUnpaid_Click(object sender, EventArgs e)
        {

        }

        private void TotalPaid_Click(object sender, EventArgs e)
        {

        }

        private void TotalSold_Click(object sender, EventArgs e)
        {

        }

        private void InventoryCount_Click(object sender, EventArgs e)
        {

        }

        private void DashboardPic_Click(object sender, EventArgs e)
        {

        }

        private void AdminDashSide_Click(object sender, EventArgs e)
        {

        }

        private void TotalUnclaimedPic_Click(object sender, EventArgs e)
        {

        }

        private void TotalClaimedPic_Click(object sender, EventArgs e)
        {

        }

        private void TotalUnpaidDashPic_Click(object sender, EventArgs e)
        {

        }

        private void EarningDashPic_Click(object sender, EventArgs e)
        {

        }

        private void inventoryDashPic_Click(object sender, EventArgs e)
        {

        }

        private void DashboardTitlePic_Click(object sender, EventArgs e)
        {

        }

        private void admin_inventory_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void aaa_TextChanged(object sender, EventArgs e)
        {

        }
        private void guna2Shapes1_Click(object sender, EventArgs e)
        {

        }
        private void admin_purchaseHistory_Click(object sender, EventArgs e)
        {

        }
        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void receiptTable_Paint(object sender, PaintEventArgs e)
        {

        }
        private void purchaseHeader_TextChanged(object sender, EventArgs e)
        {

        }
        private void historyTableHeader_Click(object sender, EventArgs e)
        {

        }
        private void lspulogo_Click(object sender, EventArgs e)
        {

        }
        private void AdminSidePanel_Click(object sender, EventArgs e)
        {

        }
        private void Checkout_Click(object sender, EventArgs e)
        {

        }
        private void receiptPanel_Paint(object sender, PaintEventArgs e)
        {

        }
       private void guna2CustomGradientPanel14_Paint(object sender, PaintEventArgs e)
        {

        }
        private void guna2CustomGradientPanel15_Paint(object sender, PaintEventArgs e)
        {

        }
        private void guna2PictureBox37_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox38_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox39_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox40_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox41_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox25_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox26_Click_1(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox27_Click_1(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox28_Click_1(object sender, EventArgs e)
        {

        }
        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox29_Click_1(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox30_Click(object sender, EventArgs e)
        {

        }
        private void guna2CirclePictureBox6_Click(object sender, EventArgs e)
        {

        }
        private void paymentIdtfy_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox31_Click_1(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox32_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox33_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox34_Click(object sender, EventArgs e)
        {

        }
        private void guna2CirclePictureBox7_Click(object sender, EventArgs e)
        {

        }
        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox35_Click_1(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox36_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        private void tabPage9_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void label15_Click(object sender, EventArgs e)
        {

        }
        private void label16_Click(object sender, EventArgs e)
        {

        }
        private void label18_Click(object sender, EventArgs e)
        {

        }
        private void label20_Click(object sender, EventArgs e)
        {

        }
        private void label23_Click(object sender, EventArgs e)
        {

        }
        private void label26_Click(object sender, EventArgs e)
        {

        }
        private void label27_Click(object sender, EventArgs e)
        {

        }
        private void label29_Click(object sender, EventArgs e)
        {

        }
        private void label31_Click(object sender, EventArgs e)
        {

        }
        private void label34_Click(object sender, EventArgs e)
        {

        }
        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }
        private void all_itemPanelThirtySecond_Paint(object sender, PaintEventArgs e)
        {

        }
        private void allThirtySecond_Overlay_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox43_Click(object sender, EventArgs e)
        {

        }
        private void all_itemPanelThirtyFirst_Paint(object sender, PaintEventArgs e)
        {

        }
        private void allThirtyFirst_Overlay_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox61_Click(object sender, EventArgs e)
        {

        }
        private void all_itemPanelThirtieth_Paint(object sender, PaintEventArgs e)
        {

        }
        private void allThirtieth_Overlay_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox47_Click(object sender, EventArgs e)
        {

        }
        private void all_itemPanelTwentyNinth_Paint(object sender, PaintEventArgs e)
        {

        }
        private void allTwentyNinth_Overlay_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox49_Click(object sender, EventArgs e)
        {

        }
        private void all_itemPanelTwentyEighth_Paint(object sender, PaintEventArgs e)
        {

        }
        private void allTwentyEighth_Overlay_Click(object sender, EventArgs e)
        {

        }
        private void guna2PictureBox51_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentySeventh_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentySeventh_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox53_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentySixth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentySixth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox55_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentyFifth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentyFifth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox57_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void all_itemPanelTwentyFourth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentyFourth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox59_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentyThird_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentyThird_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentySecond_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentySecond_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPicTwentySecond_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentyFirst_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentyFirst_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPicTwentyFirst_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelTwentieth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allTwentieth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPicTwentieth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelNineteenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allNineteenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPicNineteenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelEighteenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void alleighteenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPicEighteenth_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPanelSeventeenth_Paint(object sender, PaintEventArgs e)
        {

        }

        private void allSeventeenth_Overlay_Click(object sender, EventArgs e)
        {

        }

        private void all_itemPicSeventeenth_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }
        //===================== for 2nd all section incase ma full ung first section nung all item
        public int itemsPerPage = 16;
        public int currentPage = 1;
        public int totalItems = 0;

        public void LoadAllItemsPage(int page)
        {
            int skip = (page - 1) * itemsPerPage;

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                string query = @"
SELECT itemId, itemName, itemStock, IMAGE_PATH, isEnabled
FROM tbitems
ORDER BY itemId ASC
LIMIT @limit OFFSET @offset";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@limit", itemsPerPage);
                cmd.Parameters.AddWithValue("@offset", skip);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Combine first and second sections
                    var picsCombined = all_itemPics.Concat(all_itemPics2nd).ToArray();
                    var labelsCombined = all_itemLabel.Concat(all_itemLabel2nd).ToArray();
                    var overlaysCombined = all_overlays.Concat(all_overlays2nd).ToArray();
                    var panelsCombined = all_itemPanels.Concat(all_itemPanels2nd).ToArray();

                    int slotIndex = 0;

                    while (reader.Read() && slotIndex < picsCombined.Length)
                    {
                        string itemName = reader.GetString("itemName");
                        string img = reader.GetString("IMAGE_PATH");
                        int stock = reader.GetInt32("itemStock");
                        int id = reader.GetInt32("itemId");
                        bool isEnabled = reader.GetBoolean("isEnabled");

                        bool notAvail = stock == 0 || !isEnabled;

                        string fullPath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics", img);

                        // Set item picture and label
                        picsCombined[slotIndex].Image = File.Exists(fullPath) ? Image.FromFile(fullPath) : null;
                        labelsCombined[slotIndex].Text = itemName;

                        // Enable/disable and overlay
                        picsCombined[slotIndex].Enabled = !notAvail;
                        overlaysCombined[slotIndex].Visible = notAvail;
                        overlaysCombined[slotIndex].BringToFront();

                        // Make visible
                        picsCombined[slotIndex].Visible = true;
                        labelsCombined[slotIndex].Visible = true;
                        panelsCombined[slotIndex].Visible = true;

                        // Store ID
                        picsCombined[slotIndex].Tag = id;
                        labelsCombined[slotIndex].Tag = id;

                        slotIndex++;
                    }

                    // Hide unused slots
                    for (int i = slotIndex; i < picsCombined.Length; i++)
                    {
                        picsCombined[i].Visible = false;
                        labelsCombined[i].Visible = false;
                        panelsCombined[i].Visible = false;
                        overlaysCombined[i].Visible = false;
                    }
                }
            }

            // Show/hide page navigation buttons
            all_firstDOWN.Visible = page * itemsPerPage < totalItems;
            all_secUP.Visible = page > 1;
        }

        private void LoadTotalItemCount()
        {
            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM tbitems";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                totalItems = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        private void ToggleSecondPage()
        {
            // Page 2 exists if total items > itemsPerPage
            all_firstDOWN.Visible = totalItems > itemsPerPage;
            all_secUP.Visible = false; // initially on page 1
        }
        private void all_firstDOWN_Click(object sender, EventArgs e)
        {
            if (currentPage * itemsPerPage >= totalItems) return;  
            currentPage++;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = true;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
        }

        private void all_secUP_Click(object sender, EventArgs e)
        {
            if (currentPage <= 1) return;
            currentPage--;
            LoadAllItemsPage(currentPage);

            all_secUP.Visible = currentPage > 1;
            all_firstDOWN.Visible = currentPage * itemsPerPage < totalItems;
            LoadStockStatus();
            
        }

        private void add_Click(object sender, EventArgs e)
        {
            AddNewItem ani = new AddNewItem();
            ani.ShowDialog();
        }

        private void fab_overlay_Click(object sender, EventArgs e)
        {
          
        }






        //Ignore ko to, di ko alam kung pano mo to iimplete - liam : D


        //        public static class CountdownMessageBox
        //{

        //    public static void Show(string text, string title = "Information", int seconds = 5, Action onClose = null)
        //    {
        //        // Create a Form that looks like a MessageBox
        //        var box = new MessageBoxLikeForm(text, title, seconds, onClose);
        //        box.ShowDialog(); // modal, centered
        //    }

        //    private class MessageBoxLikeForm : Form
        //    {
        //        private readonly Label _label;
        //        private readonly Timer _timer;
        //        private int _seconds;
        //        private readonly string _baseText;
        //        private readonly PictureBox _iconBox;

        //        public MessageBoxLikeForm(string text, string title, int seconds, Action onClose)
        //        {
        //            _baseText = text;
        //            _seconds = Math.Max(1, seconds);

        //            Text = title;
        //            StartPosition = FormStartPosition.CenterScreen;
        //            FormBorderStyle = FormBorderStyle.None;
        //            MaximizeBox = false;
        //            MinimizeBox = false;
        //            ShowInTaskbar = false;
        //            TopMost = true; // behaves like a message box
        //            AutoSize = false;
        //            Width = 460;
        //            Height = 180;

        //            // Icon (information icon to feel like MessageBox)
        //            _iconBox = new PictureBox
        //            {
        //                SizeMode = PictureBoxSizeMode.CenterImage,
        //                Width = 48,
        //                Height = 48,
        //                Left = 20,
        //                Top = 25,
        //                Image = IconToBitmap(SystemIcons.Information)
        //            };

        //            // Message label
        //            _label = new Label
        //            {
        //                AutoSize = false,
        //                Left = _iconBox.Right + 16,
        //                Top = 20,
        //                Width = ClientSize.Width - (_iconBox.Right + 36),
        //                Height = 90,
        //                TextAlign = ContentAlignment.MiddleLeft,
        //                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular)
        //            };

        //            Controls.Add(_iconBox);
        //            Controls.Add(_label);

        //            // Timer
        //            _timer = new Timer { Interval = 1000 };
        //            _timer.Tick += (s, e) =>
        //            {
        //                _seconds--;
        //                UpdateText();
        //                if (_seconds <= 0)
        //                {
        //                    _timer.Stop();
        //                    Close();
        //                    onClose?.Invoke();  
        //                }
        //            };

        //            Shown += (s, e) =>
        //            {
        //                UpdateText();
        //                _timer.Start();
        //            };

        //            FormClosed += (s, e) =>
        //            {
        //                _timer.Stop();
        //                _timer.Dispose();
        //            };
        //        }

        //        private void UpdateText()
        //        {
        //            _label.Text = $"{_baseText}\r\n\r\nClosing in {_seconds}...";
        //        }

        //        // Convert Icon to Bitmap to show in PictureBox
        //        private static Bitmap IconToBitmap(Icon icon)
        //        {
        //            return icon.ToBitmap();
        //        }

        //        // Add a standard drop shadow where supported
        //        protected override CreateParams CreateParams
        //        {
        //            get
        //            {
        //                const int CS_DROPSHADOW = 0x00020000;
        //                var cp = base.CreateParams;
        //                cp.ClassStyle |= CS_DROPSHADOW;
        //                return cp;
        //            }
        //        }
        //    }
        //}

        //class

    }

}

