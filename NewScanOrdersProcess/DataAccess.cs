using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace NewScanOrdersProcess
{
    public class DataAccess
    {
        public List<ScannedItem> GetScannedItems()
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                var output = connection.Query<ScannedItem>("select PO, [Part~CustInfo] Scan, Qty, OrderID from dbo.[newscan~orders] where Updated = 'No'").ToList();
                return output;
            }
        }
    }
}
