using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace NewScanOrdersProcess
{
    public static class DBhelper
    {
        public static string CnnVal (string name)
        {
            return "Server=REP-WWW; Database=NewScanOrders; Trusted_Connection=True;";
        }

    }
}
