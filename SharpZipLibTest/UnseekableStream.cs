using System.Diagnostics;
using System.IO;

namespace SharpZipLibTest
{
    public class UnseekableStream : Stream
    {
        public static UnseekableStream Create(string filePath)
        {
            var stream = File.Create(filePath);
            return new UnseekableStream(stream);
        }

        readonly Stream m_stream;

        UnseekableStream(Stream stream)
        {
            //Debug.WriteLine($"MyStream.MyStream()");
            m_stream = stream;
        }

        ~UnseekableStream()
        {
            m_stream.Close();
        }

        public override void Close()
        {
            Debug.WriteLine($"MyStream.Close()");
            m_stream.Close();
        }

        public override bool CanRead => false;

        public override bool CanSeek
        {
            get
            {
                Debug.WriteLine("MyStream.CanSeek() => false");
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                //Debug.WriteLine("MyStream.CanWrite() => true");
                return true;
            }
        }

        public override long Length => m_stream.Length;

        public override long Position { get => m_stream.Position; set => m_stream.Position = value; }

        public override void Flush()
        {
            Debug.WriteLine("MyStream.Flush() 無視します。");
            //m_fileStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            m_stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
#if false
            Debug.Write("MyStream.Write({");
            for (int i = 0; i < count; i++)
            {
                Debug.Write($"{buffer[offset + i]:X2}, ");
            }
            Debug.WriteLine("},,)");
#endif
            m_stream.Write(buffer, offset, count);
        }
    }
}
