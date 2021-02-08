using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NewScanOrdersProcess
{
    public class DataAccess
    {
        public List<ScannedItem> GetScannedItems()
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                var output = connection.Query<ScannedItem>("dbo.spnewscanorders_GetNotUpdated").ToList();
                return output;
            }
        }

        public void ChangeUpdatedToYes(int orderID)
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                connection.Query<ScannedItem>("spnewscanorders_ChangeUpdatedToYes @OrderID", new { OrderID = orderID });
            }
        }

        public List<ItemToEmail> GetTempEmailNotSent()
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                connection.Open();
                connection.Query("spnewscanorders_CreateTempEmailNotSent");
                var output = connection.Query<ItemToEmail>("spnewscanorders_GetTempEmailNotSent").ToList();
                return output;
            }
        }

        public List<ItemToEmail> GetDistinctEmailNotSent()
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                connection.Open();
                connection.Query("spnewscanorders_CreateTempEmailNotSent");
                var output = connection.Query<ItemToEmail>("spnewscanorders_GetDistinctEmailNotSent").ToList();
                return output;
            }
        }


        public void ChangeEmailSentToYes(int orderID)
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                connection.Query<ScannedItem>("spnewscanorders_ChangeEmailSentToYes @OrderID", new { OrderID = orderID });
            }
        }

        public List<Contact> GetContact(string customerInfo)
        {
            using (IDbConnection connection = new SqlConnection(DBhelper.CnnVal("newscanorders")))
            {
                var output = connection.Query<Contact>("dbo.spnewscanorders_GetContact @CustomerInfo", new { CustomerInfo = customerInfo }).ToList();
                return output;
            }
        }
    }
}
