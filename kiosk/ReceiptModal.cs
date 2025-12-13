using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kiosk
{
    public partial class ReceiptModal : UserControl
    {
        private int FormRadius = 20;
        List<HistoryDB> listedItems = new List<HistoryDB>();
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

        public ReceiptModal(List<HistoryDB> items)
        {
            InitializeComponent();
            listedItems = items;
            this.DoubleBuffered = true;
            int radius = 40;
            this.Region = RoundRegion(radius);
            ReceiptModal_Load();
        }

        public void ReceiptModal_Load()
        {
           dataGridView1.DataSource = listedItems;
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

            
    }

    public class getAdminComponents
    {
        TabPage page;
        
        public void setPage(TabPage adminPage)
        {
            this.page = adminPage;
        }
    }

}
