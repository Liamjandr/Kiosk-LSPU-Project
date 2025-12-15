using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using Paymongo.Sharp.Features.Checkouts.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kiosk
{
    public partial class addPurchase : UserControl
    {

        public string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";
        string isPaid { get; set; }
        string isClaimed { get; set; }
        List<HistoryDB> receiptItems { get; set; }

        string transactionType;
        public addPurchase(ReceiptGroup history)
        {
            InitializeComponent();

            receiptItems = history.Items;
            ReceiptID.Text = history.ReceiptID;
            Date.Text = history.ReceiptDate.ToString("MM/dd/yyyy");

            transactionType = history.Transaction;    
            transac.Text = transactionType;

            isPaid = history.Items[0].isPaid;
            isClaimed = history.Items[0].isClaimed;

            decimal total = 0;
            foreach (var item in history.Items)
                total += item.Quantity * item.Price;

            Cost.Text = "₱" + total.ToString("F2");

            UpdatePaymentUI();
            UpdateClaimUI();

            // Lock payment if NOT cash/counter
            //if (transactionType.Equals("CASH/COUNTER", StringComparison.OrdinalIgnoreCase))
            //{
            //    Payment.Enabled = false;
            //    Payment.FillColor = Color.Gray;


            //}

        }



        private void UpdatePaymentUI()
        {
            if (isPaid == "true")
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

        private void UpdateClaimUI()
        {
            if (isClaimed == "true")
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
            // Only for CASH/COUNTER transactions that are unpaid
            if (transactionType.Equals("CASH/COUNTER", StringComparison.OrdinalIgnoreCase) && isPaid == "false")
            {
                var main = this.FindForm() as Main;
                if (main == null) return;

                adminCounterPaying counterModal = new adminCounterPaying(ReceiptID.Text);

                counterModal.PaymentConfirmed += () =>
                {
                    isPaid = "true";
                    UpdatePaymentUI();
                };

                main.Controls.Add(counterModal);
                int x = (main.ClientSize.Width - counterModal.Width) / 2;
                int y = (main.ClientSize.Height - counterModal.Height) / 2;
                counterModal.Location = new Point(x, y);
                counterModal.BringToFront();
                return;
            }

            // For all other cases, toggle payment normally
            isPaid = (isPaid == "false") ? "true" : "false";
            changeStatePaid(ReceiptID.Text, isPaid);
            UpdatePaymentUI();
        }


        private void Claim_Click(object sender, EventArgs e)
        {
            if(isClaimed == "false") isClaimed = "true";
            else isClaimed = "false";
            changeStateClaim(ReceiptID.Text, isClaimed);
            if (isClaimed == "true")
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

        private void changeStatePaid(string id, string paid)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();

                    string query = @"
                        UPDATE tbhistory
                        SET isPaid = @paid
                        WHERE ReceiptID = @receiptID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@paid", paid);
                        cmd.Parameters.AddWithValue("@receiptID", id);

                        int rows = cmd.ExecuteNonQuery();
                        //MessageBox.Show("Updated rows: " + rows);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Input: " + ex.Message);
                return;
            }
        }

        private void changeStateClaim(string id, string claim)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mycon))
                {
                    conn.Open();

                    string query = @"
                        UPDATE tbhistory
                        SET isClaimed = @claim
                        WHERE ReceiptID = @receiptID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@claim", claim);
                        cmd.Parameters.AddWithValue("@receiptID", id);

                        int rows = cmd.ExecuteNonQuery();
                        //MessageBox.Show("Updated rows: " + rows);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Input: " + ex.Message);
                return;
            }
        }
    }
}
