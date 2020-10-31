using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gravitome.AbstractDelay
{
    /// <summary>
    /// Pass-through to <see cref="Task.Delay(int, CancellationToken)"/> calls.
    /// </summary>
    public class TaskDelay : IDelay
    {
        /// <summary>
        /// As <seealso cref="Task.Delay(int, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="millisecondsDelay"></param>
        /// <returns></returns>
        public Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            return Task.Delay(millisecondsDelay, cancellationToken);
        }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static TaskDelay Instance => _Instance.Value;
        private TaskDelay() { }
        private static readonly Lazy<TaskDelay> _Instance = new Lazy<TaskDelay>(() => new TaskDelay());
    }
}
