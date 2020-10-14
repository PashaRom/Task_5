using System;
using Polly;
using Polly.Retry;
using Test.Framework.Logging;
namespace Test.Framework.Waits
{
    public static class PollyWait
    {
        public static RetryPolicy WaitAndRetryNullReferenceException(int numberWaiting, int time)
        {
            try
            {
                RetryPolicy policy = Policy
                .Handle<NullReferenceException>()
                .WaitAndRetry(numberWaiting, retryAttempt => TimeSpan.FromSeconds(time));
                return policy;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during ExplicitWaitElementIsVisible.");
                throw new NullReferenceException();
            }
        }
    }
}
