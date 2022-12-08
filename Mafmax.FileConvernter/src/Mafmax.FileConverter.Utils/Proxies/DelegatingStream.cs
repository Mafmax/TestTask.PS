
namespace Mafmax.FileConverter.Utils.Proxies
{
    /// <summary>
    /// Represents a Stream that delegates all of its operations to a wrapped Stream.
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    public class DelegatingStream : Stream
    {
        private readonly Stream _wrappedStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatingStream"/> class.
        /// </summary>
        /// <param name="wrappedStream">The wrapped stream.</param>
        internal DelegatingStream(Stream wrappedStream)
        {
            _wrappedStream = wrappedStream;
        }

        /// <inheritdoc/>
        public override bool CanRead => _wrappedStream.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => _wrappedStream.CanSeek;

        /// <inheritdoc/>
        public override bool CanTimeout => _wrappedStream.CanTimeout;

        /// <inheritdoc/>
        public override bool CanWrite => _wrappedStream.CanWrite;

        /// <inheritdoc/>
        public override long Length => _wrappedStream.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => _wrappedStream.Position;
            set => _wrappedStream.Position = value;
        }

        /// <inheritdoc/>
        public override int ReadTimeout
        {
            get => _wrappedStream.ReadTimeout;
            set => _wrappedStream.ReadTimeout = value;
        }

        /// <inheritdoc/>
        public override int WriteTimeout
        {
            get => _wrappedStream.WriteTimeout;
            set => _wrappedStream.WriteTimeout = value;
        }

        // methods
        /// <inheritdoc/>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
        {
            return _wrappedStream.BeginRead(buffer, offset, count, callback, state);
        }

        /// <inheritdoc/>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
        {
            return _wrappedStream.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <inheritdoc/>
        public override void Close()
        {
            _wrappedStream.Close();
        }

        /// <inheritdoc/>
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _wrappedStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("CreateWaitHandle has been deprecated. Use the ManualResetEvent(false) constructor instead.")]
        protected override WaitHandle CreateWaitHandle()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _wrappedStream.Dispose();
            }
        }

        /// <inheritdoc/>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return _wrappedStream.EndRead(asyncResult);
        }

        /// <inheritdoc/>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            _wrappedStream.EndWrite(asyncResult);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return _wrappedStream.Equals(obj);
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            _wrappedStream.Flush();
        }

        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _wrappedStream.FlushAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _wrappedStream.GetHashCode();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _wrappedStream.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _wrappedStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        /// <inheritdoc/>
        public override int ReadByte()
        {
            return _wrappedStream.ReadByte();
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _wrappedStream.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            _wrappedStream.SetLength(value);
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return _wrappedStream.ToString();
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _wrappedStream.Write(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _wrappedStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        /// <inheritdoc/>
        public override void WriteByte(byte value)
        {
            _wrappedStream.WriteByte(value);
        }
    }
}
