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
    public partial class Main : Form
    {
        AddCart ac;
        public int cartCounter = 0;
        public Main()
        {
            InitializeComponent();
                

        }



        private void Main_Load(object sender, EventArgs e)
        {

        }

        
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
         
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            guna2TabControl1.SelectedTab = tabPage4;
           
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            guna2TabControl1.SelectedTab = tabPage4;
        }

        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
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

      

        private void typeA_Click(object sender, EventArgs e)
        {
            ac.prodIMG.Image = typeA.Image;

            ac.prodName.Text = "MALE UNIFORM";
            ac.subName.Text = "CLOTH/FABRIC";
            ac.price.Text = "560.00";
            ac.ShowDialog();
           

        }

        private void typeB_Click(object sender, EventArgs e)
        {
            ac.prodIMG.Image = typeB.Image;

            ac.prodName.Text = "FEMALE UNIFORM";
            ac.subName.Text = "CLOTH/FABRIC";
            ac.price.Text = "560.00";
            ac.ShowDialog();

        }

        private void univ_Click(object sender, EventArgs e)
        {      
            ac.prodIMG.Image = univ.Image;

            ac.prodName.Text = "UNIVERSITY";
            ac.subName.Text = "SHIRT";
            ac.price.Text = "450.00";
            ac.ShowDialog();
        }

        private void peShirt_Click(object sender, EventArgs e)
        {
            ac.prodIMG.Image = peShirt.Image;

            ac.prodName.Text = "P.E";
            ac.subName.Text = "SHIRT";
            ac.price.Text = "360.00";
            ac.ShowDialog();
        }

        private void pePants_Click(object sender, EventArgs e)
        {
            ac.prodIMG.Image = pePants.Image;

            ac.prodName.Text = "P.E";
            ac.subName.Text = "PANTS";
            ac.price.Text = "270.00";
            ac.ShowDialog();
        }

        private void peShorts_Click(object sender, EventArgs e)
        {
            ac.prodIMG.Image = peShorts.Image;

            ac.prodName.Text = "P.E";
            ac.subName.Text = "SHORT";
            ac.price.Text = "200.00";
            ac.ShowDialog();
        }

        public void ResetCart()
        {
            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic, sixthPic, seventhPic, eighthPic, ninthPic, tenthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem, sixthItem, seventhItem, eighthItem, ninthItem, tenthIten };

            for (int i = 0; i < cartPics.Length; i++)
            {
                cartPics[i].Image = null;    
                picsPanel[i].Visible = false;
                ttext.Visible = false;
            }

            cartCounter = 0; // reset the counter
            ttext.Visible = true; // show the "empty cart" text if you have one
        }

        public void RemoveCartItem(int index)
        {
            PictureBox[] cartPics = { firstPic, secondPic, thirdPic, fourthPic, fifthPic, sixthPic, seventhPic, eighthPic, ninthPic, tenthPic };
            Panel[] picsPanel = { firstItem, secondItem, thirdItem, fourthItem, fifthItem, sixthItem, seventhItem, eighthItem, ninthItem, tenthIten };

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
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ResetCart();
        }

        private void firstCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(0);
            cartCounter = 0;
        
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

        private void sixthCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(5);
        }

        private void seventhCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(6);

        }

        private void eighthCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(7);
        }

        private void ninthCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(8);
        }

        private void tenthCancel_Click(object sender, EventArgs e)
        {
            RemoveCartItem(9);
        }
    }
    }

