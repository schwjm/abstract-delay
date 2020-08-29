using System.Threading;
using System.Threading.Tasks;

namespace Gravs.AbstractDelay
{
    /// <summary>
    /// An interface to requesting a delay similar to <see cref="Task.Delay(int, CancellationToken)"/>.
    /// Some implementations may choose to bend time to their own reality.
    /// </summary>
    public interface IDelay
    {
        /// <summary>
        /// Request a delay similar to <see cref="Task.Delay(int, CancellationToken)"/>.
        /// </summary>
        /// <param name="millisecondsDelay">The length of time to delay</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete</param>
        /// <returns>A task that represents the time delay.</returns>
        Task Delay(int millisecondsDelay, CancellationToken cancellationToken);

        /// <summary>
        /// Request a delay similar to <see cref="Task.Delay(int)"/>.
        /// </summary>
        /// <param name="millisecondsDelay">The length of time to delay</param>
        /// <returns>A task that represents the time delay.</returns>
        Task Delay(int millisecondsDelay)
        {
            return Delay(millisecondsDelay, CancellationToken.None);
        }
    }
}
