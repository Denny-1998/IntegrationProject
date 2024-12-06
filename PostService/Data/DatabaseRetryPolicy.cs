using Polly;
using Polly.Retry;
using Serilog;

namespace PostService.Data
{
    public class DatabaseRetryPolicy
    {
        public static AsyncRetryPolicy CreateRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Log.Warning(
                            exception,
                            "Error executing database operation (Attempt {RetryCount}). Waiting {TimeSpan} before retrying.",
                            retryCount,
                            timeSpan);
                    }
                );
        }
    }
}