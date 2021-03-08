using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ChatbotCustomerOnboarding.DataModel;

/* Sends Email once the Quote is confirmed/
   ///-----------------------------------------------------------------
   ///   Namespace:      <ChatbotCustomerOnboarding>
   ///   Class:          <EmailHelper>
   ///   Description:    <Sends email using SMTP>
   ///   Author:         <Vignesh Chandran balan>                    
 ///-----------------------------------------------------------------*/


namespace ChatbotCustomerOnboarding.BotHelpers
{
    public class EmailService
    {
        public static async Task<bool> SendEmail(string quoteCustomer)
        {
            try
            {
                string smtpAddress = "smtp.mail.yahoo.com";
                int portNumber = 587;
                bool enableSSL = true;

                string emailFrom = "projectswen2021@yahoo.com";
                string password = "gmsakxqsnihlkugb";
                string emailTo = CustomerInfo.EmailAddress.ToString();
                string subject = $"Here is your Quote: {quoteCustomer} CustomerID: {CustomerInfo.CustomerId}";
                string body = "CustomerName: " + CustomerInfo.FirstName + " " + CustomerInfo.LastName + "\n";
                body += "Email: " + CustomerInfo.EmailAddress + "\n";
                body += "Message: \n" + "Demo: Onboarding Customer feature 2 Assignment" + "\n";
                //   string[] cc = { "PetrenkoO6197@UHCL.edu", "ObryantJ2383@UHCL.edu", "murukutlas3934@uhcl.edu" };

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    //foreach (var person in cc)
                    //    mail.CC.Add(new MailAddress(person));
                    //mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return false;

            }
        }
    }
}
