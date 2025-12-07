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
using System.IO;

namespace kiosk
{
    public partial class paymentControl : UserControl
    {
        Panel receiptTable { get; }
        receiptTemplate receiptData;
        private string checkoutUrl;
        kioskQR kioskQR = new kioskQR();
        Paymongo Paymongo;
        List<addPurchase> purchaseList = new List<addPurchase>();
        public paymentControl(receiptTemplate receipt, Panel table)
        {
            InitializeComponent();
            this.receiptData = receipt;
            this.receiptTable = table;
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
                            for (int i = 0; i < receiptData.Items.Count; i++)
                            {
                                purchaseList.Add(new addPurchase(receiptData, i));
                            }
                            foreach(addPurchase purchase in purchaseList)
                            {
                                receiptTable.Controls.Add(purchase);
                            }
                        }
                        else result = "Payment Failed!";
                    }
                    else
                    {
                        result = await Paymongo.CheckExpire();
                    }

                    richTextBox1.Text = result;
                }));
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(checkoutUrl);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }
         
    }
}
