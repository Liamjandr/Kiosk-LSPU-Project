using Guna.UI2.WinForms;
using Paymongo.Sharp.Features.PaymentMethods.Contracts;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kiosk
{
    public partial class paymentControl : UserControl
    {
        private int FormRadius = 20;

        private string checkoutUrl;
        private bool transacStatus;

        private Panel receiptTable { get; }
        private receiptTemplate receiptData;
        private kioskQR kioskQR = new kioskQR();
        private Paymongo Paymongo;

        public paymentControl(receiptTemplate receipt, Panel table)
        {
            InitializeComponent();
            this.receiptData = receipt;
            this.receiptTable = table;
            
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Region = RoundRegion(38);
            print.Visible = false;
            Paymongo = new Paymongo();

            loadOrderSummary(receipt);
            _ = UserControlLoad(); // fire-and-forget async
        }

        // Rounded corners
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = RoundRegion(FormRadius);
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

        // Load the payment and QR
        public async Task UserControlLoad()
        {
            textBox1.Text = "Creating Checkout...";
            checkoutUrl = await Paymongo.CreateCheckout(receiptData);
            textBox1.Text = "On going payment process....";
            boxstatus(Color.FromArgb(240, 239, 239), Color.FromArgb(202, 202, 202));
            string qrPath = await kioskQR.GenerateQRCode(checkoutUrl);
            using (var fs = new FileStream(qrPath, FileMode.Open, FileAccess.Read))
            {
                Image img = Image.FromStream(fs);
                qrPictureBox.Image = new Bitmap(img);
            }

            _ = Task.Run(async () =>
            {
                var cancelProcess = new CancellationTokenSource();
                var paymentTask = Paymongo.CheckPayment(cancelProcess.Token);
                var expireTime = Task.Delay(300000); // 5 minutes
                var ifExpire = await Task.WhenAny(paymentTask, expireTime);

                Invoke(new Action(async () =>
                {
                    string result = "";
                    Main main = this.FindForm() as Main;
                    if (main == null) return;

                    if (ifExpire == paymentTask)
                    {
                        bool paymentResult = await paymentTask;
                        transacStatus = paymentResult;

                        if (paymentResult)
                        {
                            result = "Payment Successful!";
                            print.Visible = true;
                            button1.Visible = false;
                            boxstatus(Color.FromArgb(240, 253, 244), Color.FromArgb(185, 248, 207));
                            myconn.SaveReceipt(receiptData, main.paymentMethod, "true");
                        }
                        else
                        {
                            boxstatus(Color.FromArgb(253, 240, 240), Color.FromArgb(248, 185, 185));
                            result = "Payment Failed!";
                        }
                    }
                    else
                    {
                        boxstatus(Color.FromArgb(253, 240, 240), Color.FromArgb(248, 185, 185));
                        result = await Paymongo.CheckExpire();
                        transacStatus = false;
                    }

                    textBox1.Text = result;
                }));
            });
        }

        public bool getStatus() => transacStatus;

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(checkoutUrl);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void print_Click(object sender, EventArgs e)
        {
            Main main = this.FindForm() as Main;
            if (main == null) return;

            // 1️⃣ Update Main UI immediately
            main.button2.Visible = false;
            main.Printbutton.Visible = false;
            main.buybutton.Visible = false;
            main.again.Location = new Point(814, 976);

            // 2️⃣ Remove this control from parent
            if (this.Parent != null)
                this.Parent.Controls.Remove(this);

            // 3️⃣ Start printing
            ShowPrintingMessage(main);
        }

        private async void ShowPrintingMessage(Main main)
        {
            // Show printing popup
            Form printingForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(280, 110),
                BackColor = Color.White,
                TopMost = true
            };

            Label lbl = new Label
            {
                Text = "Printing...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            printingForm.Controls.Add(lbl);
            printingForm.Show();

            await Task.Delay(3000); // simulate printing
            printingForm.Close();

            // Show countdown **after printing**
            main.ShowCountdownMessage();

            // Optional: hide buttons in main UI after printing if needed
            print.Visible = true;
            button1.Visible = false;
        }

        private void guna2Shapes1_Click(object sender, EventArgs e)
        {

        }

        private void boxstatus(Color fillColor, Color borderColor)
        {
            guna2Shapes1.FillColor = fillColor;
            guna2Shapes1.BorderColor = borderColor;
            label3.BackColor = fillColor;
            textBox2.BackColor = fillColor;
            textBox1.BackColor = fillColor;
            label4.BackColor = fillColor;
        }
        private void loadOrderSummary(receiptTemplate receipt)
        {
            receiptID.Text = receipt.receiptID;
            string[] name = new string[receipt.Items.Count];
            string[] qty = new string[receipt.Items.Count];
            string[] cost = new string[receipt.Items.Count];
            int subtotal = 0;
            for (int i = 0; i < receipt.Items.Count; i++)
            {
                subtotal += Convert.ToInt32(receipt.Items[i].Price) * receipt.Items[i].Quantity;
                name[i] = receipt.Items[i].Name;
                qty[i] = receipt.Items[i].Quantity.ToString();
                cost[i] = receipt.Items[i].Price.ToString();
            }


            itemNames.Lines = name;
            itemQTY.Lines = qty;
            itemCosts.Lines = cost;
            int dis = 0; // Placeholder for future discount logic
            Subtotal.Text = "₱" + subtotal.ToString("F2");
            discount.Text = "₱" + dis.ToString("F2");
            total.Text = "₱" + (subtotal-dis).ToString("F2");
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
