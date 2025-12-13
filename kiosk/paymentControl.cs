using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Navigation;
using Paymongo.Sharp.Features.PaymentMethods.Contracts;

namespace kiosk
{
    public partial class paymentControl : UserControl
    {
        //Drawing rounded corners
        private int FormRadius = 20;
       
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


        private string checkoutUrl;
        private bool transacStatus;

        Panel receiptTable { get; }
        receiptTemplate receiptData;
        kioskQR kioskQR = new kioskQR();
        Paymongo Paymongo;
        //List<addPurchase> purchaseList = new List<addPurchase>();

        public paymentControl(receiptTemplate receipt, Panel table)
        {
            InitializeComponent();
            this.receiptData = receipt;
            this.receiptTable = table;
            resultLabel.Text = "";
         
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            int radius = 40;
            this.Region = RoundRegion(radius);


            Paymongo = new Paymongo();

            UserControlLoad();
        }


        public async void UserControlLoad()
        {
            textBox1.Text = "Creating Checkout...";
            checkoutUrl = await Paymongo.CreateCheckout(receiptData);
            textBox1.Text = "Please pay using the following link: " + checkoutUrl;
            string qrPath = await kioskQR.GenerateQRCode(checkoutUrl);
            using (var fs = new FileStream(qrPath, FileMode.Open, FileAccess.Read))
            {
                Image img = Image.FromStream(fs);
                qrPictureBox.Image = new Bitmap(img); //avoid locking
            }

            _ = Task.Run(async () =>
            {
                string result = "";
                var cancelProcess = new CancellationTokenSource();
                var payment = Paymongo.CheckPayment(cancelProcess.Token);
                var expireTime = Task.Delay(30000); //120000 ms = 2 minutes
                var ifExpire = await Task.WhenAny(payment, expireTime);
                //Image img = File.Exists(fullPath) ? Image.FromFile(fullPath) : null;

                // Return to UI thread
                Invoke(new Action(async () =>
                {
                    if (ifExpire == payment)
                    {
                        bool paymentResult = await payment;
                        if (paymentResult)
                        {
                            result = "Payment Successful!";
                            transacStatus = false;

                            Main m = new Main();

                            //Temporary save to database
                            myconn.SaveReceipt(receiptData, m.paymentMethod);

                            //tofix
                            //for (int i = 0; i < receiptData.Items.Count; i++)
                            //{
                            //    purchaseList.Add(new addPurchase(receiptData, i));
                            //}
                            //foreach(addPurchase purchase in purchaseList)
                            //{
                            //    receiptTable.Controls.Add(purchase);
                            //}
                        }
                        else result = "Payment Failed!";
                        transacStatus = false;
                    }
                    else
                    {
                        result = await Paymongo.CheckExpire();
                        transacStatus = false;
                    }

                    this.resultLabel.Text = result;
                }));
            });
        }

        public bool getStatus()
        {
            return transacStatus;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(checkoutUrl);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }
    }
}
