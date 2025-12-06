using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Microsoft.Web.WebView2.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Guna.UI2.Native.WinApi;
using Microsoft.Web.WebView2.WinForms;
using static kiosk.randomData;

namespace kiosk
{

    public partial class Main : Form
    {
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

        AddCart ac;

        //--------------------------------- Admin Section ---------------------------------
        //Admin Inventory
        List<InventoryItem> InventoryData = new List<InventoryItem>();
        List<AddInventory> Inventory  = new List<AddInventory>();
        //Receipt Data
        receiptTemplate receiptData = new receiptTemplate();
        createPDF createPDF = new createPDF();
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

        }



        private void Main_Load(object sender, EventArgs e)
        {

            ac = new AddCart(this);

            //BindOverlay(allFirst_Overlay, all_itemPicFirst);
            //BindOverlay(allSecond_Overlay, all_itemPicSecond);
            //BindOverlay(allThird_Overlay, all_itemPicThird);
            //BindOverlay(allFourth_Overlay, all_itemPicFourth);
            //BindOverlay(allFifth_Overlay, all_itemPicFifth);
            //BindOverlay(allSeventh_Overlay, all_itemPicSeventh);

            SetupItemLists();
            LoadStockStatus();
            ResetCart();

            LoadItemsByType("shirt", top_itemPics, top_itemLabel, top_overlays, top_itemPanels);
            LoadItemsByType("pants", bot_itemPics, bot_itemLabel, bot_overlays, bot_itemPanels);
            LoadItemsByType("fabric", fab_itemPics, fab_itemLabel, fab_overlays, fab_itemPanels);
            LoadItemsByType("other", other_itemPics, other_itemLabel, other_overlays, other_itemPanels);
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

        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            fab_overlay.SelectedTab = tabPage4;
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
                    SELECT itemId, itemName, itemStock, IMAGE_PATH 
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

                            bool notAvail = stock == 0;
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

        private void SetupItemLists()
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
            // Bind overlay to picture (this was missing)
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

        private void LoadStockStatus()
        {
            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();
                string query = "SELECT itemId, itemName, itemStock, itemType, IMAGE_PATH FROM tbitems ORDER BY itemId ASC";

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
                        string fullPath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics", imgFileName);

                        Image img = File.Exists(fullPath) ? Image.FromFile(fullPath) : null;

                        // ALL TAB
                        if (allIndex < all_itemPics.Count)
                        {
                            all_itemPics[allIndex].Image = img;
                            all_itemLabel[allIndex].Text = name;
                            all_itemPics[allIndex].Enabled = stock > 0;
                            all_overlays[allIndex].Visible = stock == 0;
                            all_overlays[allIndex].BringToFront();
                            all_itemPics[allIndex].Visible = true;
                            all_itemPanels[allIndex].Visible = true;
                            all_itemLabel[allIndex].Visible = true;
                            all_itemPics[allIndex].Tag = id;
                            all_itemLabel[allIndex].Tag = id;
                            allIndex++;
                        }

                        // Type-specific tab
                        switch (type)
                        {
                            case "shirt":
                                if (topIndex < top_itemPics.Count)
                                {
                                    top_itemPics[topIndex].Image = img;
                                    top_itemLabel[topIndex].Text = name;
                                    top_itemPics[topIndex].Enabled = stock > 0;
                                    top_overlays[topIndex].Visible = stock == 0;
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
                                    bot_itemPics[botIndex].Enabled = stock > 0;
                                    bot_overlays[botIndex].Visible = stock == 0;
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
                                    fab_itemPics[fabIndex].Enabled = stock > 0;
                                    fab_overlays[fabIndex].Visible = stock == 0;
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
                                    other_itemPics[otherIndex].Enabled = stock > 0;
                                    other_overlays[otherIndex].Visible = stock == 0;
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
            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem };

            for (int i = 0; i < cartPics.Length; i++)
            {
                cartPics[i].Image = null;
                picsPanel[i].Visible = false;
                ttext.Visible = false;
            }

            cartCounter = 0; // reset the counter
            ttext.Visible = true; // show the "empty cart" text if you have one
            confirmBtn.Enabled = false;

            /////// FOR CLEARING THE RECEIPT
            // Reset cart items if needed
            cartItems.Clear();
             
        }

        public void RemoveCartItem(int index)
        {
            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem };

            // shift items left from index
            for (int i = index; i < cartPics.Length - 1; i++)
            {
                cartPics[i].Image = cartPics[i + 1].Image;
                picsPanel[i].Visible = picsPanel[i + 1].Visible;
            }

            // clear last slot
            cartPics[cartPics.Length - 1].Image = null;
            picsPanel[cartPics.Length - 1].Visible = false;

            // recompute cartCounter as number of filled slots
            cartCounter = 0;
            for (int i = 0; i < cartPics.Length; i++)
            {
                if (cartPics[i].Image != null) cartCounter++;
            }

            // show empty-cart text if now empty
            ttext.Visible = (cartCounter == 0);

            if (cartCounter == 0)
            {
                confirmBtn.Enabled = false;

                /////// FOR CLEARING THE RECEIPT
         

                // Reset cart items if needed
                cartItems.Clear();
               
            }
            else
                confirmBtn.Enabled = true;


         



        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ResetCart();
        }

        private void firstCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(0);
            //cartCounter = 0;

        }

