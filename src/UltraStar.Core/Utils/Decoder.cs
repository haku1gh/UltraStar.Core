#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Threading;
using UltraStar.Core.ThirdParty.Serilog;

namespace UltraStar.Core.Utils
{
    /// <summary>
    /// Represents an abstract decoder class.
    /// </summary>
    /// <typeparam name="T">The type of elements to be decoded.</typeparam>
    public abstract class Decoder<T> : IDisposable
    {
        // Private variables
        private IRingBuffer<T> buffer;
        private Thread workerThread;
        private EventWaitHandle sleepWaitHandle;        // Sleep wait handle
        private readonly int nonOverwritingItems;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="Decoder{T}"/>.
        /// </summary>
        /// <param name="minimumBufferSize">The minimum size of the internal buffer.</param>
        /// <param name="nonOverwritingItems">
        /// The number of items which can be retrieved with <see cref="NextItem"/> before any of these items will be overwritten.
        /// The minimum value is 1.
        /// </param>
        public Decoder(int minimumBufferSize, int nonOverwritingItems)
        {
            if (nonOverwritingItems < 1) nonOverwritingItems = 1;
            this.nonOverwritingItems = nonOverwritingItems;
            buffer = new RingBuffer<T>(minimumBufferSize + nonOverwritingItems);
            DecoderRunning = false;
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~Decoder()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes the decoder.
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                // Special for Close() and Dispose()
                if (disposing)
                    GC.SuppressFinalize(this);
                stopThread();
                buffer = null;
                workerThread = null;
                sleepWaitHandle = null;
            }
        }

