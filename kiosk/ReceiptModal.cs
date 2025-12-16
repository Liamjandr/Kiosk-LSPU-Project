using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace kiosk
{
    public partial class ReceiptModal : UserControl
    {
        private int FormRadius = 20;
        createPDF createPDF = new createPDF();
        WebView2 webPanel;
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
        public ReceiptModal(ReceiptGroup receipt)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            int radius = 40;
            this.Region = RoundRegion(radius);
            ReceiptModal_Load(receipt);
        }

        public async Task ReceiptModal_Load(ReceiptGroup receiptGroup)
        {
            receiptTemplate receipt = new receiptTemplate{ 
                receiptID = receiptGroup.ReceiptID,
                receiptDate = receiptGroup.ReceiptDate,
                
                TotalAmount = receiptGroup.Items[0].TotalAmount,
                Cash = receiptGroup.Items[0].Cash,
                Change = receiptGroup.Items[0].Change,   
                Items = receiptGroup.Items.Select(item => new OrderItem
                {
                    ItemID = item.ItemID,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Size = item.Size,
                    Type = item.Type,   
                }).ToList()
            };
            pdfTemplate pdf = new pdfTemplate(receipt);
            createPDF.generate(pdf);

            webPanel = await Web.viewPDF();



            panel1.Controls.Clear();
            panel1.Controls.Add(webPanel);
            webPanel.Dock = DockStyle.Fill;
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