        private void secondCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(1);
        }

        private void thirdCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(2);
        }

        private void fourthCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(3);
        }

        private void fifthcancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(4);
        }



        //------------------------------------------------------------------------------------------------------------------------
        // Admin Functionalities
        private async void adminIntialize()
        {
            //AddInventory sample = new AddInventory(randomData.GetInventoryItem("1"));
            //inventoryTable.Controls.Add(sample);

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM tbitems ORDER BY itemId ASC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                inventoryTable.Controls.Add(item);
            }

            admin_tabControl.SelectedTab = admin_dashboard;
            activeCBox();
            //await loadSort();
        }

        private void dashboardButton_Click(object sender, EventArgs e)
        {
            admin_tabControl.SelectedTab = admin_dashboard;
            activeCBox();
        }

        private void inventoryButton_Click(object sender, EventArgs e)
        {
            admin_tabControl.SelectedTab = admin_inventory;
            activeCBox();
        }
        private void HistoryButton_Click(object sender, EventArgs e)
        {
            admin_tabControl.SelectedTab = admin_purchaseHistory;
            activeCBox();
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
        private void receipt_Click(object sender, EventArgs e)
        {
            //addPurchase receipt = new addPurchase(receiptData);
            //receiptTable.Controls.Add(receipt);
        }

      
        //checkout tab
        private async void confirmBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = Checkout;

            // Pass REAL objects, NOT class names
            receiptData = randomData.generatePurchase(cartItems);

            pdfTemplate pdf = new pdfTemplate(receiptData);
            createPDF.generate(pdf);

            webPanel = await Web.viewPDF();
            receiptPanel.Controls.Clear();
            receiptPanel.Controls.Add(webPanel);
            webPanel.Dock = DockStyle.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {

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
            ResetCart();
           


            tabControl1.SelectedTab = tabPage3;
            receiptPanel.Controls.Remove(webPanel);

           
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

        private void adminSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<AddInventory> inventoryByDescription = Inventory.OrderBy(i => i.Description.Text).ToList();
            List<AddInventory> inventoryByCost = Inventory.OrderBy(i => i.Cost.Text).ToList();
            List<AddInventory> inventoryByStock = Inventory.OrderBy(i => Convert.ToInt32(i.Stock.Text)).ToList();

            if (admin_tabControl.SelectedTab == admin_inventory)
                switch (adminSort.SelectedItem.ToString()) 
                {
                    case "Default":
                        inventoryTable.Controls.Clear();
                        foreach (AddInventory item in Inventory) inventoryTable.Controls.Add(item);
                        break;
                    case "Alphabetical (A-Z)":
                        inventoryTable.Controls.Clear();
                        foreach (AddInventory item in inventoryByDescription) inventoryTable.Controls.Add(item);
                        break;
                    case "Alphabetical (Z-A)":
                        inventoryTable.Controls.Clear();
                        inventoryByDescription.Reverse();
                        foreach (AddInventory item in inventoryByDescription) inventoryTable.Controls.Add(item);
                        break;
                    case "By Cost (Highest)":
                        inventoryTable.Controls.Clear();
                        inventoryByCost.Reverse();
                        foreach (AddInventory item in inventoryByCost) inventoryTable.Controls.Add(item);
                        break;
                    case "By Cost (Lowest)":
                        inventoryTable.Controls.Clear();
                        foreach (AddInventory item in inventoryByCost) inventoryTable.Controls.Add(item);
                        break;
                    case "By Stock (Highest)":
                        inventoryTable.Controls.Clear();
                        inventoryByStock.Reverse();
                        foreach (AddInventory item in inventoryByStock) inventoryTable.Controls.Add(item);
                        break;
                    case "By Stock (Lowest)":
                        inventoryTable.Controls.Clear();
                        foreach (AddInventory item in inventoryByStock) inventoryTable.Controls.Add(item);
                        break;
                    //case "":
                    //    break;
                    default:
                        break;
                }
        }   

        

        //class
    }
    //namespace
}

