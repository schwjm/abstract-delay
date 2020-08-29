using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gravs.AbstractDelay
{
    /// <summary>
    /// Ignore all requests to delay.
    /// </summary>
    class NoDelay : IDelay
    {
        /// <summary>
        /// No-op.
        /// </summary>
        /// <param name="millisecondsDelay">Must be a number greater than or equal to -1</param>
        /// <param name="cancellationToken">No effect</param>
        /// <returns><see cref="Task.CompletedTask"/></returns>
        public Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            if (millisecondsDelay < -1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NoDelay Instance => _Instance.Value;
        private NoDelay() { }
        private static readonly Lazy<NoDelay> _Instance = new Lazy<NoDelay>(() => new NoDelay());
    }
}
