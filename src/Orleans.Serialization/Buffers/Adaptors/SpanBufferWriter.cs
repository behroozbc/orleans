using System;
using System.Buffers;

namespace Orleans.Serialization.Buffers.Adaptors
{
    /// <summary>
    /// A special-purpose <see cref="IBufferWriter{T}"/> implementation for supporting <see cref="Span{T}"/> in <see cref="Writer{TBufferWriter}"/>.
    /// </summary>
    public struct SpanBufferWriter : IBufferWriter<byte>
    {
        private readonly int _maxLength;
        private int _bytesWritten;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpanBufferWriter"/> struct.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        internal SpanBufferWriter(Span<byte> buffer)
        {
            _maxLength = buffer.Length;
            _bytesWritten = 0;
        }

        /// <summary>
        /// Gets the number of bytes written.
        /// </summary>
        /// <value>The number of bytes written.</value>
        public int BytesWritten => _bytesWritten;

        /// <inheritdoc />
        public void Advance(int count) => _bytesWritten += count;

        /// <inheritdoc />
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (_bytesWritten + sizeHint > _maxLength)
            {
                ThrowInsufficientCapacity(sizeHint);
            }

            ThrowNotSupported();
            return default;

            static void ThrowNotSupported() => throw new NotSupportedException("Method is not supported on this instance");
        }

        /// <inheritdoc />
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            if (_bytesWritten + sizeHint > _maxLength)
            {
                ThrowInsufficientCapacity(sizeHint);
            }

            ThrowNotSupported();
            return default;

            static void ThrowNotSupported() => throw new NotSupportedException("Method is not supported on this instance");
        }

        private void ThrowInsufficientCapacity(int sizeHint) => throw new InvalidOperationException($"Insufficient capacity to perform the requested operation. Buffer size is {_maxLength}. Current length is {_bytesWritten} and requested size increase is {sizeHint}");
    }
}
