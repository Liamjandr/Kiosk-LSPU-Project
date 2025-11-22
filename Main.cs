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
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }
    }
    }

