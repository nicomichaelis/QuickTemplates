using System;
using System.IO;

namespace Michaelis.QuickTemplates;

public class ChangeStream : Stream
{
    enum ChangeModes { Damaged, Compare, WriteThrough }

    bool _disposed;
    ChangeModes _changeMode;
    byte[] _buffer;
    int _bufferFill;
    int _bufferPos;
    Stream BaseStream { get; set; }
    readonly bool _leaveOpen;

    public ChangeStream(Stream inoutStream, bool leaveOpen) : this(inoutStream, 4096, leaveOpen)
    {
    }

    public ChangeStream(Stream inoutStream, int bufferSize, bool leaveOpen)
    {
        _leaveOpen = leaveOpen;
        if (inoutStream == null) throw new ArgumentNullException(nameof(inoutStream));
        if (!(inoutStream.CanRead && inoutStream.CanWrite && inoutStream.CanSeek))
            throw new ArgumentOutOfRangeException(nameof(inoutStream), $"Parameter {nameof(inoutStream)} must support Read, Write and Seek.");
        if (bufferSize < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));
        BaseStream = inoutStream;
        _changeMode = ChangeModes.Compare;
        _buffer = new byte[bufferSize];
    }

    public bool Updated { get; protected set; } = false;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => !_disposed;

    public override long Length => Position;

    public override long Position
    {
        get
        {
            EnsureAllGood();
            return BaseStream.Position - (_bufferFill - _bufferPos);
        }
        set => throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Flush()
    {
        EnsureAllGood();
        if (BaseStream != null)
        {
            SeekReset();
        }
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        EnsureAllGood();
        if (buffer.Length < count + offset) throw new ArgumentOutOfRangeException(nameof(buffer), "buffer.Length > count + offset");
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
        try
        {
            while (_changeMode == ChangeModes.Compare && count > 0)
            {
                if (!(EnsureBuffer() && CompareBuffer(buffer, ref offset, ref count)))
                {
                    SeekReset();
                    _changeMode = ChangeModes.WriteThrough;
                    _buffer = null;
                }
            }
        }
        catch (Exception)
        {
            _changeMode = ChangeModes.Damaged;
            throw;
        }

        if (_changeMode == ChangeModes.WriteThrough)
        {
            Updated = Updated | count > 0;
            BaseStream.Write(buffer, offset, count);
        }
    }

    void EnsureAllGood()
    {
        if (_changeMode == ChangeModes.Damaged) throw new InvalidOperationException("There was an exception during a prior write operation. Further operations are not supported.");
        if (BaseStream == null) throw new ObjectDisposedException(nameof(ChangeStream));
    }

    void SeekReset()
    {
        int offset = _bufferFill - _bufferPos;
        if (offset != 0)
        {
            BaseStream.Seek(-offset, SeekOrigin.Current);
        }
        _bufferFill = 0;
        _bufferPos = 0;
    }

    bool EnsureBuffer()
    {
        if (_bufferFill <= _bufferPos)
        {
            _bufferFill = BaseStream.Read(_buffer, 0, _buffer.Length);
            _bufferPos = 0;
            return _bufferFill > 0;
        }
        return true;
    }

    bool CompareBuffer(byte[] buffer, ref int roffset, ref int rcount)
    {
        int offset = roffset;
        int dCount = Math.Min(rcount, _bufferFill - _bufferPos);
        bool result = true;
        while (dCount > 0 & result)
        {
            result = buffer[offset++] == _buffer[_bufferPos++];
            dCount--;
        }

        if (!result)
        {
            _bufferPos--;
            offset--;
        }

        int delta = offset - roffset;
        roffset = offset;
        rcount = rcount - delta;
        return result;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (BaseStream != null) && !_disposed)
        {
            _disposed = true;
            try
            {
                if (_changeMode == ChangeModes.Compare)
                {
                    SeekReset();
                }

                if (_changeMode != ChangeModes.Damaged && !_leaveOpen)
                {
                    if (BaseStream.Position != BaseStream.Length) BaseStream.SetLength(BaseStream.Position);
                }
            }
            finally
            {
                try
                {
                    if (!_leaveOpen) BaseStream.Dispose();
                }
                finally
                {
                    BaseStream = null;
                    _buffer = null;
                }
            }
        }
    }
}
