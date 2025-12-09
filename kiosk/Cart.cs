using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace kiosk
{

    public class Cart
    {
        private readonly string itemImagePath = Path.Combine(Application.StartupPath, "images_rsrcs", "itemPics");

        string mycon = "datasource=localhost;Database=dbkiosk;username=root;convert zero datetime=true";
        public static receiptTemplate generatePurchase(
    List<CartItem> cart)
        //studentInfo studentInfoObject;
        {
            Random rand = new Random();
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

            string receiptID = "";
            for (int i = 0; i < 8; i++)
                receiptID += chars[rand.Next(chars.Length)];

            decimal TotalAmount = cart.Sum(item => item.Price * item.Quantity);

            return new receiptTemplate
            {
                receiptID = receiptID,
                receiptDate = DateTime.Now,
                TotalAmount = TotalAmount,
                Cash = 0,            // you can update later
                Change = 0,          // you can update later    
                Items = cart.Select(item => new OrderItem
                {
                    ItemID = item.ItemID,
                    Type = item.Type,
                    Name = item.Name,
                    Size = item.Size,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList(),
            };
        }


        //public List<CartItem> GetCartItems()
        //{
        //    Main m = new Main();

        //    List<CartItem> list = new List<CartItem>();
        //    Label[] productID = { m.firstID, m.secondID, m.thirdID, m.fourthID, m.fifthID };
        //    Label[] productName = { m.firstItemName, m.secondItemName, m.thirdItemName, m.fourthItemName, m.fifthItemName };
        //    Label[] prodType = { m.firstItemType, m.secondItemType, m.thirdItemType, m.fourthItemType, m.fifthItemType };
        //    Label[] prodQty = { m.firstItemQty, m.secondItemQty, m.thirdItemQty, m.fourthItemQty, m.fifthItemQty };
        //    Label[] prodSize = { m.firstItemSize, m.secondItemSize, m.thirdItemSize, m.fourthItemSize, m.fifthItemSize };
        //    Label[] prodPrice = { m.firstItemPrice, m.secondItemPrice, m.thirdItemPrice, m.fourthItemPrice, m.fifthItemPrice };

        //    for (int i = 0; i < productName.Length; i++)
        //    {
        //        if (!string.IsNullOrWhiteSpace(productName[i].Text))
        //        {
        //            list.Add(new CartItem
        //            {
        //                ItemID = int.Parse(productID[i].Text),
        //                Name = productName[i].Text,
        //                Type = prodType[i].Text,
        //                Size = prodSize[i].Text,
        //                Quantity = int.Parse(prodQty[i].Text),
        //                Price = decimal.Parse(prodPrice[i].Text)
        //            });
        //        }
        //    }

        //    return list;
        //}
    }
    public class CartItem
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
