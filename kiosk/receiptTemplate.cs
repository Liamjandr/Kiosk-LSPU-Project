using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk
{
    public class receiptTemplate
    {
        public string receiptID { get; set; }
        public DateTime receiptDate { get; set; }
        public List<OrderItem> Items { get; set; }

        //to remove
        public studentInfo Student { get; set; }
    }

    public class OrderItem
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class studentInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }

    }
}
