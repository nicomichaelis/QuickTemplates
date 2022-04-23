using System;
using System.IO;

namespace Michaelis.QuickTemplates;

public class ChangeStream : Stream
{
    enum ChangeModes { Damaged, Compare, WriteThrough }

    bool Disposed { get; set; }

    ChangeModes ChangeMode { get; set; }

    byte[] m_Buffer;
    int m_BufferFill;
    int m_BufferPos;
    Stream m_BaseStream;
    bool m_LeaveOpen;

    public ChangeStream(Stream inoutStream, bool leaveOpen) : this(inoutStream, 4096, leaveOpen)
    {
    }

    public ChangeStream(Stream inoutStream, int bufferSize, bool leaveOpen)
    {
        m_LeaveOpen = leaveOpen;
        if (inoutStream == null) throw new ArgumentNullException(nameof(inoutStream));
        if (!(inoutStream.CanRead && inoutStream.CanWrite && inoutStream.CanSeek))
            throw new ArgumentOutOfRangeException(nameof(inoutStream), $"Parameter {nameof(inoutStream)} must support Read, Write and Seek.");
        if (bufferSize < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));
        m_BaseStream = inoutStream;
        ChangeMode = ChangeModes.Compare;
        m_Buffer = new byte[bufferSize];
    }

    public bool Updated { get; protected set; } = false;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => !Disposed;

    public override long Length => Position;

    public override long Position
    {
        get
        {
            EnsureAllGood();
            return m_BaseStream.Position - (m_BufferFill - m_BufferPos);
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
        if (m_BaseStream != null)
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
            while (ChangeMode == ChangeModes.Compare && count > 0)
            {
                if (!(EnsureBuffer() && CompareBuffer(buffer, ref offset, ref count)))
                {
                    SeekReset();
                    ChangeMode = ChangeModes.WriteThrough;
                    m_Buffer = null;
                }
            }
        }
        catch (Exception)
        {
            ChangeMode = ChangeModes.Damaged;
            throw;
        }

        if (ChangeMode == ChangeModes.WriteThrough)
        {
            Updated = Updated | count > 0;
            m_BaseStream.Write(buffer, offset, count);
        }
    }

    private void EnsureAllGood()
    {
        if (ChangeMode == ChangeModes.Damaged) throw new InvalidOperationException("There was an exception during a prior write operation. Further operations are not supported.");
        if (m_BaseStream == null) throw new ObjectDisposedException(nameof(ChangeStream));
    }

    private void SeekReset()
    {
        int offset = m_BufferFill - m_BufferPos;
        if (offset != 0)
        {
            m_BaseStream.Seek(-offset, SeekOrigin.Current);
        }
        m_BufferFill = 0;
        m_BufferPos = 0;
    }

    private bool EnsureBuffer()
    {
        if (m_BufferFill <= m_BufferPos)
        {
            m_BufferFill = m_BaseStream.Read(m_Buffer, 0, m_Buffer.Length);
            m_BufferPos = 0;
            return m_BufferFill > 0;
        }
        return true;
    }

    private bool CompareBuffer(byte[] buffer, ref int roffset, ref int rcount)
    {
        int offset = roffset;
        int dCount = Math.Min(rcount, m_BufferFill - m_BufferPos);
        bool result = true;
        while (dCount > 0 & result)
        {
            result = buffer[offset++] == m_Buffer[m_BufferPos++];
            dCount--;
        }

        if (!result)
        {
            m_BufferPos--;
            offset--;
        }

        int delta = offset - roffset;
        roffset = offset;
        rcount = rcount - delta;
        return result;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (m_BaseStream != null))
        {
            try
            {
                if (ChangeMode == ChangeModes.Compare)
                {
                    SeekReset();
                }

                if (ChangeMode != ChangeModes.Damaged && !m_LeaveOpen)
                {
                    if (m_BaseStream.Position != m_BaseStream.Length) m_BaseStream.SetLength(m_BaseStream.Position);
                }
            }
            finally
            {
                try
                {
                    if (!m_LeaveOpen) m_BaseStream.Dispose();
                }
                finally
                {
                    m_BaseStream = null;
                    m_Buffer = null;
                }
            }
        }
    }
}
