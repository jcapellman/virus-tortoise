using System;
using System.Linq;

using virus_tortoise.lib.Parsers.Base;

namespace virus_tortoise.lib.Parsers
{
    public class PNGParser : BaseParser
    {
        public class IHDR
        {
            public Int32 Width;

            public Int32 Height;

            public byte BitDepth;

            public byte ColorType;

            public byte Compression;
            public byte FilterMethod;

            public byte Interlace;

            public IHDR(byte[] data)
            {
                Width = BitConverter.ToInt32(data, 8);
                Height = BitConverter.ToInt32(data, 12);
            }
        }

        private byte[] FileMagicBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        public override string FileType => "PNG";

        public override bool IsFile(byte[] data) => data.AsSpan().Slice(0, FileMagicBytes.Length).SequenceEqual(FileMagicBytes);

        public override bool IsValid(byte[] data)
        {
            // Skip first 8 bytes
            // Read 4 chunks to get size

            // Loop through next 4 bytes
            var header = new IHDR(data);

            return true;
        }
    }
}