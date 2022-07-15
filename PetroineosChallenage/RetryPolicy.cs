using PetroineosChallenage.Settings;
using Polly;
using Polly.Retry;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage
{
    /// <summary>
    /// The retry policy ensure maxium resiliency
    /// My preference is not to stop/fail the service, but to recover the service whenever possible
    /// If the error persists after retry, then some sort of notification shold be sent (could be done via logger)
    /// </summary>
    public class RetryPolicy : IRetryPolicy
    {
        protected readonly AsyncRetryPolicy _retryPolicy;

        public RetryPolicy(ILogger<RetryPolicy> logger, RetrySettings retrySettings)
        {

            // can add other exceptions that worth retrying
            // currently this is exponential back-off for 5 times for total 62 seconds
            _retryPolicy = Policy.Handle<PowerServiceException>()
            .WaitAndRetryAsync(retrySettings.RetryCount, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, sleepDuration, retryCount, context) =>
            {
                if (retryCount == retrySettings.RetryCount)
                {
                    logger.LogError(exception, $"Retry count {retryCount} reached, will give up");
                }
                else
                {
                    logger.LogWarning(exception, $"Waiting {sleepDuration} before retry attempt {retryCount}");
                }
            });
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken)
        {
            return await _retryPolicy.ExecuteAsync<TResult>(action, cancellationToken);
        }
    }
}
