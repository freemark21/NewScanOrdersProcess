using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;

namespace NewScanOrdersProcess
{
    class SendEmailService : ISendEmailService
    {
        private readonly ILogger<SendEmailService> _log;
        List<ItemToEmail> itemsToEmail = new List<ItemToEmail>();
        List<ItemToEmail> distinctCustomerInfo = new List<ItemToEmail>();
        List<ItemToEmail> emailingItems = new List<ItemToEmail>();
        List<Contact> contacts = new List<Contact>();
        string emailTo;
        string messageBody;
        string emailFrom = "automation@replenex.com";

        public SendEmailService(ILogger<SendEmailService> log)
        {
            _log = log;
        }

        public void Run()
        {

            DataAccess dataAccess = new DataAccess();
            itemsToEmail = dataAccess.GetTempEmailNotSent();
            distinctCustomerInfo = dataAccess.GetDistinctEmailNotSent();
            bool emailSent = false;
            foreach (ItemToEmail distinct in distinctCustomerInfo)
            {
                emailTo = null;
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Replenex", emailFrom));
                messageBody = null;
                contacts = dataAccess.GetContact(distinct.CustomerInfo);
                emailingItems = itemsToEmail.FindAll(x => x.CustomerInfo == distinct.CustomerInfo);
                foreach (ItemToEmail item in emailingItems)
                {
                    messageBody += $"{item.CustomerInfo}, {item.ReplenexNumber}, {item.Qty}, /N ";

                }
                if (contacts.Count > 0)
                {

                    foreach (Contact contact in contacts)
                    {
                        message.To.Add(new MailboxAddress(contact.FirstName + contact.LastName, contact.Email));

                        message.Body = new TextPart("plain")
                        {
                            Text = $"{messageBody}"
                        };
                    }
                }
                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                        client.Authenticate();
                        client.Send(message);
                        client.Disconnect(true);
                        emailSent = true;
                    }

                }
                catch (Exception e)
                {
                    _log.LogInformation("Error sending email for {CustomerInfo} {ErrorMsg}", distinct.CustomerInfo, e);
                    emailSent = false;
                }
            }

            foreach (ItemToEmail item in emailingItems)
            {
                if (emailSent == true)
                {
                    dataAccess.ChangeEmailSentToYes(item.OrderID);
                }
            }
        }
    }
}
