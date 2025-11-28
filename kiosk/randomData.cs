using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk
{
    public class randomData
    {
        //generate random values for purchase history
        public static HistoryItem generateHistory()
        {
            Random rand = new Random();
            char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string ID = "";
            string type = "";
            string description = "";
            int qty = rand.Next(1,4);
            decimal price = rand.Next(40,450);
          
            bool isPaid = rand.Next(0, 2) == 1 ? true : false;
            bool isClaimed = rand.Next(0, 2) == 1 ? true : false;
            String[] types = { "ID Lace", "Shirt", "Pants"};
            String[] laceDescription = { "CCS ID Lace", "CCJE ID Lace", "CIT ID Lace", "CHMT ID Lace" };
            String[] shirtDescription = { "CCS Dep Shirt", "CCJE Dep Shirt", "CIT Dep Shirt", "CHMT Dep Shirt" };
            String[] pantsDescription = { "CCS Pants", "CCJE Pants", "CIT Pants", "CHMT Pants" };
            List<String[]> descriptions = new List<String[]>();
            descriptions.Add(laceDescription);
            descriptions.Add(shirtDescription);
            descriptions.Add(pantsDescription);
            int randTypeIndex = rand.Next(types.Length);
            type = types[randTypeIndex];
            string[] selectedDes = descriptions[randTypeIndex];
            description = selectedDes[rand.Next(selectedDes.Length)];
            for (int i = 0; i < 8; i++)
            {
                int random = rand.Next(characters.Length);
                ID += characters[random];
            }
            return new HistoryItem
            {
                ID = ID,
                Type = type,
                Description = description,
                Date = DateTime.Now.AddDays(-rand.Next(1, 365)),
                QTY = qty,
                Cost = price,
                isPaid = isPaid,
                isClaimed = isClaimed
            };
        }

        public static InventoryItem GetInventoryItem(string inputID) {
            if (inputID == "1")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "ID Lace",
                    Description = "CCS ID Lace",
                    Stock = 100,
                    Price = 50.99m,
                    isEnable = true
                };
            if (inputID == "2")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "ID Lace",
                    Description = "CCJE ID Lace",
                    Stock = 180,
                    Price = 50.99m,
                    isEnable = true
                };
            if (inputID == "3")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "ID Lace",
                    Description = "CIT ID Lace",
                    Stock = 610,
                    Price = 50.99m,
                    isEnable = true
                };
            if (inputID == "4")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "ID Lace",
                    Description = "SCHMT ID Lace",
                    Stock = 180,
                    Price = 50.99m,
                    isEnable = true
                };
            if (inputID == "5")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Shirt",
                    Description = "CCS Dep Shirt",
                    Stock = 10,
                    Price = 449.99m,
                    isEnable = true
                };
            if (inputID == "6")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Shirt",
                    Description = "CCJE Dep Shirt",
                    Stock = 10,
                    Price = 449.99m,
                    isEnable = true
                };
            if (inputID == "7")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Shirt",
                    Description = "CIT Dep Shirt",
                    Stock = 10,
                    Price = 449.99m,
                    isEnable = true
                };
            if (inputID == "8")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Shirt",
                    Description = "CHMT Dep Shirt",
                    Stock = 10,
                    Price = 449.99m,
                    isEnable = true
                };
            if (inputID == "9")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Pants",
                    Description = "CCS Pants",
                    Stock = 8,
                    Price = 299.99m,
                    isEnable = true
                };
            if (inputID == "10")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Pants",
                    Description = "CCJE Pants",
                    Stock = 8,
                    Price = 299.99m,
                    isEnable = true
                };
            if (inputID == "11")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Pants",
                    Description = "CIT Pants",
                    Stock = 8,
                    Price = 299.99m,
                    isEnable = true
                };
            if (inputID == "12")
                return new InventoryItem
                {
                    ID = inputID,
                    Type = "Pants",
                    Description = "CHMT Pants",
                    Stock = 8,
                    Price = 299.99m,
                    isEnable = true
                };

            //String[] types = { "ID Lace", "Shirt", "Pants" };
            //String[] laceDescription = { "CCS ID Lace", "CCJE ID Lace", "CIT ID Lace", "CHMT ID Lace" };
            //String[] shirtDescription = { "CCS Dep Shirt", "CCJE Dep Shirt", "CIT Dep Shirt", "CHMT Dep Shirt" };
            //String[] pantsDescription = { "CCS Pants", "CCJE Pants", "CIT Pants", "CHMT Pants" };        
            return new InventoryItem
            {
                ID = inputID,
                Type = "Unknown",
                Description = "Unknown",
                Stock = 0,
                Price = 0.00m,
                isEnable = false
            };
        }
    }


    public class HistoryItem
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int QTY { get; set; }
        public decimal Cost { get; set; }
        public bool isPaid { get; set; }
        public bool isClaimed { get; set; }
    }

    public class InventoryItem
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public bool isEnable { get; set; }
    }
}
