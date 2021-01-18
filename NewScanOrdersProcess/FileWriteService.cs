using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewScanOrdersProcess
{
    public class FileWriteService : IFileWriteService
    {
        private readonly ILogger<FileWriteService> _log;

        List<ScannedItem> scannedItems = new List<ScannedItem>();

        public FileWriteService(ILogger<FileWriteService> log)
        {
            _log = log;
        }
        public void Run()
        {

            DataAccess dataAccess = new DataAccess();
            scannedItems = dataAccess.GetScannedItems();

            foreach (ScannedItem scannedItem in scannedItems)
            {
                _log.LogInformation($"{scannedItem.PO} {scannedItem.Qty} {scannedItem.Scan}");
            }
        }
    }
}
