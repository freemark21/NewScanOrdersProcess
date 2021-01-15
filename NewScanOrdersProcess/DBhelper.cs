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
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}
