using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace kiosk
{
    internal class myconn
    {
        public MySqlConnection con;
        public MySqlDataReader dr;
        public MySqlCommand cmd;
        public DataTable dt;

        public void connect()
        {
            con = new MySqlConnection("datasource=localhost;Database=dbkiosk;username=root");
            con.Open();
        }

        public void Disconnect()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
            con.Dispose();
        }

        public static void SaveReceipt(receiptTemplate receipt, string transactionId)
        {
            string mycon = "server=localhost;Database=dbkiosk;Uid=root;Convert Zero Datetime=True;";

            using (MySqlConnection conn = new MySqlConnection(mycon))
            {
                conn.Open();

                using (var sqlTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in receipt.Items)
                        {
                            if (item == null) continue;

                            // optional: assign a temporary ItemID if missing
                            if (item.ItemID == 0)
                                item.ItemID = new Random().Next(1000, 9999);

                            string query = @"
                    INSERT INTO tbHistory
                    (`ReceiptID`, `DateTime`, `Transaction`, `ItemID`, `itemName`, `itemType`, `itemSize`, `itemQTY`, `itemPrice`, `Total`, `Cash`, `Change`)
                    VALUES
                    (@ReceiptID, @DateTime, @Transaction, @ItemID, @itemName, @itemType, @itemSize, @itemQTY, @itemPrice, @Total, @Cash, @Change)";

                            using (MySqlCommand cmd = new MySqlCommand(query, conn, sqlTransaction))
                            {
                                cmd.Parameters.AddWithValue("@ReceiptID", receipt.receiptID);
                                cmd.Parameters.AddWithValue("@DateTime", receipt.receiptDate);
                                cmd.Parameters.AddWithValue("@Transaction", transactionId);
                                cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                                cmd.Parameters.AddWithValue("@itemName", item.Name);
                                cmd.Parameters.AddWithValue("@itemType", item.Type);
                                cmd.Parameters.AddWithValue("@itemSize", item.Size);
                                cmd.Parameters.AddWithValue("@itemQTY", item.Quantity);
                                cmd.Parameters.AddWithValue("@itemPrice", item.Price);
                                cmd.Parameters.AddWithValue("@Total", receipt.TotalAmount);
                                cmd.Parameters.AddWithValue("@Cash", receipt.Cash);
                                cmd.Parameters.AddWithValue("@Change", receipt.Change);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