        /// <summary>
        /// Starts the internal thread.
        /// </summary>
        protected void startThread(string threadName)
        {
            // Initialize wait handles, queues and threads
            sleepWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            workerThread = new Thread(backgroundTask)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal,
                Name = threadName
            };
            // Start threads
            lock (buffer) DecoderRunning = true;
            workerThread.Start();
        }

        /// <summary>
        /// Stops the internal thread.
        /// </summary>
        /// <returns><see langword="true"/> if the thread is stopped; otherwise <see langword="false"/> to indicate an error.</returns>
        protected bool stopThread()
        {
            if ((workerThread.ThreadState == ThreadState.Stopped) || (workerThread.ThreadState == ThreadState.Unstarted))
                return true;
            bool returnValue = false;
            lock (buffer) DecoderRunning = false;
            try
            {
                lock (sleepWaitHandle)
                {
                    sleepWaitHandle.Set(); // This means, wake up there is something to do
                }
                returnValue = workerThread.Join(500);
            }
            catch { }
            return returnValue;
        }

        /// <summary>
        /// The background task running in a separate thread.
        /// </summary>
        private void backgroundTask()
        {
            try
            {
                while (true)
                {
                    // Check if buffer is full, if yes sleep
                    bool bufferFull = false;
                    lock (buffer)
                    {
                        if (!DecoderRunning) break;
                        bufferFull = (buffer.Count == (buffer.Size - nonOverwritingItems));
                        if (bufferFull)
                        {
                            lock (sleepWaitHandle)
                            {
                                sleepWaitHandle.Reset();
                            }
                        }
                    }
                    if (bufferFull)
                    {
                        sleepWaitHandle.WaitOne();
                        continue;
                    }
                    // Get the element which will be overwritten next
                    T entry;
                    lock (buffer)
                    {
                        if (!DecoderRunning) break;
                        entry = buffer[buffer.Size - 1];
                    }
                    // Update entry
                    bool success = addItemToBuffer(ref entry);
                    // Push entry back to buffer
                    lock(buffer)
                    {
                        if (!success) DecoderRunning = false;
                        if (!DecoderRunning) break;
                        buffer.Push(entry);
                    }
                }
            }
            // Will be raised when the thread terminates
            catch (ThreadAbortException)
            { }
            // Will be raised when the thread is interrupted while waiting
            catch (ThreadInterruptedException)
            { }
            // For any other kind of exceptions
            catch (Exception e)
            {
                // TODO: Add error logic here
                Log.Error("Unresolvable error occurred in {ThreadName}. Exception: {Exception}. Stacktrace: {Stacktrace}.", workerThread.Name, e.Message, e.StackTrace);
            }
            lock (buffer) DecoderRunning = false;
        }

        /// <summary>
        /// Adds a new item to the buffer.
        /// </summary>
        /// <param name="entry">The old entry at the position where the item will be added. This entry shall be modified by this method.</param>
        /// <returns>
        /// <see langword="true"/> if the next item could be set;
        /// otherwise <see langword="false"/> to indicate an error or EOF and the termination of the thread.
        /// </returns>
        protected abstract bool addItemToBuffer(ref T entry);

        /// <summary>
        /// Resizes the internal buffer.
        /// </summary>
        /// <remarks>
        /// This method should only be called before the internal thread had been started.
        /// </remarks>
        /// <param name="minimumBufferSize">The minimum size of the internal buffer.</param>
        protected void resizeBuffer(int minimumBufferSize)
        {
            lock (buffer) buffer.Resize(minimumBufferSize + nonOverwritingItems);
        }

        /// <summary>
        /// Gets an indicator whether the internal decoder is running or has stopped.
        /// </summary>
        public bool DecoderRunning { get; private set; }

        /// <summary>
        /// Gets an indicator whether one or more items can be retrieved from the decoder.
        /// </summary>
        public bool ItemsAvailable
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    return false;
                // Return value
                bool returnValue;
                lock (buffer) returnValue = (buffer.Count > 0);
                return returnValue;
            }
        }

        /// <summary>
        /// Gets an indicator whether the decoder buffer is full; and therefore idling.
        /// </summary>
        public bool BufferFull
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    return false;
                // Return value
                bool returnValue;
                lock (buffer) returnValue = (buffer.Count == (buffer.Size - nonOverwritingItems));
                return returnValue;
            }
        }

        /// <summary>
        /// Gets the current number of items in the decoder buffer.
        /// </summary>
        public int ItemsCount
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    return 0;
                // Return value
                int returnValue;
                lock (buffer) returnValue = buffer.Count;
                return returnValue;
            }
        }

        /// <summary>
        /// Returns the next item from the decoder and removes this item from the buffer.
        /// </summary>
        /// <returns>Returns the requested item.</returns>
        /// <exception cref="IndexOutOfRangeException">No item is available. Check with <see cref="ItemsAvailable"/> before calling this method.</exception>
        public T NextItem()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(Decoder<T>));
            // Return value
            T returnItem;
            lock (buffer)
            {
                if (buffer.Count == 0)
                    throw new IndexOutOfRangeException("No item available in the decoder.");
                returnItem = buffer.Pop();
            }
            lock (sleepWaitHandle)
            {
                sleepWaitHandle.Set(); // This means, wake up there is something to do
            }
            return returnItem;
        }

        /// <summary>
        /// Returns the next items from the decoder and removes all these items from the buffer.
        /// </summary>
        /// <param name="maxCount">The maximum number of items to return.</param>
        /// <returns>An array containing the requested items.</returns>
        public T[] NextItems(int maxCount)
        {
            // Check if disposed
            if (isDisposed)
                return null;
            // Return value
            T[] returnArray;
            lock (buffer)
            {
                int count = (maxCount < buffer.Count) ? maxCount : buffer.Count;
                returnArray = new T[count];
                for (int i = 0; i < count; i++)
                    returnArray[i] = buffer.Pop();
            }
            lock (sleepWaitHandle)
            {
                sleepWaitHandle.Set(); // This means, wake up there is something to do
            }
            return returnArray;
        }

        /// <summary>
        /// Returns the next item from the decoder, but does not remove this item from the buffer.
        /// </summary>
        /// <returns>Returns the requested item.</returns>
        /// <exception cref="IndexOutOfRangeException">No item is available. Check with <see cref="ItemsAvailable"/> before calling this method.</exception>
        public T PeekNextItem()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(Decoder<T>));
            // Return value
            T returnItem;
            lock (buffer)
            {
                if (buffer.Count == 0)
                    throw new IndexOutOfRangeException("No item available in the decoder.");
                returnItem = buffer.Last;
            }
            return returnItem;
        }

        /// <summary>
        /// Returns the next items from the decoder, but does not remove any of these items from the buffer.
        /// </summary>
        /// <param name="maxCount">The maximum number of items to return.</param>
        /// <returns>An array containing the requested items.</returns>
        public T[] PeekNextItems(int maxCount)
        {
            // Check if disposed
            if (isDisposed)
                return null;
            // Return value
            T[] returnArray;
            lock (buffer)
            {
                int count = (maxCount < buffer.Count) ? maxCount : buffer.Count;
                returnArray = buffer.ToArray(count);
            }
            return returnArray;
        }
    }
}
