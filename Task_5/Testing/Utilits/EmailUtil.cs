using System;
using MailKit.Security;
using MailKit.Net.Pop3;
using Test.Framework.Configuration;
using Test.Framework.Logging;
using Polly;
using Polly.Retry;
namespace Task_5.Utilits
{
    public static class EmailUtil
    {
        public static string GetContentEmail(DateTime timeBeforeSending, string targetEmail)
        {
            Pop3Client popClient = null;            
            try
            {
                bool flagWaiting = false;
                RetryPolicy waitingEmail = Policy
                .Handle<Exception>()
                .WaitAndRetry(1, retryAttempt => TimeSpan.FromSeconds(ConfigurationManager.Configuration.GetIntParam("waits:polly:emailWeiting")),onRetry:(exception, retryCount) => {
                    Log.Info($"Weiting email {ConfigurationManager.Configuration.GetIntParam("waits:polly:emailWeiting")} seconds.");
                    flagWaiting = true;
                });
                waitingEmail.Execute(() =>
                {
                    if (!flagWaiting)
                        throw new Exception();
                });
                popClient = new Pop3Client();                
                popClient.Connect(
                    ConfigurationManager.Configuration.GetStringParam("emailSetting:pop3:server"),
                    ConfigurationManager.Configuration.GetIntParam("emailSetting:pop3:port"),
                    ConfigurationManager.Configuration.GetBooleanParam("emailSetting:pop3:ssl") ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);
                popClient.Authenticate(
                    ConfigurationManager.Configuration.GetStringParam("emailSetting:user:login"),
                    ConfigurationManager.Configuration.GetStringParam("emailSetting:user:password"));
                int ir = popClient.Count;               
                for (int i = 0; i < popClient.Count; i++)
                {
                    var mailMessage = popClient.GetMessage(i);                    
                    if (mailMessage.Date >= timeBeforeSending && mailMessage.From.ToString().Contains(targetEmail))
                    {
                        Log.Info($"Email from {mailMessage.From} which sent {mailMessage.Date} has been found.");                       
                        return mailMessage.HtmlBody;
                    }                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during working with emails.");
                return String.Empty;
            }
            finally
            {
                popClient.Disconnect(true);
                Console.WriteLine($"Finaly");
            }
            return String.Empty;
        }
    }
}
