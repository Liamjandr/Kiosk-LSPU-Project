using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Paymongo.Sharp.Features.PaymentMethods.Contracts;

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
            resultLabel.Text = "";

            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Region = RoundRegion(40);

            Paymongo = new Paymongo();

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
            textBox1.Text = "Please pay using the following link: " + checkoutUrl;

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

                            myconn.SaveReceipt(receiptData, main.paymentMethod, "true");
                        }
                        else
                        {
                            result = "Payment Failed!";
                        }
                    }
                    else
                    {
                        result = await Paymongo.CheckExpire();
                        transacStatus = false;
                    }

                    resultLabel.Text = result;
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


    }
}
