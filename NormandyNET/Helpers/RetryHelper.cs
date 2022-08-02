using System;
using System.Threading.Tasks;

namespace NormandyNET
{
    public static class RetryHelper
    {
        public static void RetryOnException(int times, TimeSpan delay, Action operation)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                        throw;

                    Task.Delay(delay).Wait();
                }
            } while (true);
        }
    }
}