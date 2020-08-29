using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gravs.AbstractDelay
{
    /// <summary>
    /// Allows testing time delays without actually waiting, instead relying on a controller to move time forward arbitrarily.
    /// </summary>
    /// <example>
    /// <code>
    /// class SlowOperation
    /// {
    ///     private IDelay Time;
    ///     public SlowOperation(IDelay time)
    ///     {
    ///         Time = time;
    ///     }
    ///     
    ///     public SlowOperation()
    ///     : this(TaskDelay.Instance)
    ///     {}
    ///     
    ///     public async Task Work()
    ///     {
    ///         await Time.Delay(1000);
    ///     }
    /// }
    /// 
    /// [TestClass]
    /// class SlowOperationTest
    /// {
    ///     [TestMethod]
    ///     async Task TestWorkCompletes()
    ///     {
    ///         var time = new DelaySimulator();
    ///         var sut = new SlowOperation(time);
    ///         var work = sut.Work();
    ///         Assert.IsFalse(work.IsCompleted);
    ///         time.Advance(1000);
    ///         await work;
    ///         Assert.IsTrue(work.IsCompleted);
    ///     }
    /// }
    /// </code>
    /// </example>
    public class DelaySimulator : IDelay
    {
        private object Lock = new object();
        private long Elapsed;

        /// <summary>
        /// Delay until an equivalent amount of time has been simulated forward via <see cref="Advance(int)"/>.
        /// </summary>
        /// <param name="millisecondsDelay">The length of time to delay, or to delay indefinitely</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete</param>
        /// <returns>A task that waits until the simulator has advanced the required amount of time.</returns>
        public Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            if (millisecondsDelay < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
            }
            else if (millisecondsDelay == -1)
            {
                return Task.Delay(-1, cancellationToken);
            }

            long target;
            lock (Lock)
            {
                target = Elapsed + millisecondsDelay;
            }
            return Task.Run(() =>
            {
                lock (Lock)
                {
                    using (cancellationToken.Register(() =>
                    {
                        lock (Lock) Monitor.PulseAll(Lock);
                    }))
                    {
                        while (Elapsed < target)
                        {
                            Monitor.Wait(Lock);
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                    }
                }
            },
            cancellationToken);
        }

        /// <summary>
        /// Pass a given amount of time instantly.
        /// Tasks from <see cref="Delay"/> that are still waiting may complete if enough time has "passed".
        /// </summary>
        /// <param name="millisecondsTime">Non-negative amount of milliseconds to progress time on this object</param>
        public void Advance(int millisecondsTime)
        {
            if (millisecondsTime < 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondsTime));

            lock (Lock)
            {
                Elapsed += millisecondsTime;
                Monitor.PulseAll(Lock);
            }
        }
    }
}
