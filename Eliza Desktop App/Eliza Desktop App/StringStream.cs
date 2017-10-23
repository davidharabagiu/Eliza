using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Eliza_Desktop_App
{
    public class StringStream
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StringStream(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string Read()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] buffer = new byte[len];
            ioStream.Read(buffer, 0, len);

            return streamEncoding.GetString(buffer);
        }

        public void Write(string outString)
        {
            byte[] buffer = streamEncoding.GetBytes(outString);
            int len = buffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len >> 8));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(buffer, 0, len);
            ioStream.Flush();
        }
    }
}
