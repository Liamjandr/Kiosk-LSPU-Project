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

namespace kiosk
{
    public partial class addPurchase : UserControl
    {
        receiptTemplate receipt { get; }
        int orderIndex;
        bool isPaid { get; set; }
        bool isClaimed { get; set; }
        public addPurchase(receiptTemplate receipt, int orderIndex)
        {
            InitializeComponent();
            this.receipt = receipt;
            this.orderIndex = orderIndex;

            ReceiptID.Text = receipt.receiptID;
            Type.Text = receipt.Items[orderIndex].Type;
            Description.Text = receipt.Items[orderIndex].Name;
            Date.Text = receipt.receiptDate.ToString("MM/dd/yyyy");
            Quantity.Text = receipt.Items[orderIndex].Quantity.ToString();
            Cost.Text = receipt.Items[orderIndex].Price.ToString("C2");
            if (isPaid)
            {
                Payment.Text = "PAID";
                Payment.FillColor = Color.Green;
            }
            else
            {
                Payment.Text = "UNPAID";
                Payment.FillColor = Color.Red;
            }
            if (isClaimed)
            {
                Claim.Text = "Claimed";
                Claim.FillColor = Color.Green;
            }
            else
            {
                
                Claim.Text = "Not Claimed";
                Claim.FillColor = Color.Red;
            }
        }

        private void Payment_Click(object sender, EventArgs e)
        {
            isPaid = !isPaid;
            if (isPaid)
            {
                Payment.Text = "PAID";
                Payment.FillColor = Color.Green;
            }
            else
            {
                Payment.Text = "UNPAID";
                Payment.FillColor = Color.Red;
            }
        }

        private void Claim_Click(object sender, EventArgs e)
        {
            isClaimed = !isClaimed;
            if (isClaimed)
            {
                Claim.Text = "Claimed";
                Claim.FillColor = Color.Green;
            }
            else
            {

                Claim.Text = "Not Claimed";
                Claim.FillColor = Color.Red;
            }
        }
    }
}
