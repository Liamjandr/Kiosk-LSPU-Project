using Paymongo.Sharp.Features.Checkouts.Contracts;
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

        bool isPaid { get; set; }
        bool isClaimed { get; set; }
        List<HistoryDB> receiptItems { get; set; }
        public addPurchase(ReceiptGroup history)
        {
            InitializeComponent();

            //tofix
            receiptItems = history.Items;
            ReceiptID.Text = history.ReceiptID;
            Date.Text = history.ReceiptDate.ToString("MM/dd/yyyy");
            decimal total = 0;
            foreach (var item in history.Items) total += item.Quantity * item.Price;
            Cost.Text = "₱"+total.ToString("F2");

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

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            var main = this.FindForm() as Main;
            if (main == null) return;
            main.showReceiptModal(receiptItems);
        }
    }
}
