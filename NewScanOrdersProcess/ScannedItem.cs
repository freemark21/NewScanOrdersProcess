using System;
using System.Collections.Generic;
using System.Text;

namespace NewScanOrdersProcess
{
    public class ScannedItem
    {
        public int OrderID { get; set; }

        public int Qty { get; set; }

        public string PO { get; set; }

        public string Scan { get; set; }

        public string ReplenexNumber { get; set; }

        public string CustomerInfo { get; set; }

    }
}
