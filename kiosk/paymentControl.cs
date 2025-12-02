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
    public partial class paymentControl : UserControl
    {
        receiptTemplate receiptData;
        Panel receiptTable { get; }
        int orderIndex;
        private string checkoutUrl;
        Paymongo Paymongo;
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

            _ = Task.Run(async () =>
            {
                string result = await Paymongo.CheckPayment();

                // Return to UI thread
                Invoke(new Action(() =>
                {
                    richTextBox1.Text = result;

                    for (int i = 0; i < receiptData.Items.Count; i++)
                    {
                        addPurchase receipt = new addPurchase(receiptData, i);
                        receiptTable.Controls.Add(receipt);
                    }
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
