# NewScanOrdersProcess

I've set up and configured a SQL server and IIS 10 to host a coworkers PHP application. The PHP application allows on site service providers and customers to scan orders into a database.

This program will take any rows with Updated column = "no" and write them in the specified format to a directory the ERP is watching, then update the Updated column = "yes".

This console app will be triggered 4 times throughout the day using Windows Task Scheduler.

I am using Dapper to connect, as this is lightweight DB work.
