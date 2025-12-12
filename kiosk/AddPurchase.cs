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
        public addPurchase(ReceiptGroup history)
        {
            InitializeComponent();

            //tofix
            ReceiptID.Text = history.ReceiptID;
            Type.Text = history.Items[0].Type;
            Description.Text = history.Items[0].Name;
            Date.Text = history.ReceiptDate.ToString("MM/dd/yyyy");
            Quantity.Text = history.Items[0].Quantity.ToString();
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
        ////Receipt Info  
        //public string receiptID { get; set; }
        //public DateTime receiptDate { get; set; }

        ////Item Info
        //public int ItemID { get; set; }
        //public string Name { get; set; }
        //public string Type { get; set; }
        //public decimal Price { get; set; }
        //public int Quantity { get; set; }
        //public string Size { get; set; }


        ////Transaction Info
        //public string transactionId { get; set; }
        //public decimal TotalAmount { get; set; }
        //public decimal Cash { get; set; }
        //public decimal Change { get; set; }
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
