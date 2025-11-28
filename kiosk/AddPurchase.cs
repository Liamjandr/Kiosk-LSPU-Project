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
    public partial class addPurchase : UserControl
    {
        HistoryItem historyItem;
        public addPurchase(HistoryItem item)
        {
            InitializeComponent();
            historyItem = item;
            ReceiptID.Text = historyItem.ID;
            Type.Text = historyItem.Type;
            Description.Text = historyItem.Description;
            Date.Text = historyItem.Date.ToString("MM/dd/yyyy");
            Quantity.Text = historyItem.QTY.ToString();
            Cost.Text = historyItem.Cost.ToString("C2");
            if (historyItem.isPaid)
            {
                Payment.Text = "PAID";
                Payment.FillColor = Color.Green;
            }
            else
            {
                Payment.Text = "UNPAID";
                Payment.FillColor = Color.Red;
            }
            if (historyItem.isClaimed)
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
            historyItem.isPaid = !historyItem.isPaid;
            if (historyItem.isPaid)
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
            historyItem.isClaimed = !historyItem.isClaimed;
            if (historyItem.isClaimed)
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
