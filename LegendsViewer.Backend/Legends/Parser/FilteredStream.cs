namespace LegendsViewer.Backend.Legends.Parser;

public class FilteredStream : Stream
{
    private readonly Stream _baseStream;

    public FilteredStream(Stream baseStream)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
    }

    public override bool CanRead => _baseStream.CanRead;

    public override bool CanSeek => _baseStream.CanSeek;

    public override bool CanWrite => _baseStream.CanWrite;

    public override long Length => _baseStream.Length;

    public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        byte[] tempBuffer = new byte[count];
        int bytesRead = _baseStream.Read(tempBuffer, 0, count);

        if (bytesRead == 0) return 0;

        int wrote = 0;
        for (int i = 0; i < bytesRead; i++)
        {
            if (tempBuffer[i] < 32)
            {
                // Replace non-printable characters with a space (ASCII 32)
                buffer[offset + wrote] = (byte)' ';
            }
            else
            {
                buffer[offset + wrote] = tempBuffer[i];
            }

            wrote++;
        }

        return wrote;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _baseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);
    }
}