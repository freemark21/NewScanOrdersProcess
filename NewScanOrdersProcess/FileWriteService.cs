using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace NewScanOrdersProcess
{
    public class FileWriteService : IFileWriteService
    {
        private readonly ILogger<FileWriteService> _log;
        readonly char delimiterChar = '~';
        List<ScannedItem> scannedItems = new List<ScannedItem>();

        public FileWriteService(ILogger<FileWriteService> log)
        {
            _log = log;
        }
        public void Run()
        {
            string fileName = DateTime.Now.ToString("yyyyMMddThhmmss");
            DataAccess dataAccess = new DataAccess();
            scannedItems = dataAccess.GetScannedItems();
            if (scannedItems.Count > 0)
            {

                using FileStream filestream = new FileStream(@"\\REP-APP\sftp_root\ftpuser\kanban\" + fileName + ".web", FileMode.Create, FileAccess.Write);
                using StreamWriter streamWriter = new StreamWriter(filestream);
                foreach (ScannedItem scannedItem in scannedItems)
                {

                    try
                    {
                        string[] scanSplit = scannedItem.Scan.Split(delimiterChar);
                        scannedItem.ReplenexNumber = scanSplit[0];
                        scannedItem.CustomerInfo = scanSplit[1];
                        streamWriter.WriteLine($"{ scannedItem.ReplenexNumber },{ scannedItem.CustomerInfo },{ scannedItem.Qty },{ scannedItem.PO }");
                    }
                    catch (Exception e)
                    {
                        _log.LogError($"Unable to write {scannedItem.OrderID} due to {e}");
                        break;
                    }
                    dataAccess.ChangeUpdatedToYes(scannedItem.OrderID);
                }
            }

        }
    }
}
